# Scripting Language Suggestions — The Black Ale Adventure

**Version:** 0.01a  
**Last Updated:** 2026-04-10  

## Recommendation: **Lua via MoonSharp**

### Why Lua?

For a SCUMM-inspired adventure game, the ideal scripting language needs to be:
- Easy for designers/writers to learn (non-programmers)
- Lightweight and embeddable in Unity/C#
- Good for defining game logic, dialogue, and puzzles
- Battle-tested in game development

**Lua is the clear winner** for this use case.

### Top Candidates Compared

| Feature | Lua (MoonSharp) | Ink | Yarn Spinner | Python (IronPython) | Custom DSL |
|---------|:---:|:---:|:---:|:---:|:---:|
| Learning curve | Easy | Very Easy | Very Easy | Easy | Medium |
| Unity integration | Excellent | Excellent | Excellent | Poor | Custom |
| General game scripting | ★★★★★ | ★★☆☆☆ | ★★☆☆☆ | ★★★★☆ | ★★★☆☆ |
| Dialogue/narrative | ★★★★☆ | ★★★★★ | ★★★★★ | ★★★☆☆ | ★★★★☆ |
| Puzzle logic | ★★★★★ | ★★☆☆☆ | ★★☆☆☆ | ★★★★★ | ★★★☆☆ |
| Performance | Fast | Fast | Fast | Slow | Varies |
| Community/ecosystem | Huge | Medium | Medium | Huge | None |
| Sandboxing | Good | N/A | N/A | Poor | Custom |
| Mature in games | Very | Inkle games | Unity games | Rarely | Varies |

### Recommended Architecture: Lua + Ink Hybrid

**Primary scripting: Lua (via MoonSharp)**
- Scene logic, puzzle scripts, item interactions
- Character ability effects
- Conditional triggers and game state manipulation
- Custom scene behaviors

**Dialogue system: Ink (via inkle's Unity plugin)**
- NPC dialogue trees
- Branching narrative
- Story state tracking

### MoonSharp (Lua for Unity)

[MoonSharp](https://www.moonsharp.org/) is a Lua interpreter written entirely in C#. It's:
- **Pure C#** — no native dependencies, works on all Unity platforms
- **Lua 5.2 compatible** — standard Lua syntax
- **Easy Unity integration** — call C# from Lua and vice versa
- **NuGet package**: `MoonSharp`
- **Unity Asset Store**: Free

#### Example: Scene Script in Lua

```lua
-- tavern_logic.lua
-- Scene script for the Tavern

function on_scene_enter()
    show_message("You enter the Rusty Tankard tavern. The smell of stale ale fills the air.")
    if not game.flags["tavern_visited"] then
        game.flags["tavern_visited"] = true
        show_message("The barkeep eyes you suspiciously.")
    end
end

function on_item_inspect(item_id)
    if item_id == "pale_ale" then
        show_message("A decent brew, but nothing compared to the legendary Black Ale.")
    elseif item_id == "stew" then
        show_message("The stew bubbles ominously. You're not entirely sure what meat that is.")
    end
end

function on_item_use(item_id, target_id)
    if item_id == "lockpick" and target_id == "locked_chest" then
        if party.active.class == "Thief" then
            show_message("Grimjaw makes quick work of the lock. Click!")
            set_item_state("locked_chest", "open")
            spawn_item("old_map", 8, 5)
        else
            show_message("You fumble with the lockpick. This requires a thief's touch.")
        end
    end
end

function on_ability_use(ability, target_id)
    if ability == "pick_lock" and target_id == "locked_chest" then
        return on_item_use("lockpick", target_id)
    end
end
```

#### Example: Item Definition in Lua (Alternative to JSON)

```lua
-- items/tavern_items.lua

items = {
    pale_ale = {
        name = "Pale Ale",
        description = "A pale blonde ale. It's flat and lukewarm, but it's better than water.",
        color = {0.9, 0.8, 0.3},  -- golden yellow
        takeable = true,
        position = {4, 5},
    },
    stew = {
        name = "Dog Meat Stew",
        description = "A steaming plate of stew. The chunks of meat look... questionable.",
        color = {0.6, 0.3, 0.1},  -- brown
        takeable = true,
        position = {4, 6},
    },
    locked_chest = {
        name = "Locked Chest",
        description = "A sturdy wooden chest with an iron padlock.",
        color = {0.5, 0.3, 0.1},  -- dark brown
        takeable = false,
        position = {10, 3},
        requires_ability = "pick_lock",
    },
}
```

### Implementation Phases

1. **Alpha (current)**: JSON-only scene configuration (no scripting yet)
2. **Beta v0.1**: Integrate MoonSharp, Lua scripts for item interactions
3. **Beta v0.2**: Add Ink for dialogue system
4. **Beta v0.3**: Full Lua scene scripting (triggers, conditionals, events)

### Why Not the Others?

- **Ink**: Excellent for branching narrative but too limited for general game logic. Use it alongside Lua for dialogue only.
- **Yarn Spinner**: Similar to Ink, narrative-focused. Less flexible for puzzle/item scripting.
- **Python (IronPython)**: Heavy runtime, poor Unity support, security concerns with sandboxing.
- **Custom DSL**: High development cost, no community, bugs to iron out. Only justified for very specific needs.
- **JavaScript (Jint)**: Decent option but Lua has stronger game industry presence and lighter footprint.
- **Wren**: Interesting but small community, less documentation.

### Integration Points with C#

```
C# (Unity) ←──────────→ MoonSharp (Lua)
    │                         │
    ├─ Register C# functions  │
    │  as Lua globals         │
    │  (show_message,         │
    │   spawn_item, etc.)     │
    │                         │
    ├─ Call Lua functions      │
    │  from C# (on_item_use,  │
    │   on_scene_enter, etc.) │
    │                         │
    └─ Share game state via   │
       Lua tables             │
```

### Security Note

MoonSharp provides good sandboxing — you can restrict which C# APIs are exposed to Lua scripts. This is important if you ever allow modding or community content.
