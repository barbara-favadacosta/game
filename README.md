

# How to play:
- Run the executable
- Select 800x600 as the resolution and check "Windowed" box
- Use keyboard arrow keys to move around 
- Click the Restart button to restart the game

### Suggestion:
- The first few times you run the game, don't move the player so you can observe how the 
computer moves around until it wins. 
- Try to beat it!

-----------------------------------------------------------------------------------------------------------
# The Game

You and another person wake up at a stranger place and an alien voice comes out of the speakers:

 “You, earthlings, are my prisoners. I’m responsible for researching cognitive capabilities of new form of lives and you are my test subjects. However, I only need one person, and you’ll play a game to decide who stays with me. One of the doors is the exit, and whoever gets to it first can go free. Each door has a number above it, so I will play a hot and cold game with you. Whenever you get to a door, I will tell if the door number is lower or higher than the exit door number. Good Luck!”

The user plays against the computer to find the right door to exit out of the room. The player uses the arrow keys in the keyboard to move around obstacles to reach the doors until it finds the right one. The computer has to perform the same task, but the path finding and choosing the door processes have to happen without user interference.

In order to find the optimum path between the computer’s current position and the target door, I implemented the A* algorithm, a best-first search algorithm. This algorithm is an extension of the Dijkstra algorithm and will find a shortest path between two points.

To find the next door target, the computer should use the same thought process as the user will probably use to decide on each door target, so the competition is close to fair. That is, when it is displayed whether or not the current door that the player arrived to has a number higher or lower than the winner door number, the user will make a mental note to ensure that the next door he choses falls within that boundary. In order to simulate this reasoning, the game adapts  the binary search algorithm to serve this purpose.

## Deliverable
The game deliverable is the skeleton of the final version. The goal of this version is to make sure that the gameplay reflects the game proposal. The next steps are to implement the proper visual elements into it and develop additional screens to support the game, such as initial screen and “how to play” screen.

![](/Capture.PNG)

Figure 1- Image of the game taken from the developer mode environment. The image on the left shows the computer going from the door 30 to the door with number 10. The image on the right shows the computer going from the initial state to the door with number 40

The purple boxes represent the obstacles that the players have to be aware of. Each white box without a number of top of it represents one of the players, and the boxes with numbers on top of them represent the doors. The display on the top will output information regarding the computer’s decisions and the display on the bottom will output information regarding the player’s decisions. This is an image taken from the developer’s environment, which shows the path that the computer takes, the nodes that are walkable (in grey),and the nodes that are not walkable (in green). The unwalkable nodes are the ones that contain any area of the obstacles in purple. 


