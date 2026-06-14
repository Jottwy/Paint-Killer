# PaintKiller

**Videojuego 2D multijugador local asimétrico**

Painter vs Runner. Uno dibuja y lanza herramientas. El otro solo sobrevive.

## Qué incluye

- **Movimiento del Runner** — Fluidez inmediata con aceleración, desaceleración, coyote time y jump buffer
- **Sistema asimétrico** — Painter controla con ratón (herramientas parametrizadas), Runner con teclado (solo movimiento)
- **Arquitectura modular** — Separación clara: Input → Logic → Data → Visual
- **Sistema de rondas** — 60 segundos por ronda, 3 vidas para el Runner
- **Herramientas parametrizadas** — Cada herramienta define forma, tamaño, duración, impacto, coste y cooldown
- **Física sólida** — Rigidbody2D con detección de suelo por BoxCast y gravedad dinámica
- **Persistencia** — Perfiles de herramientas en JSON

## Controles

| Rol | Input | Acción |
|---|---|---|
| **Runner** | WASD / Flechas | Movimiento lateral |
| **Runner** | Espacio | Salto |
| **Painter** | Ratón | Aiming |
| **Painter** | Click izquierdo | Usar herramienta |

## Versión de Unity

Unity 2022.3.62f3 (LTS) · Built-in Render Pipeline

## Estructura del código — Assets/Scripts/
Scripts/

├── Input/

│   └── LocalRunnerInput.cs      # Captura WASD/Espacio

├── Logic/

│   ├── RunnerMovementSystem2D.cs # Movimiento, salto, detección suelo

│   ├── DragSystem.cs            # Mecánica de arrastre del Painter

│   └── RoundSystem.cs           # Control de rondas y vidas

├── Data/

│   ├── AbilityProfile.cs        # Estructura serializable de herramientas

│   └── RoundConfig.cs           # Configuración de ronda

└── Core/

└── GameManager.cs           # Orquestador global

## Algunas decisiones

**Separación Input-Logic:** El input (LocalRunnerInput) solo captura. La lógica (RunnerMovementSystem2D) solo ejecuta. Esto permite reutilizar lógica con distintas fuentes de entrada.

**BoxCast para suelo:** Más fiable que raycasts puntuales. Detecta el suelo incluso si el Runner cae rápido.

**Herramientas parametrizadas:** No hardcodeo cada habilidad. Cada perfil JSON define un arma única. Si está desbalanceada, se ajustan números, no código.

**Bloqueo externo de movimiento:** El DragSystem puede bloquear el control del Runner (`CanMove = false`). Así la mecánica de arrastre no se mezcla con la lógica de movimiento.

**Gravedad dinámica:** La caída se siente mejor con un multiplicador en función de si está subiendo o cayendo.

## Posibles mejoras

- **UI visual** — Vidas, cooldowns y timer en pantalla (ahora es debug)
- **Más herramientas** — Sistema base preparado para añadir tipos de ataque sin tocar lógica
- **VFX y sonido** — Feedback visual/auditivo para impactos
- **Balance avanzado** — Ajuste fino de tiempos, cooldowns y fuerzas tras testing

## Notas

- Proyecto académico enfocado en **arquitectura** antes que cantidad de features
- Prototipo MVP: movimiento + mechanic asimétrica base + estructura escalable
- Documentación completa (GDD + README + comentarios en código)
