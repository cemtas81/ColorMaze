# Color Maze

A 3D maze puzzle built in **Unity 6** for PC/Steam (unreleased). Slide through hand-designed mazes, paint every floor tile before the timer runs out, and deal with UFOs, warp holes and moving walls on the way.

https://github.com/cemtas81/ColorMaze/assets/79138234/6e058d97-b41d-4831-91a3-df5c27f032ae

▶️ [Trailer on YouTube](https://www.youtube.com/watch?v=NMakyOjBbIc)

## Features

- **Slide movement** — swipe or arrow keys; the player slides until it hits a wall (raycast-driven), with an optional wall-crush ability.
- **Tile painting** — every newly painted tile adds time, repainting a tile costs time; the level ends when every tile is painted.
- **Data-driven levels** — layouts are loaded from text files in `StreamingAssets`, so new levels need no code changes.
- **Obstacles & events** — random warp holes, a roaming UFO that can abduct the player, moving yo-yo walls.
- **Meta** — gem currency, character and skin shop with unlocks, persistent progress.
- **Online** — PlayFab device login and a global high-score leaderboard.

## Tech

Unity 6000.2 · C# · DOTween · Cinemachine · PlayFab SDK

## Project layout

| Path | Contents |
|---|---|
| `MyMaze/Assets/Scripts/`, `MyMaze/Assets/SkinManager.cs`, `Sellector.cs`, `GameOverManager.cs` | **My gameplay code** |
| Other folders under `MyMaze/Assets/` | Third-party libraries and art (PlayFab SDK, Leaderboard Creator, ParticleEffectForUGUI, Circular Scrolling List, Quick Outline, Cartoon FX Free, SimpleToon) |

## Running

Open `MyMaze/` with Unity 6000.2.6f2 or newer. Set your own PlayFab Title ID on the leaderboard component to test the online features.
