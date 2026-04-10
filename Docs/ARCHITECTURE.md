# Architecture Document — The Black Ale Adventure

**Version:** 0.01a  
**Last Updated:** 2026-04-10  

## High-Level Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                        Unity Engine                              │
│  ┌───────────────────────────────────────────────────────────┐  │
│  │                     Scene Layer                            │  │
│  │  TitleScene ──────────────> GameScene                     │  │
│  └───────────────────────────────────────────────────────────┘  │
│  ┌───────────────────────────────────────────────────────────┐  │
│  │                    Game Manager (Singleton)                │  │
│  │  - Game State (Title, Playing, Paused, Inspect)           │  │
│  │  - Current Scene Config                                    │  │
│  │  - Active Character                                        │  │
│  │  - Party Data                                              │  │
│  └─────────┬────────────┬──────────────┬─────────────────────┘  │
│            │            │              │                         │
│  ┌─────────▼──┐  ┌─────▼──────┐  ┌───▼────────────────────┐   │
│  │ Grid System│  │Item Manager│  │  UI System              │   │
│  │            │  │            │  │  - Character Panel       │   │
│  │ - Coord    │  │ - Spawn    │  │  - Context Menu          │   │
│  │   Matrix   │  │ - Interact │  │  - Inspect Window        │   │
│  │ - Pathfind │  │ - Pickup   │  │  - Inventory Display     │   │
│  │ - Movement │  │            │  │                          │   │
│  └────────────┘  └────────────┘  └──────────────────────────┘   │
│  ┌───────────────────────────────────────────────────────────┐  │
│  │                  Configuration Layer                       │  │
│  │  JSON Scene Configs ──> SceneLoader ──> Runtime Objects    │  │
│  └───────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────┘
```

## Core Systems

### 1. Game Manager (`GameManager.cs`)
- **Singleton** pattern — persists across scenes
- Manages game state enum: `Title`, `Playing`, `Inspect`, `Paused`
- Holds references to active character, party, and current scene config
- Entry point for scene transitions

### 2. Grid System (`GridSystem.cs`)
- Configurable grid dimensions (default: 16 columns × 10 rows)
- Renders dot matrix overlay on screen using Unity UI
- Converts between grid coordinates and screen/world positions
- **A* pathfinding** for click-to-move (avoids item-occupied cells)
- Tracks which cells are occupied by items or impassable

### 3. Character System
- **CharacterData.cs** — Serializable data model (name, class, stats, inventory)
- **PlayerController.cs** — Handles arrow-key and click-to-move input
- **CharacterAbility.cs** — Base class for per-class abilities (Thief: pick lock, etc.)
- Characters occupy exactly one grid cell
- Movement is discrete: one cell per arrow key press, animated lerp

### 4. Item System
- **ItemData.cs** — Serializable item definition (name, description, color, grid position)
- **ItemManager.cs** — Spawns items from scene config, manages item lifecycle
- **ItemInteraction.cs** — Right-click context menu, inspect, take, use, combine
- Items block movement (character cannot walk onto an item cell)
- Items highlight cyan when selected

### 5. UI System
- **TitleScreenUI.cs** — Logo display, Start Game button
- **CharacterPanelUI.cs** — Side panel: name, stats (STR, DEX, WIS, CHA), inventory slots
- **ContextMenuUI.cs** — Popup menu: Inspect, Take, Use (greyed), Combine (greyed)
- **InspectWindowUI.cs** — Modal popup showing item name and description

### 6. Scene Configuration (`SceneConfig.cs`)
- JSON files define scenes: background, grid size, items with positions
- `SceneLoader.cs` parses JSON at runtime using Unity's JsonUtility (+ Newtonsoft fallback)
- Easily extensible for future scenes without code changes
- See `SCENE_CONFIG_GUIDE.md` for format details

## Data Flow

```
tavern.json ──parse──> SceneConfig ──spawn──> Grid + Items + Character
                                         │
User Input ──> PlayerController ──> Grid Movement
                                         │
Right-Click ──> ItemInteraction ──> ContextMenuUI
                                         │
                              ┌──────────┼──────────┐
                              │          │          │
                         Inspect      Take     (Use/Combine)
                              │          │      [disabled]
                     InspectWindowUI  Inventory
```

## Design Patterns

| Pattern | Usage |
|---------|-------|
| Singleton | GameManager — single instance across scenes |
| Observer | Events for item pickup, state changes |
| Strategy | Character abilities (each class has different ability) |
| Data-Driven | JSON scene configs for items, locations, descriptions |
| MVC-ish | Data models separate from MonoBehaviour controllers |

## Future Architecture Considerations

- **Sprite Renderer** swap: Grid dots and colored squares will be replaced by sprite-based rendering
- **Scene Graph**: Background images per scene, layered rendering
- **Dialogue System**: Will need a dialogue tree (consider Ink or Yarn Spinner integration)
- **Save/Load**: JSON serialization of game state
- **Audio**: Event-driven audio manager
- **Scripting**: Lua or MoonSharp for scene scripting (see `SCRIPTING_SUGGESTIONS.md`)

## Technical Constraints (Alpha)

- Resolution: 1280×720 fixed
- Grid: 16×10 (configurable per scene via JSON)
- No sprite assets — all visuals are colored UI elements
- Single scene at a time (no scene streaming)
- No save/load system yet
- No audio yet
