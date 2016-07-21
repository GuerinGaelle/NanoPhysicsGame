using UnityEngine;
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
	public GameObject viscosityTutScience;

	public ArrayList tutListGame = new ArrayList();
	public ArrayList tutListScience = new ArrayList();
	public bool paused = false;
	public bool revisit = false;

	public int i = 0;
	public GameObject item;
	public float h;
	public string input;
	public bool gotInput;

	public bool scienceTutActive = false;
	public bool revisitSc = false;
	public int indexSc = 0;


	//public Animator animRight;

	//-------------------------------------------//

	void Awake() {
		if (Instance == null)
			Instance = this;
		
		Game = FindObjectOfType<GameManager> ();
		//animRight = transform.FindChild("AnimRightSlide").GetComponent<Animator> ();
	}

	void Start() {
		//CloseAllPopups ();				//  close all active UI windows, if any
		CloseGameplayPopups();
		CloseScientificPopups ();
	}

	void Update() {
		// ------ FOR GAMEPLAY TUTORIALS! ------
		if (gameTutActive) {																		// if a popup is open, pause the game
			if (!paused)
				PauseGame ();
			// TODO later: disable "pressing" powers while paused!
			if (Input.GetKeyDown (KeyCode.JoystickButton5) || Input.GetKeyDown (KeyCode.P)) { 		// TO CLOSE. if u press RB then close the popup and resume the game 
				//CloseAllPopups ();
				CloseGameplayPopups ();
				ResumeGame ();
				if (!tutListGame.Contains (saturationTutGame) && tutListGame.Contains (gravityTutGame)) {	// if we haven't seen saturiation tutorial yet, show it
					AddSaturation ();
				}

			} else if ((Input.GetKeyDown (KeyCode.JoystickButton4) || Input.GetKeyDown (KeyCode.O)) && tutListScience.Count > 0) {		// TO OPEN SCIENTIFIC
				//CloseAllPopups ();
				if (!tutListGame.Contains (saturationTutGame) && tutListGame.Contains (gravityTutGame)) {	// if we haven't seen saturiation tutorial yet, show it
					CloseGameplayPopups ();
					AddSaturation ();

				} else {
					CloseGameplayPopups ();					// close scientific tutorials
					ShowScientificTutorials ();
				}
			}

		} 
		// ------ FOR SCIENTIFIC TUTORIALS! ------
		else if (scienceTutActive) {
			if (!paused)
				PauseGame ();
			// TODO later: disable "pressing" powers while paused!
			if (Input.GetKeyDown (KeyCode.JoystickButton4) || Input.GetKeyDown (KeyCode.O)) { 			// TO CLOSE SCIENCE with LB	
				//CloseAllPopups ();
				CloseScientificPopups ();
				ResumeGame ();
			} else if ((Input.GetKeyDown (KeyCode.JoystickButton5) || Input.GetKeyDown (KeyCode.P)) && tutListGame.Count > 1) {		// TO OPEN GAMEPLAY
				//CloseAllPopups ();
				Debug.Log ("3");
				CloseScientificPopups ();					// close scientific tutorials
				Debug.Log ("4");
				ShowGameplayTutorials ();
				//gameTutActive = true;
				//scienceTutActive = false;
			}
		} else if (Input.GetKeyDown (KeyCode.JoystickButton4) || Input.GetKeyDown (KeyCode.O)) { 				// TO OPEN SCIENCE with LB
			ShowScientificTutorials ();
		} else if (Input.GetKeyDown (KeyCode.JoystickButton5) || Input.GetKeyDown (KeyCode.P)) {
			ShowGameplayTutorials ();
		}



		if (revisit) {
			gameTutActive = true;
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

		if (revisitSc) {
			scienceTutActive = true;
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
				ShowNextSc ();
			}
			else if (input == "left" && gotInput && h == 0) {
				ShowPreviousSc ();	
			}
		}
	}

	void ShowGameplayTutorials() {
		Debug.Log ("1");
		if (!gameTutActive) {
			if (tutListGame.Count > 0) {
				item = tutListGame [i] as GameObject;
				//animRight.
				Debug.Log ("2");
				item.SetActive (true);
				revisit = true;
				gameTutActive = true;		// TEST?
				if (!paused) {
					PauseGame ();
				}
			}
		}
	}

	void ShowScientificTutorials() {
		if (!scienceTutActive) {
			if (tutListScience.Count > 0) {
				item = tutListScience [indexSc] as GameObject;
				//animRight.
				item.SetActive (true);
				revisitSc = true;
				scienceTutActive = true; // TEST?
				if (!paused) {
					PauseGame ();
				}
			} 
		}
	}
		// power progression: gravity, antibrownian, antiVDW, inertia

	void AddSaturation() {	
		Debug.Log("saturation");
		saturationTutGame.SetActive (true);
		gameTutActive = true;
		tutListGame.Add (saturationTutGame);
	}
	IEnumerator WaitForQuiet() {
		yield return new WaitUntil (() => (h == 0));
	}

	public void ShowNext() {
		i++;
		if (i >= tutListGame.Count) {							// if we reach the last tutorial, go to the first one
			i = 0;
		}
		item.SetActive (false);	
		item = tutListGame [i] as GameObject;
		item.SetActive (true);
		gotInput = false;
	}

	public void ShowPrevious() {
		i--;
		if (i < 0) {									// if we reach the first tutorial, go to the last one
			i = tutListGame.Count-1;
		}
		item.SetActive (false);
		item = tutListGame [i] as GameObject;
		item.SetActive (true);
		gotInput = false;
	}
		
	public void ShowNextSc() {
		indexSc++;
		if (indexSc >= tutListScience.Count) {							// if we reach the last tutorial, go to the first one
			indexSc = 0;
		}
		item.SetActive (false);	
		item = tutListScience [indexSc] as GameObject;
		item.SetActive (true);
		gotInput = false;
	}

	public void ShowPreviousSc() {
		indexSc--;
		if (indexSc < 0) {									// if we reach the first tutorial, go to the last one
			indexSc = tutListScience.Count-1;
		}
		item.SetActive (false);
		item = tutListScience [indexSc] as GameObject;
		item.SetActive (true);
		gotInput = false;
	}

	public void CloseGameplayPopups() {
		// Gameplay tutorial popups
		foreach (GameObject tut in tutListGame) { 
			tut.SetActive (false);
			revisit = false;						// we are not revisiting the tutorials anymore
			i = 0;									// reset it to zero	
			gameTutActive = false;			// TEST?
		}
	}
	public void CloseScientificPopups() {
		// Scientific tutorial popups
		foreach (GameObject tutSc in tutListScience) {
			tutSc.SetActive (false);
			revisitSc = false;						// we are not revisiting the tutorials anymore
			indexSc = 0;									// reset it to zero	
			scienceTutActive = false;  			// TEST?
		}
	}

	void PauseGame() {
		Time.timeScale = 0f;
		paused = true;
		//gameTutActive = true;

	}

	void ResumeGame() {
		Time.timeScale = 1f;
		gameTutActive = false;
		scienceTutActive = false;
		paused = false;
	}


}
