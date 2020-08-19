using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

//This class is attached to the Restart button.
//It ensures that when the button is clicked, the game will restart
public class Restart : MonoBehaviour {

	public void RestartGame() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

}