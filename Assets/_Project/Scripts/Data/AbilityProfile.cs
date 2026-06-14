/// <summary>
/// AbilityProfile
/// Contenedor de datos serializable
/// donde defino los parametros  principales de una habilidad.
/// 
/// Incluye:
/// - Daño
/// - Duración
/// - Cooldown
/// 
/// Diseñado para permitir persistencia en formato JSON
/// y facilitar la creación dinámica de habilidades.
/// 
/// </summary>

[System.Serializable]
public class AbilityProfile
{
    public float damage; // Daño
    public float duration; // Duracion 
    public float cooldown; // Cooldown
}