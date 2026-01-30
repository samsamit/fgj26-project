# Game Design Document

> 48-Hour Game Jam Edition - Keep it lean, keep it playable!

GAME JAM THEME: ### MASK ###

## 1. Game Identity

| Field         | Value                                                                     |
| ------------- | ------------------------------------------------------------------------- |
| **Title**     | Veiled                                                                    |
| **Tagline**   | Two players, two realities, one path forward                              |
| **Genre**     | Co-op Puzzle                                                              |
| **Theme**     | MASK                                                                      |
| **Core Hook** | Asymmetric co-op where communication and coordination reveal hidden paths |
| **Platform**  | Desktop (Windows/Linux/Mac via Godot)                                     |

---

## 2. Core Gameplay Loop

```
┌─────────────────────┐     ┌─────────────────────┐     ┌─────────────────────┐
│       ACTION        │ ──► │       RESULT        │ ──► │       REWARD        │
│  P1: Move/Interact  │     │  Hidden elements    │     │  Puzzle solved,     │
│  P2: Position mask  │     │  revealed/changed   │     │  new area unlocked  │
└─────────────────────┘     └─────────────────────┘     └─────────────────────┘
       ▲                                                         │
       └─────────────────────────────────────────────────────────┘
```

**Moment-to-moment gameplay:**

- Player 1 does: Move character with WASD, interact with puzzle elements
- Player 2 does: Position the mask overlay with mouse, activate mask abilities
- Game responds with: Alternate reality layer revealed within mask area, hidden paths/objects become visible or interactable
- Players progress by: Coordinating to solve puzzles - P2 reveals what P1 needs to see/reach

**Win condition:** Complete all puzzles on the map

**Lose condition:** Soft fail only - puzzles can be reset, no game over state. Players can retry indefinitely.

---

## 3. MVP Scope

### In Scope (Must Have)

These features are required for a playable submission:

- [ ] Character movement (WASD) - Player 1
- [ ] Mask control (mouse position) - Player 2
- [ ] Full-screen mask overlay with shaped center revealing alternate reality
- [ ] At least 3 different mask types with distinct visual effects
- [ ] One map with multiple interconnected puzzles
- [ ] Basic puzzle completion logic and state tracking
- [ ] Puzzle reset functionality (soft fail)
- [ ] Basic UI: start screen, puzzle completion feedback

### Out of Scope (If Time Permits)

Nice-to-haves, only after MVP is complete:

- [ ] Additional mask types beyond the initial 3
- [ ] More complex puzzle chains with dependencies
- [ ] Particle effects and visual polish
- [ ] Sound design and music
- [ ] Mask ability animations
- [ ] Tutorial/onboarding sequence

### Explicitly NOT Doing

To avoid scope creep, we are NOT building:

- Online multiplayer (local co-op only)
- Procedural generation
- Multiple maps/levels (single map with all puzzles)
- Save/load system
- Leaderboards or scoring

---

## 4. Art Direction

**Visual Style:** Pixel art with mysterious/atmospheric tone

**Color Palette:**
| Role | Color | Hex |
|------|-------|-----|
| Primary (Base Reality) | Dark slate blue | #2D3A4A |
| Secondary (Mask Reality) | Deep purple | #4A2D5C |
| Accent (Interactive) | Teal glow | #00D9C0 |
| Background | Near black | #0D1117 |
| Mask Overlay | Semi-transparent violet | #8B5CF6 |
| Mask Center Shape | Bright contrast | #F0ABFC |

**Art References:**

- Hyper Light Drifter (atmospheric pixel art, mysterious mood)
- Majora's Mask (mask theming, transformation concept)
- Celeste (clean pixel art with strong visual feedback)

**Key Assets Needed:**

- [ ] Player character sprite (idle, walk animations)
- [ ] Environment tileset (walls, floors, obstacles)
- [ ] Mask overlay shader/visual effect
- [ ] Mask center shape variants (circle, square, custom shapes per mask type)
- [ ] Puzzle element sprites (switches, doors, platforms)
- [ ] UI elements (start screen, puzzle complete indicator)
- [ ] Visual feedback for mask ability activation

---

## 5. Audio Direction

**Music Mood:** Ambient, mysterious, atmospheric - subtle tension that builds with puzzle complexity

**Sound Effects Needed:**

- [ ] Character footsteps (subtle, not distracting)
- [ ] Mask activation/deactivation sound (ethereal whoosh)
- [ ] Mask movement ambient hum
- [ ] Reality shift sound (when mask reveals alternate layer)
- [ ] Puzzle element interaction (switches, doors)
- [ ] Puzzle completion chime (satisfying, mysterious)
- [ ] Puzzle reset sound (soft, non-punishing)
- [ ] UI navigation clicks
- [ ] Ambient environmental sounds

**Audio Sources:** [Where will audio come from?]

- [ ] Team member creating original
- [ ] Free asset packs (freesound.org, itch.io asset packs)
- [ ] Generated/synthesized (BFXR for placeholder SFX)

---

## 6. Technical Architecture

**Engine:** Godot 4.x with C#

**Key Components/Systems:**
Following the project's composition-over-inheritance pattern:

