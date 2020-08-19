using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is
//used only to aid in the development process.
//It creates a grid containing displaying all the 10x10 nodes used 
//to calculate the path between two points
//and highlights the path chosen at each new target selection
//This class was retrieved from the video tutorial A* pathfinding (node grid)
// by Sebastian Lague at https://www.youtube.com/watch?v=nhiFx28e7JY
public class GridGizmos : MonoBehaviour {
	public Vector2 gridWorldSize;
	public Transform player;
	public float nodeRadius;
	public LayerMask unwalkableMask;
	Node[,] grid;
	float nodeDiameter;
	int gridSizeX, gridSizeY;
	public List<Node> path;

	void Awake()
	{
		//How many nodes can we fit in the grid
		nodeDiameter = nodeRadius * 2;
		gridSizeX = Mathf.RoundToInt (gridWorldSize.x / nodeDiameter);
		gridSizeY = Mathf.RoundToInt (gridWorldSize.y / nodeDiameter);
		CreateGrid ();
	}

	void CreateGrid()
	{	
		grid = new Node[gridSizeX, gridSizeY];
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

		for (int x = 0; x < gridSizeX; x++) 
		{
			for (int y = 0; y < gridSizeY; y++) {
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) +
				                     Vector3.forward * (y * nodeDiameter + nodeRadius);
				bool walkable = !(Physics.CheckSphere(worldPoint,nodeRadius,unwalkableMask));
				grid[x,y] = new Node(walkable,worldPoint,x,y);
			}	
		}
	}

	public List<Node> GetNeighbours(Node node){
		List<Node> neighbours = new List<Node> ();

		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if (x == 0 && y == 0)
					continue;
				int checkX = node.gridX + x;
				int checkY = node.gridY + y;

				if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {
					neighbours.Add(grid[checkX, checkY]);
				}
			}
		}
		return neighbours;
	}

	public Node NodeFromWorldPoint(Vector3 worldPosition)
	{
		int x = Mathf.RoundToInt((worldPosition.x + gridWorldSize.x / 2 - nodeRadius)/nodeDiameter);
		int y = Mathf.RoundToInt((worldPosition.z + gridWorldSize.y / 2 - nodeRadius)/nodeDiameter);

		x = Mathf.Clamp(x, 0, gridSizeX - 1);
		y = Mathf.Clamp(y, 0, gridSizeY - 1);

		return grid[x, y];
	}



	void OnDrawGizmos()
	{
		Gizmos.DrawWireCube (transform.position, new Vector3 (gridWorldSize.x, 1, gridWorldSize.y));
		if (grid != null) {
			foreach (Node n in grid) {
				Gizmos.color = (n.walkable) ? Color.white : Color.green; 

				if (path != null) {
					if(path.Contains(n)){
						Gizmos.color = Color.black;
					}
				}
				Gizmos.DrawCube (n.worldPosition, Vector3.one * (nodeDiameter - .1f));
			}
		}
	}
}
