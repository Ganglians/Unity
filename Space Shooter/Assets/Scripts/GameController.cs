using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {
	public GameObject[] hazards;
	public GameObject restartButton;

	public Vector3 spawnValues;

	public int hazardCount;

	public float spawnWait;
	public float startWait;
	public float waveWait;

	public Text scoreText;
//	public Text restartText;
	public Text gameOverText;
	


	private bool gameOver;
	private bool restart;

	private int score;

	void Start () {
		gameOver = false; // Make sure game over and restart not triggered 
		restart = false;

//		restartText.text = ""; // Effectively turning off the labels
		gameOverText.text = "";

		restartButton.SetActive (false);

		score = 0;
		UpdateScore ();

		StartCoroutine (SpawnWaves ()); // Making function a coroutine
	}

//	void Update () {
//		if (restart && Input.GetKeyDown (KeyCode.R)) {
//			Application.LoadLevel (Application.loadedLevel); // Reload currently loaded lv.
//		}
//	}

	IEnumerator SpawnWaves () { // iEnumerator to not make whole game wait
		yield return new WaitForSeconds (startWait);
		while (true) {
			for (int i = 0; i < hazardCount; ++ i) {
				GameObject hazard = hazards [Random.Range(0, hazards.Length)];
				Vector3 spawnPosition = new Vector3 (Random.Range (- spawnValues.x, spawnValues.x),
				                                                   spawnValues.y, 
				                                                   spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (hazard, spawnPosition, spawnRotation);
				yield return new WaitForSeconds (spawnWait); // So whole game doesn't wait just the asteroid spawning
			}
			yield return new WaitForSeconds (waveWait); // Space out each wave of asteroids

			if(gameOver) {
				restartButton.SetActive (true);
//				restartText.text = "Press 'R' to Restart";
				restart = true;
				break;
			}
		}
	}

	// Exposed to receive signals from hazards if they're destroyed
	public void AddScore (int newScoreValue) {
		score += newScoreValue;
		UpdateScore ();
	}

	void UpdateScore () {
		scoreText.text = "Score: " + score;
	}

	public void GameOver () {
		gameOverText.text = "Game Over";
		gameOver = true;
	}

	// LoadLevel has to be called from a function if we are using a button
	public void RestartGame () {
		Application.LoadLevel (Application.loadedLevel);
	}
}
