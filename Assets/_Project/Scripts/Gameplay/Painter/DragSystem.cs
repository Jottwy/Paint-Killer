using UnityEngine;

/// <summary>
/// DragSystem (Repasar por nivel de complejidad en caso de no recordar algun valor)
/// Sistema que permite al Painter agarrar y lanzar al Runner con el ratón.
/// 
/// Funciones actuales:
/// - Detecta al Runner con un Raycast al hacer click izquierdo.
/// - Mientras mantengo el click, arrastro al Runner a la posición del ratón.
/// - Bloquea el movimiento del Runner durante el arrastre (runnerMovement.CanMove = false).
/// - Cambia el Rigidbody2D a Kinematic durante el agarre para moverlo “a mano” sin físicas raras.
/// - Al soltar el click (o al acabarse el tiempo), lo vuelve Dynamic y lo lanza con un impulso.
/// - Aplica cooldown proporcional al tiempo que lo he usado (y con un mínimo garantizado).
/// - Devuelve el control al Runner tras un pequeño delay (controlLockTime) para que el lanzamiento se sienta limpio.
/// 
/// Cómo funciona (para entenderlo rápido):
/// 1) Update:
///    - Si estoy arrastrando -> muevo el Runner al ratón.
///    - Si no estoy arrastrando -> compruebo cooldown -> si click -> intento empezar drag.
/// 2) TryStartDrag:
///    - Raycast al ratón -> si toca "Runner" -> guardo Rigidbody2D + RunnerMovementSystem2D.
///    - Inicio temporizador y preparo el estado (Kinematic, gravedad 0, velocidad 0, bloqueo control).
/// 3) ContinueDrag:
///    - Resto tiempo y muevo el objeto a la posición del ratón.
///    - Si suelto click o acaba el tiempo -> StopDrag.
/// 4) StopDrag:
///    - Calculo vector de lanzamiento usando (pos actual - pos inicial).
///    - Vuelvo Dynamic, gravedad normal, aplico AddForce impulso.
///    - Arranco cooldown proporcional.
///    - Programo RestoreControl con Invoke.
/// 
/// Interactúa con:
/// - RunnerMovementSystem2D (bloqueo de control)
/// - CooldownSystem (cooldown de la habilidad)
/// - Camera (para convertir ScreenToWorldPoint)
/// 
/// Nota:
/// Debe tener asignado mainCamera y cooldownSystem en el Inspector,
/// y el Runner debe tener Tag "Runner" + Rigidbody2D + RunnerMovementSystem2D.
/// </summary>
public class DragSystem : MonoBehaviour
{
    [Header("Drag Settings")]
    [SerializeField] private float dragDuration = 2f; // Tiempo máximo que puedo arrastrar
    [SerializeField] private float maxCooldown = 3f; // Cooldown máximo si uso el drag completo
    [SerializeField] private float minCooldown = 0.3f;   // Cooldown mínimo aunque lo use 0.1s
    [SerializeField] private float throwMultiplier = 18f; // Fuerza del lanzamiento
    [SerializeField] private float controlLockTime = 0.25f; // Tiempo extra sin control tras soltar (para “sentir” el lanzamiento)

    [SerializeField] private Camera mainCamera; // Cámara para pasar ratón a mundo
    [SerializeField] private CooldownSystem cooldownSystem; // Sistema central de cooldowns

    private const string DRAG_KEY = "DragAbility"; // Clave de cooldown para esta habilidad

    private Rigidbody2D targetRb; // Rigidbody del Runner agarrado
    private RunnerMovementSystem2D runnerMovement; // Script de movimiento del Runner

    private bool isDragging; // Estado: ¿estoy arrastrando ahora?
    private Vector2 dragStartObjectPos; // Posición inicial para calcular el vector de lanzamiento

    private float dragTimer; // Cuenta atrás del arrastre
    private float timeUsed; // Cuánto tiempo he usado realmente (para cooldown proporcional)

    private void Update()
    {
        // Si estoy arrastrando, no quiero ejecutar nada más.
        if (isDragging)
        {
            ContinueDrag();
            return;
        }

        // Si está en cooldown, no puedo iniciar drag.
        if (cooldownSystem.IsOnCooldown(DRAG_KEY))
            return;

        // Click izquierdo -> intento agarrar.
        if (Input.GetMouseButtonDown(0))
            TryStartDrag();
    }

    private void TryStartDrag()
    {
        // Posición del ratón en el mundo 2D
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        // Raycast a esa posición (Vector2.zero = sin dirección, como “click directo”)
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        // Si hay collider y es el Runner, intento capturar referencias
        if (hit.collider != null && hit.collider.CompareTag("Runner"))
        {
            targetRb = hit.collider.GetComponent<Rigidbody2D>();
            runnerMovement = hit.collider.GetComponent<RunnerMovementSystem2D>();

            // Solo inicio drag si tiene todo lo necesario
            if (targetRb != null && runnerMovement != null)
            {
                isDragging = true;
                dragTimer = dragDuration;
                timeUsed = 0f;

                // Guardo posición inicial para calcular lanzamiento al final
                dragStartObjectPos = targetRb.position;

                // Bloqueo movimiento del Runner
                runnerMovement.CanMove = false;

                // Preparo el cuerpo para arrastre manual
                targetRb.linearVelocity = Vector2.zero;
                targetRb.gravityScale = 0f;
                targetRb.bodyType = RigidbodyType2D.Kinematic;
            }
        }
    }

    private void ContinueDrag()
    {
        // Cuenta hacia atrás y acumula el uso real
        dragTimer -= Time.deltaTime;
        timeUsed += Time.deltaTime;

        // Mueve el Runner al ratón
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        targetRb.transform.position = mousePos;

        // Si suelta click o se acaba el tiempo, termina el drag
        if (Input.GetMouseButtonUp(0) || dragTimer <= 0f)
            StopDrag();
    }

    private void StopDrag()
    {
        if (targetRb != null)
        {
            // Vector de lanzamiento = desde la posición inicial hasta la final
            Vector2 throwVector = targetRb.position - dragStartObjectPos;

            // Reactivar físicas normales
            targetRb.bodyType = RigidbodyType2D.Dynamic;
            targetRb.gravityScale = 1f;

            // Impulsar del lanzamiento
            targetRb.AddForce(throwVector * throwMultiplier, ForceMode2D.Impulse);

            Invoke(nameof(RestoreControl), controlLockTime);
        }

        // Cooldown proporcional al uso:
        // si se usa el 100% del drag -> maxCooldown
        // si se usa poco->menos, pero nunca baja de minCooldown
        float usedRatio = Mathf.Clamp01(timeUsed / dragDuration);
        float proportionalCooldown = maxCooldown * usedRatio;
        float finalCooldown = Mathf.Max(minCooldown, proportionalCooldown);

        cooldownSystem.StartCooldown(DRAG_KEY, finalCooldown);

        // Limpieza de estado
        isDragging = false;
        targetRb = null;
    }

    private void RestoreControl()
    {
        if (runnerMovement != null)
            runnerMovement.CanMove = true;
    }
}