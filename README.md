# The Black Ale Adventure

**Version:** 0.01a | **Engine:** Unity 2022.3 LTS | **Genre:** 2D Point & Click Adventure

A low-fantasy adventure game where four dwarven heroes quest to retrieve the legendary Black Ale — a mythical brew lost to dwarvenkind.

Inspired by LucasArts classics: Sam & Max, Day of the Tentacle, Monkey Island, Full Throttle.

## Quick Start

1. Install **Unity Hub** and **Unity 2022.3.20f1** (or any 2022.3.x)
2. Open Unity Hub → **Add** → Browse to this project folder
3. Wait for Unity to import (first time takes a few minutes)
4. Go to menu: **Black Ale → Setup Scenes** (this creates the scenes properly with all components wired up)
5. Open **File → Build Settings** and verify TitleScene and GameScene are listed
6. Open `Assets/Scenes/TitleScene.unity`
7. Press **Play** ▶

## Alpha Controls

| Input | Action |
|-------|--------|
| Arrow Keys / WASD | Move character one grid cell |
| Left Click (on grid) | Character pathfinds to clicked cell |
| Right Click (on adjacent item) | Opens context menu |
| Escape | Close menus/windows |

## Project Documentation

- [Docs/BRIEF.md](Docs/BRIEF.md) — Game design brief and feature list
- [Docs/ARCHITECTURE.md](Docs/ARCHITECTURE.md) — Technical architecture
- [Docs/INTERFACE_SUGGESTIONS.md](Docs/INTERFACE_SUGGESTIONS.md) — UI/UX recommendations
- [Docs/SCRIPTING_SUGGESTIONS.md](Docs/SCRIPTING_SUGGESTIONS.md) — Scripting language analysis (Lua recommendation)
- [Docs/SCENE_CONFIG_GUIDE.md](Docs/SCENE_CONFIG_GUIDE.md) — How to create scene JSON configs

## Scene Configuration

Scenes are defined in JSON files at `Assets/Config/Scenes/`. See the [Scene Config Guide](Docs/SCENE_CONFIG_GUIDE.md) for the full schema.

Example: `Assets/Config/Scenes/tavern.json`

## License

All rights reserved. © 2026
