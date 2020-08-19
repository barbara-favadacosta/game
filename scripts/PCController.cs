using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PCController : MonoBehaviour {

	//Auxiliary game objects
	Transform target;
	GameObject[] door;
	GameObject[] doorObject;
	GameObject textDoorObject;
	PlayerController p;

	//Speed variable determines the speed with which the computer
	//will move its player. It is set to public so it can be easily changed
	//at the GUI Unity developer interface
	public float speed;

	//Private variables
	bool isRunning;
	bool isWinner;
	int low, high;
	int targetInt;
	int targetIndex;
	Vector3[] path;
	String s;


	void Start() {

		//Gets Player.cs script associated with Player game object
		GameObject obj = GameObject.FindGameObjectWithTag ("Player");
		p = obj.GetComponent<PlayerController>();

		//Initializes variables
		//isRunning tells the algorithm if the PC player is moving in the game
		//isWinner tells if the PC player found the right target
		isRunning = false;
		isWinner = false;

		//Finds all the Door game objects in the game
		door = GameObject.FindGameObjectsWithTag ("Door");
		//Initializes array that will store the number ID of each door
		int[] doorNumber = new int[door.Length];
		//Initialies array that will store the Door objects in ascending order
		doorObject = new GameObject [door.Length];

		//This loop will go through every door and extract the TextMesh object that contains
		// the door ID and add to the doorNumber array
		int n;
		for (int i = 0; i < door.Length; i++) {
			var c = door[i].transform.GetChild(0);
			var t = c.GetComponent<TextMesh> ().text.ToString ();
			Int32.TryParse(t,out n);
			doorNumber[i] = n;
		}

		//The built in Array.Sort function will sort an array using insertion sort
		//if the array length is up until 16 elements; heapsort if the
		//length is above 2*log(n^2); and quicksort for every other case.
		//In this case it will use insertion sort because we only have 4 doors,
		//which will take O(n^2) time, as for every item in the array it will compare to every
		//element seen by the algorithm so far.
		Array.Sort (doorNumber);

		//This loop will go the door array (which contains all the
		//doors in the game) and get its door ID.
		//Then, it will go through the array containing all the door ID's
		//and find the match. With the matched element, it will add
		//the door object into the ordered doorObject array at the index
		//that corresponds to the number ID at the doorNumber array.
		//Finally, we will have an oredered array with containing all the door objects
		for (int i = 0; i < door.Length; i++) {
			var c = door[i].transform.GetChild(0);
			var t = c.GetComponent<TextMesh> ().text.ToString ();
			Int32.TryParse(t,out n);
			for(int j = 0; j < doorNumber.Length; j++){
				if (n == doorNumber [j]) {
					doorObject [j] = door [i];
				}
			}	
		}

		//Initializes variables responsible for the 
		//range of the target select method
		low = -1;
		high = door.Length;
	}

	//This function will randomly select a new door
	//that is within the range provided by the CheckTarget function
	void SelectTarget(){
		int index = UnityEngine.Random.Range (low+1, high);
		target = doorObject [index].transform;
		var t = target.GetChild(0);
		s = t.GetComponent<TextMesh> ().text.ToString ();
		Int32.TryParse(s,out targetInt);

	}

	//This function is called at every frame in the game
	void Update(){
		//If the PC player is not moving; and have not won; 
		//and the player has not won (indicated by then p.canMove variable);
		//then the algorithm will select the new target and get the path towards
		//it. It will also fire the OnPathFound, which is the callback function for the 
		//RequestPath fucntion.
		//Finally, it will update the range with which the next door (target) ID
		//will have to fall in between.
		if (!isRunning && !isWinner && p.canMove) {
			SelectTarget ();
			PathRequestManager.RequestPath (transform.position, target.position, OnPathFound);
			CheckTarget ();

		}
	}

	//Given the current target chosen, this function will
	//check if the target chosen (after the PC player arrives at the destination)
	//is the right one. If it is not, it will update the range
	//with which the next SelectTarget function will choose from.
	public void CheckTarget(){
		int finalTarget = PathRequestManager.GetTarget ();
		if (targetInt == finalTarget) {
			isWinner = true;
		} else {
			if (targetInt < finalTarget)
				low = Array.IndexOf (doorObject, target.gameObject);
			else if(targetInt > finalTarget)
				high = Array.IndexOf (doorObject, target.gameObject);
		}
	}

	//If the path was found, this function will fire the FollowPath function
	public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
		if (pathSuccessful  && newPath.Length > 0) {
			path = newPath;
			targetIndex = 0;
			//This will first stop the FollowPath Coroutine if it is still being
			//run and start a new FollowPath Coroutine
			//Coroutine allows a process to take place in parallel with the other
			//processes. The moving action makes use of this because we don't want that
			//the whole game freezes until the PC player has arrived at the target. If
			//that would to happen, then it would not be possible to move
			//our player at the same time as the PC player is moving
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
			isRunning = true;
		}
	}

	//The IEnumerator interface allows to return a function at a specific
	//point and, when called again, the function will get back where it left.
	//This function will make the PC player move towards the target
	IEnumerator FollowPath() {
		//Gets the first x,y position from the path
		Vector3 currentWaypoint = path[0];
		while (true) {
			//Checks if PC player arrived at the specified x,y position
			if (transform.position == currentWaypoint) {
				targetIndex ++;
				//If it arrived at the target position, it will break the 
				//routine and stop moving
				if (targetIndex >= path.Length) {
					isRunning = false;
					yield break;
				}
				//Gets next x,y position from the path
				currentWaypoint = path[targetIndex];
			}
			//This will ensure that the object does not move in the z axis, but only x and y
			//(because it is a 3D object, in Unity y represents what we know as the z axis)
			transform.position = new Vector3 (transform.position.x, 1, transform.position.z);
			currentWaypoint.y = 1;
			//This will ensure that the object does not rotate
			transform.rotation = Quaternion.Euler(0, 0, 0);
			//MoveTowards function wiill update the object position to the desired x,y point
			transform.position = Vector3.MoveTowards(transform.position,currentWaypoint,speed * Time.deltaTime);
			yield return null;
		}

	}

}