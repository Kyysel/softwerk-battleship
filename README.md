# battleship_softwerk
## How to use
**/!\ Current build crashes after the first phase of the game due to an unexpected error. The program runs fine in the unity editor.**
- Windows build only
- Unzip the archive an the Release folder.
- Execute the file "Battleship.exe"
</br>

- The only way to run the program correctly currently is to use the Unity Game Engine editor in version 2022.3 (LTS)
- Open the project with Unity selecting the "Battleship" folder at the root of the repository.
- Click on play button at the top of the screen.

## Game functionnalities
The game is a Battleship, 2 human players play on the same screen, alternating at each turn. One player plays as the Rebellion, the other as the Empire (Star Wars fan one day, Star Wars fan always).</br>
In the first phase, players have to place their ship on their grid. They can click on the ship they want to place, and move their cursor over the tile they want to place it at. They can rotate the ship with the right click of the mouse. The ships cannot be placed outside of the grid, nor on top of each other.
</br>When a player has played, a timer of 2 seconds is started and then the other player can press on the ENTER key of the keyboard to reveal his grid.
</br>Once the ships are placed, the players can click on the grid on the right to attack at the selected tile. If a ship is hit, it turns red, otherwise turns white. An indicator at the top of the screen indicates which ship is hit, or has sunk during the turn. The owner of the ships has an update on their grid as well to know where the enemy has attacked.
</br>Once all the ships of a player have sunk, the winner is called and it is possible to restart a game by clicking at a button at the top left of the screen.

## Project explanation
I chose to use the Unity Engine to create the project as I both have experience with it and it allows to create programs like this one easily. Unity allows the user to create objects and has lots of built in functions that can be used in order to facilitate UI events.
</br>The version used is the 2022.3 (LTS) in order to assure best maintanability of the software in the future. Scripts are written in C# and can be found at ``Battleship\Assets\Scripts``.

### Scene details
The project contains one Scene, in which we have all the game objects that are used:
- Camera object --> to natively handle graphics and what is shown on screen
- Game Manager object --> to handle all the game logic and contains references to objects that need to be accessed frequently
- UI Canvas object --> contains the different UI buttons and text elements relative to the game :
  - The mask used to hide the player’s grids between turns
  - The text info to display who has to play and what ships have been hit
  - The Replay button to restart the game
- 2 player objects --> similar but independant objects that contain:
  - 1 OwnGrid object on which he can place ships and see where the enemy attacks
  - 1 EnemyGrid object to attack and see where he has hit the enemy ships
  - 1 Canvas object with buttons used to place ships during the first phase of the game
 </br>![scene](https://github.com/Kyysel/softwerk-battleship/assets/39512699/2cfe1a3e-5fde-4e28-a359-87a2901afe48)


### Script Architecture
- The ``GameManager`` Script :
  - Handles the game logic.
  - Used to toggle player UIs on/off.
  - Has game states that are used for the 3 states of the game : ``Placing ships, Fighting, Ending``.
  - It’s a static game object that can be accessed from any other script.
  - When the ``NextTurn()`` method is called, it adds a filter on top of the screen to prevent the other player from looking at the opponents ships.
 
- The ``PlayerManager`` script is used to regroup all necessary information about a player, its ships, grids and related game logic. It is used to check the state of the grid of the player in order to know if all ships have been sunk, to place the ships on the grid and to handle the related UI events at the different stages of the game.
- The ``Tile`` and ``Ship`` scripts are self explanatory and store information about tile and ship states.
 
- Grids Scripts :
  - The ``GridManager`` is responsible of handling the grid generation and events.
  - The ``OwnGrid`` script herits from the ``GridManager`` but its behaviour is adapted to handling the placement of the ships. It checks if ships can be placed in a specific tile, and in what orientation.
  - The ``EnemyGrid`` script herits from the ``GridManager`` but its behaviour is adapted to handling the fighting stage of the game. It checks if the selected tile has been hit in the past, if it has a ship and if so, updates the corresponding tiles.
 
## Evolutions
The project does not contain test cases at the moment as I focused on having something that worked in the given amount of time.</br>
Both web and windows versions of the release build have an issue where the GridManager does not create the grid dictionnary containing the tiles. I have tried many ways of handling it, but ultimately didn’t find a constant fix. (Sometimes, just restarting the Unity Editor fixed the problem on my end, but it’s random and not reliable. It feels out of hand)</br>
When we rotate the ship, we don’t have a direct visual indication if the ship can be placed or not. We have to move the mouse to another tile to refresh the check --> adding a specific case in the code for that would fix the issue.</br>
The UI isn’t the cleanest as on some displays the UI elements might appear out of screen (on resolutions different than 16:9)</br>
There is a lack of information on the UI for the player to know which grid corresponds to what, and what he is supposed to do. (issue only for the player)</br>
The code of the ``GameManager`` script could be refactored in order to keep a single use for each method. For example the ``NextTurn`` method calls the ``HandleWaitScreen`` but it’s purpose isn’t only to handle the wait screen, it also changes game states and activates / deactivates UI elements. 
**Method names should be self explanatory and only be used for a single purpose.**
With a little more time this is something that can easily be improved.
