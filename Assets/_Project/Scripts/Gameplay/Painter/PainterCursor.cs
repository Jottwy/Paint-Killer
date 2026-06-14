using UnityEngine;

/// <summary>
/// PainterCursor
/// Representa visualmente el cursor del Painter dentro del mundo 2D.
/// 
/// Sincroniza la posición del objeto con el ratón
/// utilizando ScreenToWorldPoint.
/// 
/// Se utiliza junto con GameManager para ocultar
/// el cursor del sistema operativo.
/// </summary>

public class PainterCursor : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    private void Update()
    {
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x, mousePos.y, 0f);
    }
}