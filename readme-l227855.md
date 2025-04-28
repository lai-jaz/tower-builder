# Tower Builder

*Laiba Ijaz (22L-7855)*

## Overview
A 3D block stacking Android game where players have to tap to place a block on the tower, strategically keeping it balanced and thus preventing it from collapsing. Every block placed grants +10 points.

## Basic Details
### Blocks spawning:
The block to-be-dropped is spawned with 1. random size within an acceptable range of scale for x, y and z and 2. random materials from a given array of downloaded materials.

### Score calculation
Every block placed gives +10 points.

### Game Over condition
If the tower tilts past 30 degrees or if a block falls on the ground. Either of these conditions triggers "Game Over".

### Reset tower
A button is provided to allow the player to replay the game.

## Implementation Details
### Prefabs used
The only prefab used is Tower Block which is repeatedly spawned to be placed on the tower. 

### Scripts:
- **CheckCollapsed** : Checks if the tower collapsed based on the 2 mentioned conditions. If it does, it triggers the game over logic.

- **PlaceBlock** : Handles everything related to instantiating block, floating the block, dropping it, getting the input tot drop it. Also updates the camera to move upwards as the tower gets taller.

- **ReplayBtnClick** : Just contains the onClick methodology for the Replay button.

- **Score** : Used to track and update score.

- **TowerPhysics** : Contains logic for adding sway to the tower, collapsing the tower and resetting it. The game over logic is also specified in this script, which includes managing canvases.

### Canvases:
- **GamePlaying** : Contains current score and Replay button.

- **GameOver** : Contains the game over screen and shows the final score for 3 seconds.

### GitHub Repo:
https://github.com/lai-jaz/tower-builder

