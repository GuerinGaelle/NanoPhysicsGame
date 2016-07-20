using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

	public static UIManager Instance;
	//public Canvas UI;
	public GameManager Game;

	public bool gameTutActive = false;

	// Gameplay Tutorial popups UI
	public GameObject gravityTutGame;
	public GameObject inertiaTutGame;
	public GameObject brownianTutGame;
	public GameObject vdwTutGame;

	// Scientific Tutorial popups UI: ++++ not done yet
	public GameObject gravityTutScience;
	public GameObject inertiaTutScience;
	public GameObject brownianTutScience;
	public GameObject vdwTutScience;

	public bool paused = false;
	//-------------------------------------------//

	void Awake() {
		if (Instance == null)
			Instance = this;

		//UI = FindObjectOfType<Canvas> ();
		Game = FindObjectOfType<GameManager> ();
		gravityTutGame = GameObject.Find ("Tutorial Gravity");
		inertiaTutGame = GameObject.Find ("Tutorial Inertia");
		brownianTutGame = GameObject.Find ("Tutorial Brownian");
		vdwTutGame = GameObject.Find ("Tutorial VDW");

	}

	void Start() {
		CloseAllPopups ();
	}

	void Update() {
		if (gameTutActive) {																		// if a popup is open, pause the game
			if (!paused)
				PauseGame ();
			// TODO later: disable "pressing" powers while paused!
			if (Input.GetKeyDown (KeyCode.JoystickButton5) || Input.GetKeyDown (KeyCode.P)) { 		// if u press RB then close the popup and resume the game 
				CloseAllPopups ();
				ResumeGame ();
			}
		} 
		else {																// if u press RB then pause & bring up the popups
			if (Input.GetKeyDown (KeyCode.JoystickButton5) || Input.GetKeyDown (KeyCode.P)) { 		
				//PauseGame ();
				// TODO : show tutorials
				if (Game.isGravityUnlocked && !paused) {
					gravityTutGame.SetActive (true);
					PauseGame ();
				} else if (Game.isGravityUnlocked && paused) {
					gravityTutGame.SetActive (false);
					ResumeGame ();
				}
			}
		}

		// power progression: gravity, antibrownian, antiVDW, inertia
	}

	void CloseAllPopups() {
		// Gameplay tutorial popups
		gravityTutGame.SetActive (false);
		inertiaTutGame.SetActive (false);
		brownianTutGame.SetActive (false);
		vdwTutGame.SetActive (false);
	}

	void PauseGame() {
		Time.timeScale = 0f;
		paused = true;
	}

	void ResumeGame() {
		Time.timeScale = 1f;
		gameTutActive = false;
		paused = false;
	}
}