| Component             | Responsibility                                           |
| --------------------- | -------------------------------------------------------- |
| MovementComponent     | Handles character WASD movement (Player 1)               |
| MaskController        | Mouse-controlled mask position and state (Player 2)      |
| MaskOverlay           | Visual shader for full-screen overlay with shaped center |
| MaskAbility           | Base class for mask-specific abilities                   |
| PuzzleComponent       | Individual puzzle logic and completion conditions        |
| PuzzleManager         | Tracks puzzle completion states across the map           |
| GameStateManager      | Menu, playing, puzzle reset, game complete states        |
| InteractableComponent | Base for puzzle elements that respond to player/mask     |

**Scene Structure:**

```
Main
├── GameManager (autoload)
├── UI
│   ├── MainMenu
│   ├── HUD (puzzle status, current mask indicator)
│   └── PuzzleComplete
├── MaskOverlay (CanvasLayer - renders above world)
│   └── MaskController
│       └── MaskShader
└── World
    ├── Character
    │   └── MovementComponent
    ├── Map
    │   ├── BaseLayer (always visible)
    │   └── MaskedLayer (visible only through mask)
    └── Puzzles
        ├── Puzzle1
        ├── Puzzle2
        └── ...
```

**Signals to Define:**

- `GameStarted`
- `GameCompleted`
- `MaskChanged(maskType: MaskType)`
- `MaskAbilityActivated(maskType: MaskType)`
- `PuzzleCompleted(puzzleId: string)`
- `PuzzleReset(puzzleId: string)`
- `AllPuzzlesCompleted`

---

## 7. Team & Task Breakdown

**Team (9 members):** Samu, Roni, Tomi, Lauri (Taffe), Emma, Pekka, Ville, Juuso, Juhani

### Sprint Schedule

#### Hours 0-8: Foundation

- [ ] **ALL**: Finalize GDD, agree on scope, assign roles
- [ ] **PROG Team**: Project setup, basic character movement (WASD)
- [ ] **PROG Team**: Basic mask controller (mouse position tracking)
- [ ] **ART Team**: Concept sketches, finalize color palette
- [ ] **ART Team**: Start character sprite design
- [ ] **AUDIO**: Source/create placeholder sounds

#### Hours 8-24: Core Loop

- [ ] **PROG**: Mask overlay shader implementation
- [ ] **PROG**: Alternate reality layer system (base layer + masked layer)
- [ ] **PROG**: Basic puzzle component and completion logic
- [ ] **PROG**: Game state management (menu, play, reset)
- [ ] **ART**: Character sprite with walk animation
- [ ] **ART**: Environment tileset (base and masked versions)
- [ ] **ART**: Mask overlay visual effects
- [ ] **AUDIO**: Ambient track draft, mask activation sounds

#### Hours 24-40: Content & Polish

- [ ] **PROG**: Implement 3+ mask types with different abilities
- [ ] **PROG**: Build out puzzles on the map
- [ ] **PROG**: Puzzle dependency/progression system
- [ ] **PROG**: Bug fixes from playtesting
- [ ] **ART**: Final sprites and animations
- [ ] **ART**: UI polish (start screen, HUD, puzzle indicators)
- [ ] **ART**: Visual feedback for interactions
- [ ] **AUDIO**: Final audio pass, puzzle completion sounds

#### Hours 40-48: Ship It!

- [ ] **ALL**: Feature freeze at hour 40!
- [ ] **ALL**: Playtesting and critical bug fixes only
- [ ] **PROG**: Build exports for target platforms
- [ ] **ALL**: Prepare submission (screenshots, description)
- [ ] **ALL**: Submit with buffer time!

---

## 8. Quick Reference

### Controls

**Player 1 (Character):**
| Input | Action |
| ------------- | ------------------------------- |
| W | Move up |
| A | Move left |
| S | Move down |
| D | Move right |
| E / Space | Interact with puzzle element |

**Player 2 (Mask):**
| Input | Action |
| ------------- | ------------------------------- |
| Mouse Move | Position mask overlay |
| Left Click | Activate mask ability |
| Scroll Wheel | Cycle through available masks |

**Shared:**
| Input | Action |
| ------------- | ------------------------------- |
| Esc | Pause/Menu |
| R | Reset current puzzle |

### Key Values to Tune

| Parameter               | Starting Value | Notes                              |
| ----------------------- | -------------- | ---------------------------------- |
| Character speed         | 200 px/s       | Should feel responsive but precise |
| Mask movement smoothing | 0.1            | Slight lag for intentional control |
| Mask center radius      | 150 px         | Visible area through mask          |
| Mask overlay opacity    | 0.7            | Balance between visible layers     |
| Puzzle reset delay      | 0.5 s          | Time before puzzle can be retried  |

---

## Notes & Ideas

_Quick capture space for ideas during development:_

**Open decisions to finalize:**

- Confirm game title: "Veiled" (or alternative?)
- Define specific abilities for the 3+ mask types (examples: reveal hidden platforms, phase through walls, see hidden paths, slow time in mask area)
- Determine if mask shape should vary per mask type or be configurable

**Mask type ideas:**

- Reveal Mask: Shows hidden platforms/paths in the masked area
- Phase Mask: Allows character to pass through certain walls
- Vision Mask: Reveals puzzle solutions or hidden switches
- Time Mask: Slows or stops moving obstacles within the area

**Puzzle ideas:**

- Hidden path navigation (P2 reveals, P1 walks)
- Timed switches that need mask to slow down
- Multi-step puzzles requiring mask swapping
- Coordination challenges where both players must act simultaneously

---

> Remember: A finished game is better than a perfect game. Ship it!
