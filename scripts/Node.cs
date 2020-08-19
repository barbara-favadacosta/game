using UnityEngine;
using System.Collections;

public class Node {

	//Variable to check whether or not the node is walkable, 
	//so if it doesn't have a object on top of it
	public bool walkable;
	//Stores x y position that the node is in the game 
	public Vector3 worldPosition;
	public int gridX;
	public int gridY;

	//These variables are used to calculate the shortest path,
	// gCost is the cost between the current node and the start node
	// hCost is the cost between the current node and the target node
	// parent is the parent node, which indicates the node that preceds the current node
	// on the path towards the player/Pc player initial position and target
	public int gCost;
	public int hCost;
	public Node parent;

	//Initializes Node object
	public Node(bool walkable, Vector3 worldPos, int gridX, int gridY) {
		this.walkable = walkable;
		this.worldPosition = worldPos;
		this.gridX = gridX;
		this.gridY = gridY;
	}

	//Returns fCost, which is the sum of gCost and hCost
	public int fCost {
		get {
			return gCost + hCost;
		}
	}
}