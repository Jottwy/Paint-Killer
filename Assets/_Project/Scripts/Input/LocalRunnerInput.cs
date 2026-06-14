using UnityEngine;

/// <summary>
/// LocalRunnerInput
/// Captura la entrada del jugador Runner (teclado).
///
/// Expone:
/// - MoveX: eje horizontal (-1, 0, 1).
/// - JumpPressed: salto pulsado este frame.
/// - JumpHeld: salto mantenido.
///
/// No mueve al personaje.
/// Solo entrega datos al sistema de movimiento (RunnerMovementSystem2D).
/// </summary>

public class LocalRunnerInput : MonoBehaviour
{
    public float MoveX { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool JumpHeld { get; private set; }

    private void Update()
    {
        MoveX = Input.GetAxisRaw("Horizontal"); 
        JumpPressed = Input.GetButtonDown("Jump"); // Space por defecto
        JumpHeld = Input.GetButton("Jump");
    }

    public void ConsumeJumpPressed() => JumpPressed = false;
}