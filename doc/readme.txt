Dragons & Miniguns v1.1
=======================

Gameplay
--------
- protect the treasure
- kill the enemies
- survive

Game will be over if all of the treasure is stolen or player gets killed.


Gameplay hints
--------------
- there are items on the map which may help your fight with the enemies
- you can't pick up items while carrying the treasure


Default keymapping
------------------
Movement                =  W,A,S,D
Look around             =  mouse movement
Fire                    =  left mouse button
Sprint                  =  shift
Handgrenade             =  G or right mouse button
Jump                    =  space
Pick-Up / Drop          =  E
Reload                  =  R
Change Weapon           =  mouse wheel or tab
Pistol                  =  1
Assault Rifle           =  2
Grenade Launcer         =  3
Minigun (if available)  =  4
Scar-L (if available)   =  5
Pause / Resume          =  esc


Menus
-----
- you can load your previous game progress from Main Menu or Pause Menu
- game progress can be saved at any time by pausing the game and selecting Save Game
- please note that you will lose any unsaved progress by going into main menu as there is no autosaving!


Start & Settings
----------------
Start the game by running DnM.exe. This will start the configurator where you can
change the graphics settings and alter the default keymapping.

If your computer can handle it, always run the game with "Fantastic" quality setting as this will 
make sure you have the best shadowing and antialiasing settings enabled.  "Normal" will give a 
compromise with simpler shadows and "Fastest" will disable all shadowing and antialiasing so the 
game can run on slower computers.

When everything is set, hit Play!-button and have fun!

Changelist 1.0->1.1
-------------------
fixes:
- game now correctly saves guns which player has picked up
- game now correctly saves enemy spawning
- game now correctly saves dragon battle
- gun animations didn't play always correctly
- minigun sounds don't keep playing if game ends while player is firing
- minigun sounds don't keep playing if game is paused while player is firing
- dragon can't be killed twice using grenades anymore
- sound of player hitting ground after jump is now always played correctly
- sprinting doesn't continue after cooldown period automatically
- story's next-button is now correctly shown after returning from game level

new features:
- game has now a difficulty setting

improvements:
- there is now a delay after player has been killed or treasure has been stolen
- player drops to the groumd when killed
- game now tells player when treasure has been fully stolen
- dead dragon ragdoll improvements
- save game dialog now shows a preview of save contents of the save slots
- tweaked gun animations while sprinting
- most of the materials now respond to gun fire (particle effects)
- sprinting now automatically stops while airborne
- mouse smoothing reduced to favor faster responce time
- player jumps now higher
- better balancing on enemy spawns
- fonts on main menu no longer blurry