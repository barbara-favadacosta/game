using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//Except for the Awake() fucntion, this class was retrieved from 
//the video tutorial A* pathfinding (units) at https://www.youtube.com/watch?v=dn1XRIaROM4
//by Sebastian Lague
//This class ensures that the movement of the PC player happens in 
//prallel with the other processes
public class PathRequestManager : MonoBehaviour {

	Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
	//PathRequest currentPathRequest;
	PathRequest request;

	static PathRequestManager instance;
	Pathfinding pathfinding;

	bool isProcessingPath;

	GameObject[] door;
	GameObject doorObject;
	GameObject textDoorObject;

	int target;

	//This function randomly chooses the winning door at the beginning of the game
	//This function is called when the game is open
	//and before the Start() function, which ensures that the 
	//door with the winner ID will be picked before
	//any other function is run.
	void Awake() {
		instance = this;
		pathfinding = GetComponent<Pathfinding>();
		door = GameObject.FindGameObjectsWithTag ("Door");
		int index = UnityEngine.Random.Range (0, door.Length);
		var d = door[index].transform;
		var t = d.GetChild(0);
		String s = t.GetComponent<TextMesh> ().text.ToString ();
		Int32.TryParse(s,out instance.target);
	}

	public static int GetTarget(){
		return instance.target;
	}

	public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback) {
		instance.request = new PathRequest(pathStart,pathEnd,callback);
		instance.CallPath(instance.request.pathStart,instance.request.pathEnd);
	}

	void CallPath(Vector3 pathStart, Vector3 pathEnd){
		pathfinding.StartFindPath(pathStart, pathEnd);
	}

	public void CallCallback(Vector3[] path, bool success){
		request.callback(path,success);
	}


	public struct PathRequest {
		public Vector3 pathStart;
		public Vector3 pathEnd;
		public Action<Vector3[], bool> callback;

		public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback) {
			pathStart = _start;
			pathEnd = _end;
			callback = _callback;
		}

	}
}