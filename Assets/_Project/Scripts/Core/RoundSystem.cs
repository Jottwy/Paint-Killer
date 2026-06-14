using UnityEngine;

/// <summary>
/// RoundSystem
/// Clase gestiora DE la lógica de rondas del juego.
/// 
/// Funciones actuales:
/// - (Aun no implemntado nada).
/// 
/// Responsabilidades futuras previstas:
/// - Gestionar el temporizador de las rondas de la partida.
/// - Comprobar que condiciones de victoria y derrota hay preparadas.
/// - Reiniciar la escena al perder o ganar y/o iniciar una nueva ronda.
/// - Comunicar los resultados al GameManager.
/// 
/// Servirá como sistema independiente para evitar concentrar
/// demasiada lógica dentro del GameManager.
/// 
/// Organizamos y separamos logicas para reducir el maximo el desorden y mantener un buen control del codigo 
/// escalable y bien estructurado para evitar malas practicas.
/// 
/// Debe asignarse en la escena jugable dentro de un GameObject vacío
/// llamado "RoundSystem", desde donde se controlará la partida. (Podria añadir un gameobject padre llamado managers y asignar todos los managers)
/// 
/// </summary>
public class RoundSystem : MonoBehaviour
{
}