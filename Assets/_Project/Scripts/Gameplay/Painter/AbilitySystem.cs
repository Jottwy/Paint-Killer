using UnityEngine;

/// <summary>
/// AbilitySystem
/// Clase encargada de gestionar todo lo relacionado con la ejecución
/// de habilidades dentro del juego.
/// 
/// Funciones actuales:
/// - (Aún no tiene lógica implementada).
/// 
/// Qué hará más adelante:
/// - Leer los valores definidos en AbilityProfile (daño, duración, cooldown).
/// - Ejecutar la habilidad cuando el jugador la active.
/// - Instanciar efectos o prefabs si la habilidad los necesita.
/// - Aplicar daño a otros objetos o jugadores.
/// - Controlar que no se pueda usar si está en cooldown.
/// 
/// La idea de separarlo en su propia clase es no mezclar esta lógica
/// dentro del GameManager u otros sistemas.
/// 
/// Así mantengo el código más ordenado, más fácil de entender
/// y más sencillo de ampliar en el futuro sin romper otras cosas.
/// 
/// Debe colocarse en la escena jugable en un GameObject vacío
/// llamado "AbilitySystem" para tener centralizado todo este comportamiento.
/// 
/// </summary>

public class AbilitySystem : MonoBehaviour
{
}