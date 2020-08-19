using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Pathfinding : MonoBehaviour {

	GridGizmos grid;
	public Vector3[] path = new Vector3[0];
	PathRequestManager requestManager;

	void Awake() {
		//Initializes PathRequestManager and GridGizmos instances
		requestManager = GetComponent<PathRequestManager>();
		grid = GetComponent<GridGizmos>();
	}


	//Returns the path from start position to target
	//Being N the number of nodes in a grid, the time complexity
	//will be, on average, O(N^2), as in the worst case scenario you will have to
	//check every node
	public void StartFindPath(Vector3 startPos, Vector3 targetPos) {

		Vector3[] path = new Vector3[0];
		bool pathSuccess = false;
		//Initializes start and end node, storing their x and y position 
		//from the game grid
		Node startNode = grid.NodeFromWorldPoint(startPos);
		Node targetNode = grid.NodeFromWorldPoint(targetPos);

		//Check if both start and end node are walkable by the object
		if (startNode.walkable && targetNode.walkable) {
			//Will store the nodes immediatelly available to the algorithm
			//to determine its cost
			List<Node> open = new List<Node>();
			//Will store the nodes alredy seen by the algorithm
			//The closed set is used only for lookup purposes,
			//so it is better to create it as a hash table-like structure, so
			//the lookup happens at O(1) time
			HashSet<Node> closed = new HashSet<Node>();
			open.Add(startNode);

			//While there are nodes to visit, keep running the algorithm
			while (open.Count > 0) {
				Node node = open[0];


				//Find the node with the lowest cost from all the nodes visited so far
				//This lookup takes O(node count) time
				for (int i = 1; i < open.Count; i ++) { 
					if (open[i].fCost < node.fCost || open[i].fCost == node.fCost) {
						//Prioritizes nodes with the lowest distance from the current node
						//to the target node
						if (open[i].hCost < node.hCost)
							node = open[i];
					}
				}
				//If node is the target node, exit the loop
				if (node == targetNode) {
					pathSuccess = true;
					break;
				}

				//Removes node from the list of available nodes and adds it to 
				//the list of nodes already seen
				open.Remove(node);
				closed.Add(node);

				//This takes O(nodes) time 
				//This loop gets all the neighbours of the current node. 
				foreach (Node neighbour in grid.GetNeighbours(node)) {
					//If the node is located at an area that the user cannot go, or if 
					//the algorithm has already seen that node, breaks the for loop
					//(obs: because the closed variable is a Hash Table, the lookup takes O(1) time)
					if (!neighbour.walkable || closed.Contains(neighbour)) {
						continue;
					}
					//Calculates the g cost, which is how far from the current node each neighbour is
					int costNeighbour = node.gCost + GetDistance(node, neighbour);
					//If the cost to go from the current node to the neighbour is less than 
					//If the algorithm has not seen this neighbour yet or if the cost 
					// to go to it is less than the cost associated with the node so far, then
					// set the neighbour's parent as being the current node
					if (costNeighbour < neighbour.gCost || !open.Contains(neighbour)) {
						//Update (or insert new) cost from node to current neighbour
						neighbour.gCost = costNeighbour;
						// Calculate the distance between the neighbour and the final target node
						neighbour.hCost = GetDistance(neighbour, targetNode);
						neighbour.parent = node;
						//If neighbour is being visited, add it to the open list
						if (!open.Contains(neighbour))
							open.Add(neighbour);
					}
				}
			}
		}
		//If reached target node, retrace the path
		if (pathSuccess) {
			path = RetracePath(startNode,targetNode);
		}

		//After it finds the path, it calls the PathRequestManager object to 
		//redirect the call to the Player Controller class, and gives the command for its instance
		//to move towards the target node following the provided path
		requestManager.CallCallback(path,pathSuccess);
	}



	//Retraces the path found by the find path function
	//It returns the path that the object should follow
	//Starting by the last node, which is the target, this function
	//retraces the back the path through each node's parent
	//until it reaches the initial position
	public Vector3[] RetracePath(Node startNode, Node endNode) {
		List<Node> path = new List<Node>();
		Node currentNode = endNode;
		List<Vector3> pathVector = new List<Vector3>();
		while (currentNode != startNode) {
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		if (path.Count > 0) {
			path.Reverse ();

			//This creates a path at the Gizmos grid
			//to be highlighted when running the game on
			//developer mode
			grid.path = path;
			//Gets the x,y position of each node
			for (int i = 0; i < path.Count; i++) {
				pathVector.Add (path [i].worldPosition);
			}
		}
		return pathVector.ToArray();
	}


	//Calculates the distance of two nodes, given their
	//x and y position in the game grid
	int GetDistance(Node nodeA, Node nodeB) {
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
		//if the the direction from one node to the other is
		//diagonal, the distance is associated with a cost of 14,
		//if not, associated with the cost of 10
		if (dstX > dstY)
			return 14*dstY + 10* (dstX-dstY);
		else
			return 14*dstX + 10 * (dstY-dstX);
	}
}