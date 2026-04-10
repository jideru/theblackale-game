# The Black Ale Adventure

**Version:** 0.01a (Alpha)  
**Genre:** 2D Point & Click Adventure  
**Engine:** Unity 2022.3 LTS  

## Overview

The Black Ale Adventure is a low-fantasy point-and-click adventure game inspired by LucasArts classics (Sam & Max Hit the Road, Day of the Tentacle, Monkey Island, Full Throttle) combined with RPG character abilities. 

A group of four dwarven adventurers embark on a quest to retrieve the legendary Black Ale — a mythical brew lost to dwarvenkind for generations.

## Story

Long ago, the master brewers of the Ironforge Deep crafted the Black Ale — a brew so perfect it was said to grant wisdom, courage, and an exceptionally warm belly. When the great cavern collapse sealed the Brewmaster's Vault, the recipe and the last remaining barrel were lost. Now, four unlikely dwarven heroes must journey across the land, solving puzzles, gathering clues, and dealing with all manner of obstacles to reclaim this legendary drink.

## Characters

| Name | Class | Ability | Description |
|------|-------|---------|-------------|
| Grimjaw Lockfinger | Thief | Pick Locks | A nimble-fingered rogue with a penchant for other people's property |
| Brokk Ironhand | Warrior | Force/Strength | A stout fighter who solves most problems with brute force |
| Mossvein Oakroot | Druid | Nature Magic | A mystical dwarf attuned to the old magic of stone and root |
| Brother Aldar | Priest | Miracles | A devout cleric whose small miracles have big consequences |

## Alpha (0.01a) Features

- [x] Title screen with game logo and Start Game button
- [x] Grid-based coordinate system with visible dot matrix
- [x] Thief character (magenta "C") on screen
- [x] Arrow key movement (one coordinate per press)
- [x] Click-to-move with pathfinding (shortest route, avoids items)
- [x] Right-click context menu on items (when adjacent)
- [x] Inspect Item — shows description in popup
- [x] Take Item — moves to inventory
- [x] Use Item — greyed out (future)
- [x] Combine Item — greyed out (future)
- [x] Character panel with name, stats, and inventory display
- [x] Items rendered as colored squares (non-cyan, non-magenta)
- [x] Selected items highlight in cyan
- [x] JSON-based scene configuration

## How to Open

1. Install Unity Hub and Unity 2022.3.20f1 (or compatible 2022.3.x)
2. Open Unity Hub → Add Project → Browse to this folder
3. Unity will import and set up the project
4. Open `Assets/Scenes/TitleScene.unity` and press Play

## Controls

- **Arrow Keys**: Move character one grid position
- **Left Click on Grid**: Character pathfinds to that position
- **Right Click on Item** (when adjacent): Opens context menu
- **Left Click**: General UI interaction

## Project Structure

```
Assets/
├── Scenes/
│   ├── TitleScene.unity        # Title/menu screen
│   └── GameScene.unity         # Main game scene
├── Scripts/
│   ├── Core/
│   │   ├── GameManager.cs      # Main game state management
│   │   ├── SceneLoader.cs      # Scene configuration loader
│   │   └── GridSystem.cs       # Coordinate grid system
│   ├── Character/
│   │   ├── CharacterData.cs    # Character data model
│   │   ├── CharacterController.cs # Movement & input
│   │   └── CharacterAbility.cs # Ability system base
│   ├── Items/
│   │   ├── ItemData.cs         # Item data model
│   │   ├── ItemManager.cs      # Item spawning & management
│   │   └── ItemInteraction.cs  # Right-click menu & actions
│   ├── UI/
│   │   ├── TitleScreenUI.cs    # Title screen controller
│   │   ├── CharacterPanelUI.cs # Character stats & inventory
│   │   ├── ContextMenuUI.cs    # Right-click context menu
│   │   └── InspectWindowUI.cs  # Item inspection popup
│   └── Config/
│       └── SceneConfig.cs      # JSON deserialization models
├── Config/
│   └── Scenes/
│       └── tavern.json         # Example scene configuration
├── Resources/
│   └── (runtime-loaded assets)
└── Prefabs/
    └── (future: reusable prefabs)
Docs/
├── BRIEF.md                    # This file
├── ARCHITECTURE.md             # Technical architecture
├── INTERFACE_SUGGESTIONS.md    # UI/UX recommendations
├── SCRIPTING_SUGGESTIONS.md    # Scripting language analysis
└── SCENE_CONFIG_GUIDE.md       # How to configure scenes
```

## License

All rights reserved. The Black Ale Adventure © 2026
