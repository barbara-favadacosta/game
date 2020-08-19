using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TriggerManager : MonoBehaviour {

	int target;
	//Each player has its own display
	//to indicate if the target the player arrived to is the 
	//winning one. If it is not, it will display whether
	//its ID is lower or higher than the winning one
	TextMesh displayPlayer;
	TextMesh displaySeeker;
	PlayerController p;

	void Start(){
		//Initializes display information variables
		target= PathRequestManager.GetTarget ();
		GameObject obj = GameObject.FindGameObjectWithTag ("DisplayInfo");
		displayPlayer = obj.GetComponent<TextMesh> ();

		obj = GameObject.FindGameObjectWithTag ("DisplayInfoPC");
		displaySeeker = obj.GetComponent<TextMesh> ();

		obj = GameObject.FindGameObjectWithTag ("Player");
		p = obj.GetComponent<PlayerController>();

	}

	// When a player hits one of the doors, it will 
	//fire this function,which will update the target status 
	//of the player's display
	void OnTriggerEnter(Collider player){

		target = PathRequestManager.GetTarget ();
		var t = this.transform.GetChild(0);
		TextMesh text = t.GetComponent<TextMesh> ();


		int doorNumber;
		Int32.TryParse(text.text,out doorNumber);

		if (player.tag == "Player") {
			if (doorNumber == target) {
				displayPlayer.text = "You won!";
				p.canMove = false;
			}
			else if (doorNumber < target)
				displayPlayer.text = "Door number too low!";
			else if (doorNumber > target)
				displayPlayer.text = "Door number too high!";
		}
		else if (player.tag == "Seeker") {
			if (doorNumber == target){	
				displaySeeker.text = "The computer won!";
				p.canMove = false;
			}
			else if (doorNumber < target)
				displaySeeker.text = "Door number too low!";
			else if (doorNumber > target)
				displaySeeker.text = "Door number too high!";
		}
			
	}
}
