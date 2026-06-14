using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// CooldownSystem
/// Sistema encargado de gestionar los tiempos de espera (cooldowns)
/// de distintas acciones del juego usando claves tipo string.
/// 
/// Funciones actuales:
/// - Iniciar un cooldown indicando una clave y una duración.
/// - Consultar si una acción concreta sigue en cooldown.
/// 
/// Funcionamiento internamente:
/// - Guardar en un Dictionary una clave (string) asociada
///   al momento exacto en el que termina el tiempo (cooldown).
/// - Usar Time.time para hacer comparar el tiempo actual
///   con el tiempo almacenado.
/// 
/// De esta forma no necesito crear variables separadas
/// para cada habilidad, sino que puedo reutilizar este sistema
/// para cualquier acción simplemente usando una clave distinta como en practicas anteriores.
/// 
/// Diseñado para ser reutilizable y ampliable en el futuro.
/// Puede utilizarse junto con AbilitySystem u otros sistemas.
/// 
/// Reduzco redundancias y factorizamos el codigo con buenas practicas
/// 
/// </summary>

public class CooldownSystem : MonoBehaviour
{
    // Diccionario donde guardo cada cooldown activo.
    // La clave identifica la acción y el float indica el momento exacto en el que el cooldown termina.
    private Dictionary<string, float> cooldowns = new Dictionary<string, float>();

    // Inicia un cooldown para una acción concreta.
    public void StartCooldown(string key, float duration)
    {
        // Guarda el tiempo actual + la duración.
        cooldowns[key] = Time.time + duration;
    }

    // Devuelve true si la acción sigue en cooldown.
    public bool IsOnCooldown(string key)
    {
        // Si no existe la clave, no hay cooldown activo.
        if (!cooldowns.ContainsKey(key)) return false;

        // Compruba si el tiempo actual es menor que el tiempo de finalizacion guardado
        return Time.time < cooldowns[key];
    }
}