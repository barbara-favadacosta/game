using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	TriggerManager t;
	public float mSpeed;
	public bool canMove;
	void Start ()
	{
		//Sets the speed to which the player will move
		mSpeed = 7f;
		canMove = true;
	}


	//This function is called at every frame
	//It will make the player move in the up/down and right/left 
	//directions according to the keyboard arrows
	void Update ()
	{
		//If no one has found the winning door so far, the player can move
		if (canMove) {
			this.transform.rotation = Quaternion.Euler (0, 0, 0);
			this.transform.position = new Vector3 (this.transform.position.x, 1.5f, this.transform.position.z);

			this.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			this.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;

			transform.Translate (mSpeed * Input.GetAxis ("Horizontal") * Time.deltaTime, 0f, mSpeed * Input.GetAxis ("Vertical") * Time.deltaTime);

		}
	}


}﻿