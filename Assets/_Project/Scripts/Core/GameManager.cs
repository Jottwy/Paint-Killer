using UnityEngine;

/// <summary>
/// GameManager
/// Clase controlador global del estado general del juego.
/// 
/// Funciones actuales:
/// - Evitar duplicados entre cargas de escenas.
/// - Mantiene persistencia gracias al DontDestroyOnLoad.
/// - Gestiona visibilidad y confinamiento del cursor.
/// 
/// Responsabilidades futuras previstas:
/// Coordinar los sistemas globales como el RoundSystem, AudioManager, etc. 
/// Gestionar el estado de Menu, Playing, Paused, etc.
/// Centralizar el control de los eventos globales.
/// 
/// Servirá como punto central de coordinacion futura.
/// No olvidar que debe asignarse a mi escena principal en un gameobject vacio y llamado gamemanager donde gestionaremos toda la coordinacion.
/// 
/// </summary>

public class GameManager : MonoBehaviour
{
    // Garantizamos que solo existira una instancia activa del GameManager.
    private void Awake()
    {
        // Busca todas las instancias existentes de GameManager
        GameManager[] managers = FindObjectsByType<GameManager>(FindObjectsSortMode.None);

        // Si hay más de una instancia, destruye la actual
        if (managers.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        // Mantiene este objeto entre cambios de escena
        DontDestroyOnLoad(gameObject);
    }

    // Configura el estado inicial del cursor. 
    private void Start()
    {
        // Aplica configuración de cursor para modo gameplay
        HideCursor();
    }

    // Oculta y confina el cursor dentro de la ventana del juego.
    private void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    // Se ejecuta cuando la aplicación gana o pierde foco.
    private void OnApplicationFocus(bool hasFocus)
    {
        // Reaplica la configuración del cursor si vuelve a tener foco.
        if (hasFocus)
        {
            HideCursor();
        }
    }

    // Se ejecuta al cerrar la aplicación.
    private void OnApplicationQuit()
    {
        // Restaura el estado normal del cursor.
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}