# Interface Suggestions — The Black Ale Adventure

**Version:** 0.01a  
**Last Updated:** 2026-04-10  

## Recommended UI Layout (1280×720)

```
┌──────────────────────────────────────────────┬──────────────┐
│                                              │  CHARACTER   │
│                                              │  PANEL       │
│                                              │              │
│              GAME VIEWPORT                   │  [Portrait]  │
│              (Grid Area)                     │  Name        │
│              960 × 720                       │  Class       │
│                                              │              │
│    . . . . . . . . . . . . . . . .          │  STR: 10     │
│    . . . . . . . . . . . . . . . .          │  DEX: 16     │
│    . . . . . . . . . . . . . . . .          │  WIS: 8      │
│    . . . . . . . . . . . . . . . .          │  CHA: 12     │
│    . . . C . . ■ . . . . . . . . .          │              │
│    . . . . . . . . . ■ . . . . . .          │  ──────────  │
│    . . . . . . . . . . . . . . . .          │  INVENTORY   │
│    . . . . . . . . . . . . . . . .          │  [■] [■] [ ] │
│    . . . . . . . . . . . . . . . .          │  [ ] [ ] [ ] │
│    . . . . . . . . . . . . . . . .          │  [ ] [ ] [ ] │
│                                              │              │
└──────────────────────────────────────────────┴──────────────┘
```

## UI Components

### 1. Game Viewport (Left, 75% width)
- Black background with dot matrix grid overlay
- Items displayed as colored squares at grid positions
- Character displayed as magenta "C"
- This is where all gameplay interaction happens

### 2. Character Panel (Right, 25% width)
- **Portrait Area**: Placeholder colored block with initial (future: character art)
- **Name & Class**: "Grimjaw Lockfinger — Thief"
- **Stats Block**: RPG stats in classic format
- **Inventory Grid**: 3×3 grid of item slots (expandable later)
  - Each slot shows item color swatch and abbreviated name
  - Hover tooltip for full name

### 3. Context Menu (Floating Popup)
- Appears at cursor position on right-click
- Semi-transparent dark background with border
- Menu items:
  - **Inspect** — white text, active
  - **Take** — white text, active
  - **Use** — grey text, disabled (alpha)
  - **Combine** — grey text, disabled (alpha)
- Dismiss by clicking elsewhere or pressing Escape

### 4. Inspect Window (Modal Overlay)
- Centered on screen
- Dark panel with lighter border
- Item name as header
- Description text body
- Close button [X] top-right
- Click outside to dismiss

### 5. Title Screen
- Full black background
- "The Black Ale Adventure" in white, centered, stylized text
- Subtle animation (fade-in, slight glow — future)
- "Start Game" button below title
- Future: Continue, Options, Credits

## Color Palette (Alpha)

| Element | Color | Hex |
|---------|-------|-----|
| Background | Black | `#000000` |
| Grid Dots | Dark Grey | `#333333` |
| Character | Magenta | `#FF00FF` |
| Item Selected | Cyan | `#00FFFF` |
| Item Colors | Random (not cyan/magenta) | Various |
| UI Panel BG | Dark Navy | `#1A1A2E` |
| UI Text | White | `#FFFFFF` |
| Disabled Text | Grey | `#666666` |
| UI Border | Gold | `#C9A830` |

## SCUMM-Style Inspirations

The classic SCUMM interface had these elements we should evolve toward:

1. **Verb Bar** (future): A row of verbs at the bottom (Look, Pick Up, Use, Talk, Push, Pull)
2. **Inventory Strip** (future): Horizontal scrolling inventory at the bottom
3. **Scene Viewport**: The main view taking most of the screen
4. **Sentence Line** (future): "Use [item] with [item]" display

For the alpha, we simplify this to a right-click context menu, but the architecture should support evolving to a verb-based interface.

## Font Recommendations

- **Title**: A medieval/dwarven-style display font (future — use TextMeshPro default for alpha)
- **UI Text**: Clean sans-serif (Liberation Sans / built-in)
- **Item Descriptions**: Slightly stylized serif for flavor text (future)

## Responsive Design Notes

- Alpha is fixed 1280×720
- UI uses Unity Canvas Scaler (Scale With Screen Size, reference 1280×720)
- Future: Support 16:9 and 16:10 aspect ratios
- Character panel width should be percentage-based for future scaling
