# Hungry Pacman
Richard Klíma, NPRG035 C# language and .NET platform, Faculty of Mathematics
and Physics of Charles University, Prague, Czech Republic, winter semester
2019/2020

## Short annotation
The Hungry PacMan is a single player arcade game, where the player controls
the PacMan character and tries to navigate himself in the maze to eat all of
PacMan’s food, all while not being caught by enemy ghosts.

## Exact task
The program offers a clone of the old arcade PacMan game in seven maze maps
(levels). The player’s goal in each level is to eat all of PacMan’s regular food
(small white dot), while avoiding being caught by 2-4 enemy ghosts. PacMan can
also eat a killer food (large white dot), which allows him to eat the ghosts for
5 seconds, and bonus food (banana, strawberry, cherry), for which he gets bonus
score.\
\
The goal of the game is to complete all seven levels with the highest score
possible. PacMan gets 1 point for eating each regular food, 100 points for
eating a cherry, 150 points for eating a strawberry, 200 points for eating
a banana and 200 points for every ghost he manages to eat. The player has three
lives in the beginning, when all are lost, the game is over.

## Game information
The game is controlled by a “game loop”, a constant calling of `Update` and
`Draw` methods, which update the current situation in the game and display it on
the screen respectively.\
\
When PacMan collides with ghosts, he either loses a life or eats the ghosts,
depending on the ghosts not being or being blue after PacMan ate the killer
food.\
\
The ghosts have four different types of movements. The first two, chase and
scatter, are periodically changing after 10 seconds and each ghost has
a distinct target spot to which he tries to get in the maze.\
\
In the chase mode, the pink ghost pursues PacMan and his target is current
PacMan’s position. The purple ghost tries to ambush PacMan and aims four tiles
in front of PacMan, unless he is close to PacMan (less than 6 tiles measured
as Euclidean distance) when he gets frightened and runs to the lower right
corner of the maze. The red ghost counts his target spot from positions of
PacMan and the pink ghost – essentially his target is the same as the pink
ghost’s position, just on the other side of PacMan. The yellow ghost choses
a random turn on every crossroads.\
\
In the scatter mode, each ghost’s target spot is located outside the maze
in some corner. The pink ghost goes to the upper left corner, the purple ghost
to the lower right corner, the red ghost to the upper right corner and
the yellow ghost to the lower left corner.\
\
In the third mode, the blue ghost mode, all ghosts move slower than usual and
they choose a random turn on every crossroads.\
\
In the fourth mode, the dead mode, the ghost tries to get to his original
position in the maze, where he is then resurrected and continues his movement
in one of the first three modes, which is currently on.\
\
The game gets harder on every level, which is done by altering the maze
structure, number of enemy ghosts and number of killer food enabling PacMan
to eat the ghosts.

## User interface
The game presents itself in various “states”. The first state the player sees,
is the Menu state, where he can choose his next action by clicking shown
buttons. The Game state controls the game and player progress in it.
The other two states are Rules state and About state, where the player can find
some information about the game and about the rules of it.\
\
The player controls the game with mouse and uses arrow keys to move PacMan
in the maze.
