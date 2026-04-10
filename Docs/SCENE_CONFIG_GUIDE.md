# Scene Configuration Guide — The Black Ale Adventure

**Version:** 0.01a  
**Last Updated:** 2026-04-10  

## Overview

Scenes in The Black Ale Adventure are defined using JSON configuration files. This data-driven approach allows designers to create and modify scenes without touching C# code.

Scene config files are located in: `Assets/Config/Scenes/`

## JSON Schema

### Scene File Structure

```json
{
  "sceneId": "string — unique scene identifier",
  "sceneName": "string — display name",
  "description": "string — scene flavor text shown on entry",
  "background": "string — path to background image (future, null for alpha)",
  "music": "string — path to music track (future, null for alpha)",
  "gridWidth": 16,
  "gridHeight": 10,
  "playerStart": { "x": 2, "y": 5 },
  "items": [
    {
      "itemId": "string — unique item identifier",
      "name": "string — display name",
      "description": "string — inspection text",
      "position": { "x": 4, "y": 5 },
      "color": { "r": 0.9, "g": 0.8, "b": 0.3 },
      "takeable": true,
      "usable": false,
      "combinable": false,
      "sprite": "string — path to sprite (future, null for alpha)",
      "requiredAbility": "string — ability needed (null if none)",
      "onInspect": "string — custom Lua script (future)",
      "onTake": "string — custom Lua script (future)",
      "onUse": "string — custom Lua script (future)"
    }
  ],
  "exits": [
    {
      "position": { "x": 15, "y": 5 },
      "targetScene": "string — sceneId of destination",
      "targetPosition": { "x": 0, "y": 5 },
      "description": "string — exit description"
    }
  ],
  "script": "string — path to Lua scene script (future)"
}
```

## Example: Tavern Scene

See `Assets/Config/Scenes/tavern.json` for the full example.

### Quick Reference

```json
{
  "sceneId": "tavern",
  "sceneName": "The Rusty Tankard",
  "description": "A dimly lit tavern. The air is thick with smoke and the smell of spilled ale.",
  "background": null,
  "music": null,
  "gridWidth": 16,
  "gridHeight": 10,
  "playerStart": { "x": 2, "y": 5 },
  "items": [
    {
      "itemId": "pale_ale",
      "name": "Pale Ale",
      "description": "A pale blonde ale. It's flat and lukewarm.",
      "position": { "x": 6, "y": 4 },
      "color": { "r": 0.9, "g": 0.8, "b": 0.3 },
      "takeable": true,
      "usable": false,
      "combinable": false,
      "sprite": null,
      "requiredAbility": null
    }
  ]
}
```

## Field Reference

### Scene Fields

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `sceneId` | string | Yes | Unique identifier (e.g., "tavern", "cave_entrance") |
| `sceneName` | string | Yes | Display name shown to player |
| `description` | string | Yes | Text shown when entering the scene |
| `background` | string | No | Path to background image (null = black) |
| `music` | string | No | Path to music file (null = silence) |
| `gridWidth` | int | Yes | Number of columns in the grid (default: 16) |
| `gridHeight` | int | Yes | Number of rows in the grid (default: 10) |
| `playerStart` | object | Yes | Starting grid position `{x, y}` |
| `items` | array | Yes | Array of item objects |
| `exits` | array | No | Scene transition points |
| `script` | string | No | Path to Lua scene script (future) |

### Item Fields

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `itemId` | string | Yes | Unique identifier (e.g., "pale_ale") |
| `name` | string | Yes | Display name |
| `description` | string | Yes | Text shown on Inspect |
| `position` | object | Yes | Grid position `{x, y}` (0-indexed) |
| `color` | object | Yes | RGB color `{r, g, b}` (0.0–1.0). **Avoid cyan and magenta** |
| `takeable` | bool | Yes | Can the player pick this up? |
| `usable` | bool | Yes | Can the player use this? (false in alpha) |
| `combinable` | bool | Yes | Can this be combined? (false in alpha) |
| `sprite` | string | No | Path to sprite image (null = colored square) |
| `requiredAbility` | string | No | Ability needed to interact (e.g., "pick_lock") |
| `onInspect` | string | No | Lua snippet for custom inspect behavior (future) |
| `onTake` | string | No | Lua snippet for custom take behavior (future) |
| `onUse` | string | No | Lua snippet for custom use behavior (future) |

### Exit Fields

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `position` | object | Yes | Grid position of the exit `{x, y}` |
| `targetScene` | string | Yes | Scene ID to transition to |
| `targetPosition` | object | Yes | Where player appears in target scene |
| `description` | string | No | Description of the exit |

## Color Guidelines

Items should use distinctive, non-cyan, non-magenta colors:

| Color | RGB | Good For |
|-------|-----|----------|
| Golden Yellow | `(0.9, 0.8, 0.3)` | Ale, coins, keys |
| Brown | `(0.6, 0.3, 0.1)` | Wood, food, leather |
| Dark Red | `(0.7, 0.1, 0.1)` | Potions, blood, gems |
| Forest Green | `(0.1, 0.6, 0.2)` | Plants, herbs |
| Silver Grey | `(0.7, 0.7, 0.8)` | Metal, tools, weapons |
| Deep Blue | `(0.1, 0.2, 0.7)` | Magic, water |
| Orange | `(0.9, 0.5, 0.1)` | Fire, food, torches |
| White | `(0.95, 0.95, 0.95)` | Paper, bone, cloth |

**Reserved colors:**
- **Magenta `(1.0, 0.0, 1.0)`** — Player character
- **Cyan `(0.0, 1.0, 1.0)`** — Selected/highlighted items

## Grid Coordinate System

```
     0   1   2   3   4   5   6   7   8   9  10  11  12  13  14  15
  0  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .
  1  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .
  2  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .
  3  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .
  4  .   .   .   .   .   .  [beer].   .   .   .   .   .   .   .   .
  5  .   .  [C]  .   .   .   .   .   .  [stew] .   .   .   .   .  [→]
  6  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .
  7  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .
  8  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .
  9  .   .   .   .   .   .   .   .   .   .   .   .   .   .   .   .
```

- Origin (0,0) is **top-left**
- X increases rightward (columns)
- Y increases downward (rows)
- Grid is 0-indexed

## Tips for Scene Designers

1. **Keep items spread out** — Don't cluster items; players need to walk next to them
2. **Leave pathways** — Ensure the player can always reach all items and exits
3. **Test adjacency** — Items can only be interacted with from adjacent cells (8 directions)
4. **Use descriptive IDs** — `locked_chest` is better than `item_03`
5. **Write flavorful descriptions** — This is an adventure game; personality matters!
6. **Consider abilities** — Place some items that require specific class abilities
7. **Plan exits** — Make sure exits connect logically between scenes

## Adding a New Scene

1. Create a new JSON file in `Assets/Config/Scenes/` (e.g., `cave_entrance.json`)
2. Follow the schema above
3. Add exits in connecting scenes that point to your new scene
4. Test by modifying `GameManager.cs` to load your scene (future: scene transition system)

## Validation

The `SceneLoader.cs` script performs basic validation on load:
- Checks required fields are present
- Validates grid positions are within bounds
- Warns about overlapping item positions
- Warns about items using reserved colors
