using UnityEngine;

/// <summary>
/// RunnerMovementSystem2D
/// Controla el movimiento físico del Runner usando Rigidbody2D.
///
/// Incluye:
/// - Movimiento horizontal con aceleración y frenado.
/// - Salto con impulso.
/// - Coyote Time: permite saltar un instante después de dejar el suelo.
/// - Jump Buffer: guarda el salto pulsado un instante antes de tocar suelo.
/// - Salto variable: si sueltas el botón, el salto se acorta.
/// - Caída más rápida: multiplica gravedad al caer para mejor "feeling".
/// - Ground check por BoxCast contra la capa Ground.
///
/// Nota técnica:
/// - Update() gestiona input/timers y lógica.
/// - FixedUpdate() aplica movimiento físico (Rigidbody).
///
/// Permite bloqueo externo con CanMove (por ejemplo DragSystem).
/// </summary>


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class RunnerMovementSystem2D : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float maxSpeed = 7f;
    [SerializeField] private float accel = 90f;
    [SerializeField] private float decel = 110f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 11f;
    [SerializeField] private float coyoteTime = 0.10f;
    [SerializeField] private float jumpBuffer = 0.10f;

    [Header("Gravity Tuning")]
    [SerializeField] private float fallGravityMultiplier = 2.2f;
    [SerializeField] private float lowJumpMultiplier = 2.0f;

    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckExtra = 0.06f;

    public bool IsGrounded { get; private set; }

    // Permite a otros sistemas (como DragSystem) bloquear el movimiento temporalmente.
    public bool CanMove { get; set; } = true;

    private Rigidbody2D rb;
    private BoxCollider2D col;
    private LocalRunnerInput input;

    private float coyoteTimer; // Temporizador que permite saltar tras abandonar el suelo.
    private float bufferTimer; // Temporizador que guarda un salto pulsado anticipadamente.

    private void Awake()
    {
        // Cacheo de componentes para evitar llamadas repetidas a GetComponent().
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();

        input = GetComponent<LocalRunnerInput>();
        if (input == null)
            input = gameObject.AddComponent<LocalRunnerInput>();
    }

    private void Update()
    {
        // Si otro sistema bloquea movimiento, salimos.
        if (!CanMove) return;

        IsGrounded = CheckGrounded();

        // Gestión del Coyote Time
        coyoteTimer = IsGrounded ? coyoteTime : Mathf.Max(0f, coyoteTimer - Time.deltaTime);

        // Gestión del Jump Buffer
        if (input.JumpPressed) bufferTimer = jumpBuffer;
        else bufferTimer = Mathf.Max(0f, bufferTimer - Time.deltaTime);

        // Condición real de salto
        if (bufferTimer > 0f && coyoteTimer > 0f)
        {
            DoJump();
            bufferTimer = 0f;
            coyoteTimer = 0f;
            input.ConsumeJumpPressed();
        }

        // Gravedad extra al caer (mejora sensación de peso)
        if (rb.linearVelocity.y < 0f)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallGravityMultiplier - 1f) * Time.deltaTime;
        }
        // Salto variable: si se suelta el botón antes, se reduce altura
        else if (rb.linearVelocity.y > 0f && !input.JumpHeld)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1f) * Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (!CanMove) return;

        // Velocidad deseada según input
        float targetSpeed = input.MoveX * maxSpeed;
        float speedDiff = targetSpeed - rb.linearVelocity.x;

        // Si hay input usamos aceleración, si no frenado
        float rate = Mathf.Abs(targetSpeed) > 0.01f ? accel : decel;
        float movement = speedDiff * rate * Time.fixedDeltaTime;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x + movement, rb.linearVelocity.y);
        rb.linearVelocity = new Vector2(Mathf.Clamp(rb.linearVelocity.x, -maxSpeed, maxSpeed), rb.linearVelocity.y);
    }

    private void DoJump()
    {
        // Reseteamos velocidad vertical para salto consistente
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private bool CheckGrounded()
    {
        Bounds b = col.bounds;

        // Se lanza un BoxCast ligeramente debajo del collider
        Vector2 origin = new Vector2(b.center.x, b.min.y);
        Vector2 size = new Vector2(b.size.x * 0.92f, 0.02f);

        RaycastHit2D hit = Physics2D.BoxCast(
            origin,
            size,
            0f,
            Vector2.down,
            groundCheckExtra,
            groundLayer
        );

        return hit.collider != null;
    }
}