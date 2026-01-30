# Game Design Document

> 48-Hour Game Jam Edition - Keep it lean, keep it playable!

---

## 1. Game Identity

| Field         | Value                                               |
| ------------- | --------------------------------------------------- |
| **Title**     | [Your Game Name]                                    |
| **Tagline**   | [One sentence that sells the game]                  |
| **Genre**     | [e.g., Puzzle, Platformer, Roguelike, etc.]         |
| **Theme**     | [Game jam theme if applicable]                      |
| **Core Hook** | [What makes this fun? Why would someone play this?] |
| **Platform**  | Desktop (Windows/Linux/Mac via Godot)               |

---

## 2. Core Gameplay Loop

```
┌─────────────┐     ┌─────────────┐     ┌─────────────┐
│   ACTION    │ ──► │   RESULT    │ ──► │   REWARD    │
│  [verb it]  │     │ [feedback]  │     │ [progress]  │
└─────────────┘     └─────────────┘     └─────────────┘
       ▲                                       │
       └───────────────────────────────────────┘
```

**Moment-to-moment gameplay:**

- Player does: [primary action verb - jump, shoot, solve, build, etc.]
- Game responds with: [immediate feedback]
- Player progresses by: [how do they advance?]

**Win condition:** [How does the player win/complete the game?]

**Lose condition:** [How does the player fail? Or is there no fail state?]

---

## 3. MVP Scope

### In Scope (Must Have)

These features are required for a playable submission:

- [ ] [Core mechanic #1]
- [ ] [Core mechanic #2]
- [ ] [Basic player controls]
- [ ] [Win/lose state]
- [ ] [Minimum 1 level/scenario]
- [ ] [Basic UI: start screen, game over]

### Out of Scope (If Time Permits)

Nice-to-haves, only after MVP is complete:

- [ ] [Polish feature #1]
- [ ] [Additional levels]
- [ ] [Extra mechanic]
- [ ] [Particle effects]
- [ ] [Leaderboards/saving]

### Explicitly NOT Doing

To avoid scope creep, we are NOT building:

- [Feature that sounds cool but is too ambitious]
- [Multiplayer/networking]
- [Procedural generation (unless core to concept)]

---

## 4. Art Direction

**Visual Style:** [e.g., Pixel art, Low-poly 3D, Hand-drawn, Minimalist]

**Color Palette:**
| Role | Color | Hex |
|------|-------|-----|
| Primary | [color] | #XXXXXX |
| Secondary | [color] | #XXXXXX |
| Accent | [color] | #XXXXXX |
| Background | [color] | #XXXXXX |

**Art References:**

- [Link or description of visual reference 1]
- [Link or description of visual reference 2]

**Key Assets Needed:**

- [ ] Player character
- [ ] Environment tiles/objects
- [ ] UI elements
- [ ] [Other critical assets]

---

## 5. Audio Direction

**Music Mood:** [e.g., Upbeat chiptune, Ambient tension, Jazzy, Silent]

**Sound Effects Needed:**

- [ ] Player action sound (jump/shoot/etc.)
- [ ] Success/reward sound
- [ ] Failure/damage sound
- [ ] UI click sounds
- [ ] [Other key sounds]

**Audio Sources:** [Where will audio come from?]

- [ ] Team member creating original
- [ ] Free asset packs (list sources)
- [ ] Generated/synthesized

---

## 6. Technical Architecture

**Engine:** Godot 4.x with C#

**Key Components/Systems:**
Following the project's composition-over-inheritance pattern:

| Component                 | Responsibility                   |
| ------------------------- | -------------------------------- |
| [e.g., MovementComponent] | [Handles player/entity movement] |
| [e.g., HealthComponent]   | [Manages health and damage]      |
| [e.g., ScoreManager]      | [Tracks and displays score]      |

**Scene Structure:**

```
Main
├── GameManager (autoload)
├── UI
│   ├── MainMenu
│   ├── HUD
│   └── GameOver
└── World
    ├── Player
    └── Level
```

**Signals to Define:**

- `GameStarted`
- `GameEnded(win: bool)`
- `ScoreChanged(newScore: int)`
- [Other key signals]

---

## 7. Team & Task Breakdown

### Sprint Schedule

#### Hours 0-8: Foundation

- [ ] **ALL**: Finalize GDD, agree on scope
- [ ] **PROG**: Project setup, basic player movement
- [ ] **ART**: Concept sketches, color palette
- [ ] **AUDIO**: Source/create placeholder sounds

#### Hours 8-24: Core Loop

- [ ] **PROG**: Core mechanic implementation
- [ ] **PROG**: Basic game states (menu, play, game over)
- [ ] **ART**: Player sprite/model, basic environment
- [ ] **AUDIO**: Main theme draft, key sound effects

#### Hours 24-40: Content & Polish

- [ ] **PROG**: Level/content integration
- [ ] **PROG**: Bug fixes from playtesting
- [ ] **ART**: Final assets, animations
- [ ] **ART**: UI polish
- [ ] **AUDIO**: Final audio pass

#### Hours 40-48: Ship It!

- [ ] **ALL**: Feature freeze at hour 40!
- [ ] **ALL**: Playtesting and critical bug fixes only
- [ ] **PROG**: Build exports for target platforms
- [ ] **ALL**: Prepare submission (screenshots, description)
- [ ] **ALL**: Submit with buffer time!

---

## 8. Quick Reference

### Controls

| Input         | Action         |
| ------------- | -------------- |
| WASD / Arrows | [Movement]     |
| Space         | [Jump/Action]  |
| Mouse         | [Aim/Look]     |
| Click         | [Shoot/Select] |
| Esc           | [Pause/Menu]   |

### Key Values to Tune

| Parameter        | Starting Value | Notes                  |
| ---------------- | -------------- | ---------------------- |
| Player speed     | [X]            | [Feel good?]           |
| Jump height      | [X]            | [Reachable platforms?] |
| [Mechanic value] | [X]            | [Notes]                |

---

## Notes & Ideas

_Quick capture space for ideas during development:_

-
-
- ***

  > Remember: A finished game is better than a perfect game. Ship it!
