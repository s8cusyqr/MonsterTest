# MonsterTest - Codebase Index

## Project Overview
Unity 2020.3.11f1 project (C# 8.0) - A game where enemies run away from the player.

## Project Structure

### Assets/Scripts/
Contains all C# game scripts:

#### Game.cs
- **Purpose**: Main game controller
- **Key Features**:
  - Spawns 1000 enemies in a coroutine (0.1s delay between spawns)
  - Spawns player at position (2, 0, 0)
  - Manages menu canvas visibility
  - `BackClick()`: Returns to menu

#### Player.cs
- **Purpose**: Player movement controller
- **Key Features**:
  - WASD movement (W/S: up/down, A/D: left/right)
  - Movement speed: 0.001f
  - Screen boundary checking using Camera.main.WorldToScreenPoint
  - OnTriggerEnter2D: Stops enemies when player collides with them

#### Enemy.cs
- **Purpose**: Enemy AI behavior
- **Key Features**:
  - Finds player GameObject by tag "Player" in Start()
  - Runs away from player using Rigidbody2D velocity
  - Movement speed: 0.05f
  - `canRun` flag: Can be disabled when player touches enemy
  - Uses normalized direction vector for movement

#### Menu.cs
- **Purpose**: Main menu UI controller
- **Key Features**:
  - Button array management
  - Menu navigation (Exit, Settings, Play)
  - Camera background color changes to green on Play
  - Manages canvas visibility (menu, settings, game)

#### Settings.cs
- **Purpose**: Settings menu controller
- **Key Features**:
  - Back button to return to main menu
  - Button selection on start

### Assets/Prefabs/
- **Enemy.prefab**: Enemy prefab (requires Rigidbody2D, Collider2D, tag "Enemy")
- **Player.prefab**: Player prefab (requires Collider2D, tag "Player")

### Assets/Scenes/
- **Menu.unity**: Main menu scene

## Dependencies
- Unity 2D Animation (5.0.5)
- Unity 2D Pixel Perfect (4.0.1)
- Unity 2D Sprite (1.0.0)
- Unity 2D SpriteShape (5.1.2)
- Unity 2D Tilemap (1.0.0)
- Unity TextMeshPro (3.0.6)
- Unity Timeline (1.4.8)
- Unity UGUI (1.0.0)

## Key Game Mechanics
1. **Enemy Spawning**: 1000 enemies spawned at position (0, 0, 0) with 0.1s delay
2. **Player Movement**: WASD keys move player, constrained to screen boundaries
3. **Enemy AI**: Enemies run away from player using physics-based movement
4. **Collision**: When player touches enemy, enemy stops moving (canRun = false)

## Tags Required
- "Player" - Player GameObject
- "Enemy" - Enemy GameObjects

## Known Issues/Observations
1. Player movement speed (0.001f) is extremely slow
2. Enemy spawns all at same position (0, 0, 0) - may cause overlap
3. Enemy uses GameObject.FindWithTag() in Start() - could be optimized with reference
4. Player movement uses Camera.main.WorldToScreenPoint every frame - performance consideration
5. FixedUpdate used for movement, but Input.GetKey should be in Update()

## File Count Summary
- C# Scripts: 5
- Prefabs: 2
- Scenes: 1
- Total Source Files: 8



