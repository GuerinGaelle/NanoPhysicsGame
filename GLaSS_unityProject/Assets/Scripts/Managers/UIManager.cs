using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public static UIManager Instance;
	public GameManager Game;

	// Gameplay Tutorial popups UI
	public GameObject gravityTutGame;
	public GameObject saturationTutGame;
	public GameObject inertiaTutGame;
	public GameObject brownianTutGame;
	public GameObject vdwTutGame;

	// Scientific Tutorial popups UI:
	public GameObject gravityTutScience;
	public GameObject inertiaTutScience;
	public GameObject brownianTutScience;
	public GameObject vdwTutScience;
	public GameObject viscosityTutScience;

	// Extra Science popups UI: 
	public GameObject extraGravity;
	public GameObject extraInertia;
	public GameObject extraBrownian;
	public GameObject extraVDW;
	public GameObject extraViscosity;

	public GameObject rotifera;
	public GameObject checkpoint;
	public GameObject particles;

	public ArrayList tutListGame = new ArrayList();
	public ArrayList tutListScience = new ArrayList();
	public ArrayList tutListScienceExtra = new ArrayList ();

	private bool paused = false;
	private bool revisit = false;
	private int i = 0;					// index of gameplay tutorials
	public GameObject item;			// the current item that is active
	private float h;				// value of the joystic horizontal axis
	private float v; 				// value of the joystic vertical axis
	public string input;
	private bool gotInput;
	public bool gameTutActive = false;

	public bool scienceTutActive = false;	
	private bool revisitSc = false;
	private int indexSc = 0;					// index for  for scientific tutorials
	public bool firstVisitSc = false;
	private bool showingMoreScience = false;

	public bool oneTimePopupActive = false;

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
				CloseScientificPopups ();
				CloseExtraSciencePopups ();
				ResumeGame ();
			} else if ((Input.GetKeyDown (KeyCode.JoystickButton5) || Input.GetKeyDown (KeyCode.P)) && tutListGame.Count > 1) {		// TO OPEN GAMEPLAY WHILE SCIENCE IS OPEN
				CloseScientificPopups ();					// close scientific tutorials
				CloseExtraSciencePopups ();
				ShowGameplayTutorials ();
			}
		}
		// ------ FOR OTHER POPUPS THAT NEVER APPEAR AGAIN AFTER READING THEM -------
		else if (oneTimePopupActive) {
			if (!paused) {
				PauseGame ();
				Debug.Log ("pause");
			}
			if (Input.GetKeyDown (KeyCode.JoystickButton5) || Input.GetKeyDown (KeyCode.P)) {				// TO CLOSE POPUPS WITH RB
				Debug.Log("moo");
				item.SetActive (false);
				//Destroy(item);
				ResumeGame ();
			}
		}

		// ------- OPENING TUTORIALS DURING GAMEPLAY -------
		else if (Input.GetKeyDown (KeyCode.JoystickButton4) || Input.GetKeyDown (KeyCode.O)) { 				// DURING GAMEPLAY: TO OPEN SCIENCE with LB
			ShowScientificTutorials ();
		} else if (Input.GetKeyDown (KeyCode.JoystickButton5) || Input.GetKeyDown (KeyCode.P)) {				// DURING GAMEPLAY: TO OPEN Game Tutorials with RB
			ShowGameplayTutorials ();
		}

		// ------------------ REVISITING TUTORIALS ------------------ //
		// ----- REVISITING THE GAMEPLAY TUTORIALS WITH RB ------
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

		// ----- REVISITING THE SCIENTIFIC TUTORIALS WITH LB ------
		if (revisitSc) {
			scienceTutActive = true;
			h = Input.GetAxis ("Horizontal");
			v = Input.GetAxis ("Vertical");
			if ((h >= 0.7f && h <= 1f) || Input.GetKeyDown (KeyCode.RightArrow)) {
				input = "right";
				gotInput = true;
				StartCoroutine ("WaitForQuiet");

			} else if ((h <= -0.7f && h >= -1) || Input.GetKeyDown (KeyCode.LeftArrow)) {
				input = "left";
				gotInput = true;
				StartCoroutine ("WaitForQuiet");
			} else if ((v <= -0.7f && v >= -1) || Input.GetKeyDown (KeyCode.DownArrow)) {
				input = "down";
				gotInput = true;
				StartCoroutine ("WaitForQuietV");
			} else if ((v >= 0.7f && v <= 1f) || Input.GetKeyDown(KeyCode.UpArrow)) {
				input = "up";
				gotInput = true;
				StartCoroutine ("WaitForQuietV");
			}

			if (input == "right" && gotInput && h == 0) {
				ShowNextSc ();
			} else if (input == "left" && gotInput && h == 0) {
				ShowPreviousSc ();	
			} else if (input == "down" && gotInput && v == 0) {
				ShowMoreScience ();
			} else if (input == "up" && gotInput && v == 0 && showingMoreScience) {
				HideMoreScience ();		
			}
		}

		// --------- FIRST VISIT OF EACH SCIENTIFIC TUTORIAL ----------
		if (firstVisitSc) {
			v = Input.GetAxis ("Vertical");
			if ((v <= -0.7f && v >= -1) || Input.GetKeyDown (KeyCode.DownArrow)) {
				input = "down";
				gotInput = true;
				StartCoroutine ("WaitForQuietV");
			} else if ((v >= 0.7f && v <= 1f) || Input.GetKeyDown(KeyCode.UpArrow)) {
				input = "up";
				gotInput = true;
				StartCoroutine ("WaitForQuietV");
			}


			if (input == "down" && gotInput && v == 0) {
				indexSc = tutListScience.Count - 1;
				item = tutListScience [indexSc] as GameObject;
				ShowMoreScience ();
		
			} else if (input == "up" && gotInput && v == 0 && showingMoreScience) {
				indexSc = tutListScience.Count - 1;
				item = tutListScienceExtra [indexSc] as GameObject;
				HideMoreScience ();	

			}
		}
	}

	void ShowMoreScience() {
		item.SetActive (false);
		item = tutListScienceExtra [indexSc] as GameObject;
		item.SetActive (true);
		showingMoreScience = true;
		gotInput = false;
		firstVisitSc = false;
	}

	void HideMoreScience() {
		item.SetActive (false);
		item = tutListScience [indexSc] as GameObject;
		item.SetActive (true);
		showingMoreScience = false;
		gotInput = false;
		firstVisitSc = false;
	}

	void ShowGameplayTutorials() {
		if (!gameTutActive) {
			if (tutListGame.Count > 0) {
				item = tutListGame [i] as GameObject;
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
		saturationTutGame.SetActive (true);
		gameTutActive = true;
		tutListGame.Add (saturationTutGame);
	}
	IEnumerator WaitForQuiet() {
		yield return new WaitUntil (() => (h == 0));
	}

	IEnumerator WaitForQuietV() {
		yield return new WaitUntil (() => (v == 0));
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

	public void CloseExtraSciencePopups() {
		foreach (GameObject extraSc in tutListScienceExtra) {
			extraSc.SetActive (false);
			revisitSc = false;
			indexSc = 0;
			scienceTutActive = false;
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
		oneTimePopupActive = false;
		paused = false;
	}


}
