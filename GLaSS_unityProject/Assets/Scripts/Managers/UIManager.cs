﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public static UIManager Instance;
	//public Canvas UI;
	public GameManager Game;

	public bool gameTutActive = false;

	// Gameplay Tutorial popups UI
	public GameObject gravityTutGame;
	public GameObject saturationTutGame;
	public GameObject inertiaTutGame;
	public GameObject brownianTutGame;
	public GameObject vdwTutGame;

	// Scientific Tutorial popups UI: ++++ not done yet
	public GameObject gravityTutScience;
	public GameObject inertiaTutScience;
	public GameObject brownianTutScience;
	public GameObject vdwTutScience;

	public ArrayList tutList = new ArrayList();
	public bool paused = false;
	public bool revisit = false;

	public int i = 0;
	public GameObject item;
	public float h;
	public string input;
	public bool gotInput;
	//public Animator animRight;

	//-------------------------------------------//

	void Awake() {
		if (Instance == null)
			Instance = this;
		
		Game = FindObjectOfType<GameManager> ();
		//animRight = transform.FindChild("AnimRightSlide").GetComponent<Animator> ();
	}

	void Start() {
		CloseAllPopups ();				//  close all active UI windows, if any
	}

	void Update() {
		if (gameTutActive) {																		// if a popup is open, pause the game
			if (!paused)
				PauseGame ();
			// TODO later: disable "pressing" powers while paused!
			if (Input.GetKeyDown (KeyCode.JoystickButton5) || Input.GetKeyDown (KeyCode.P)) { 		// TO CLOSE. if u press RB then close the popup and resume the game 
				revisit = false;						// we are not revisiting the tutorials anymore
				i = 0;									// reset it to zero			
				CloseAllPopups ();
				ResumeGame ();
				if (!tutList.Contains (saturationTutGame)) {					// if we haven't seen saturiation tutorial yet, show it
					Debug.Log("saturation");
					saturationTutGame.SetActive (true);
					gameTutActive = true;
					tutList.Add (saturationTutGame);
				}
			}

		} else {																// TO OPEN. if u press RB then pause & bring up the popups
			if (Input.GetKeyDown (KeyCode.JoystickButton5) || Input.GetKeyDown (KeyCode.P)) { 		
				
				if (tutList.Count > 0) {
					item = tutList [i] as GameObject;
					//animRight.
					item.SetActive (true);
					revisit = true;
					if (!paused) {
						PauseGame ();
					}
				} 
			}
		}

		if (revisit) {
			h = Input.GetAxis ("Horizontal");
			if ((h >= 0.7f && h <= 1f) || Input.GetKeyDown(KeyCode.RightArrow)) {
				input = "right";
				gotInput = true;
				StartCoroutine ("WaitForQuiet");

			} else if ((h <= -0.7f && h >= -1) || Input.GetKeyDown(KeyCode.LeftArrow)) {
				input = "left";
				gotInput = true;
				StartCoroutine ("WaitForQuiet");
			}
			if (input == "right" && gotInput && h == 0) {
				ShowNext ();
			}
			else if (input == "left" && gotInput && h == 0) {
				ShowPrevious ();	
			}
		}
	}



		// power progression: gravity, antibrownian, antiVDW, inertia

	IEnumerator WaitForQuiet() {
		yield return new WaitUntil (() => (h == 0));
	}

	public void ShowNext() {
		i++;
		if (i >= tutList.Count) {							// if we reach the last tutorial, go to the first one
			i = 0;
		}
		item.SetActive (false);	
		item = tutList [i] as GameObject;
		item.SetActive (true);
		gotInput = false;
	}

	public void ShowPrevious() {
		i--;
		if (i < 0) {									// if we reach the first tutorial, go to the last one
			i = tutList.Count-1;
		}
		item.SetActive (false);
		item = tutList [i] as GameObject;
		item.SetActive (true);
		gotInput = false;
	}
		

	public void CloseAllPopups() {
		// Gameplay tutorial popups
		foreach (GameObject tut in tutList) { 
			tut.SetActive (false);
		}
	}

	void PauseGame() {
		Time.timeScale = 0f;
		paused = true;
		gameTutActive = true;
	}

	void ResumeGame() {
		Time.timeScale = 1f;
		gameTutActive = false;
		paused = false;
	}


}
