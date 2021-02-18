# Amazing Maze

Help the fox cub find a way out of the maze! :fox_face:

# Goal
**Create a maze game:**
- [x] The game must have two modes: time game and search for hidden objects in the maze;
- [x] The game must contain a player, the player is controlled using the keyboard;
- [x] Use Cinemachine (implement two camera views of FPV and 2.5D);
- [x] Use Addressables for asynchronous loading and creating levels\items\player.

# Details
- The **main** branch contains a version of the game **without using Addressables**.
The branch using the **Addressables** - **feature/addressable**.
- The functionality for switching between camera views is located in the settings window (during the game).
- The game settings are placed in the Config file.

# Needs improvement
- At the moment, Addressables is only used for loading prefabs and scenes, but this technology can (and should) be used more widely.
