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

	public Button nextButton;


	//public GameObject[] gameTutArray;
	public ArrayList tutList = new ArrayList();
	private int i = 0;
	private GameObject item;
	public bool paused = false;
	public bool revisit = false;

	private float h;
	private string input;
	private bool gotInput;

	//-------------------------------------------//

	void Awake() {
		if (Instance == null)
			Instance = this;

		//UI = FindObjectOfType<Canvas> ();
		Game = FindObjectOfType<GameManager> ();




	}

	void Start() {
		CloseAllPopups ();
	}

	void Update() {
		if (gameTutActive) {																		// if a popup is open, pause the game
			if (!paused)
				PauseGame ();
			// TODO later: disable "pressing" powers while paused!
			if (Input.GetKeyDown (KeyCode.JoystickButton5) || Input.GetKeyDown (KeyCode.P)) { 		// TO CLOSE. if u press RB then close the popup and resume the game 
				revisit = false;
				i = 0;
				CloseAllPopups ();
				ResumeGame ();
				if (!tutList.Contains (saturationTutGame)) {					// if we haven't seen saturiation tutorial yet, show it
					saturationTutGame.SetActive (true);
					gameTutActive = true;
					tutList.Add (saturationTutGame);
				}
			}

		} else {																// TO OPEN. if u press RB then pause & bring up the popups
			if (Input.GetKeyDown (KeyCode.JoystickButton5) || Input.GetKeyDown (KeyCode.P)) { 		
				
				if (tutList.Count > 0) {
					item = tutList [i] as GameObject;
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
			if (h == 1f) {
				input = "right";
				gotInput = true;
				StartCoroutine ("WaitForQuiet");

			} else if (h == -1) {
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
			Debug.Log ("i: " + i);
			Debug.Log ("i is SMALL!");
			i = tutList.Count-1;
		}
		item.SetActive (false);
		item = tutList [i] as GameObject;
		item.SetActive (true);
		gotInput = false;
	}

	public void ShowTutorial() {
		gameTutActive = true;
		item = tutList [i] as GameObject;
		item.SetActive (true);
		//Debug.Log ("mpe");
	}

	void CloseAllPopups() {
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
