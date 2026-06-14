using UnityEngine;

/// <summary>
/// PainterController
/// Controla la acción básica del jugador Painter.
/// 
/// En esta fase del prototipo:
/// - Detecta el clic izquierdo del ratón.
/// - Instancia un prefab en la posición del cursor en el mundo.
/// 
/// No gestiona lógica de cooldown ni parámetros avanzados.
/// Su función actual es servir como base para integrar
/// en el futuro el AbilitySystem y perfiles parametrizados.
/// 
/// Representa la capa Logic asociada al rol Painter.
/// </summary>

public class PainterController : MonoBehaviour
{
    [SerializeField] private GameObject abilityPrefab;
    [SerializeField] private Camera mainCamera;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpawnAbility();
        }
    }

    private void SpawnAbility()
    {
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Instantiate(abilityPrefab, mousePos, Quaternion.identity);
    }
}