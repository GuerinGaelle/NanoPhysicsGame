using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PowerControl : MonoBehaviour {

	public enum powerIntro {
		gravity,
		inertia,
		brownian,
		VDW,
		manager
	}

	public powerIntro power;
	private float waitTimeUI = 6f;		// to delete if not used!
	public bool gameTutActive = false;


	// Gameplay Tutorial popups UI
	private GameObject gravityTutGame;
	private GameObject inertiaTutGame;
	private GameObject brownianTutGame;
	private GameObject vdwTutGame;

	// Scientific Tutorial popups UI: ++++ not done yet
	private GameObject gravityTutScience;
	private GameObject inertiaTutScience;
	private GameObject brownianTutScience;
	private GameObject vdwTutScience;

	//-------------------------------------------//

	void Awake() {
		gravityTutGame = GameObject.Find ("Tutorial Gravity");
		inertiaTutGame = GameObject.Find ("Tutorial Inertia");
		brownianTutGame = GameObject.Find ("Tutorial Brownian");
		vdwTutGame = GameObject.Find ("Tutorial VDW");
	}

	void Start() {
		
		// if we are at level 1/tutorial:
	
		//GameManager.Instance.LockedAllPowers ();
		gravityTutGame.SetActive (false);
		inertiaTutGame.SetActive (false);
		brownianTutGame.SetActive (false);
		vdwTutGame.SetActive (false);
	}
		
	void Update() {
		if (gameTutActive) {
			Time.timeScale = 0f;
			if (Input.GetKeyDown(KeyCode.Joystick8Button5) || Input.GetKeyDown(KeyCode.P)) {
				gravityTutGame.SetActive (false);
				inertiaTutGame.SetActive (false);
				brownianTutGame.SetActive (false);
				vdwTutGame.SetActive (false);
				Time.timeScale = 1f;
				gameTutActive = false;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.CompareTag ("Player")) {
			PowerCheckpoints ();
		}
	}

	void PowerCheckpoints() {
		switch (power) {
		case powerIntro.gravity:
			if (!GameManager.Instance.isGravityUnlocked) {
				GameManager.Instance.UnlockPower ("gravity");
				gravityTutGame.SetActive (true);
				gameTutActive = true;
				//StartCoroutine ("WaitAndCloseUI");
			}
			break;
		case powerIntro.brownian:
			if (!GameManager.Instance.isBrownianUnlocked) {
				GameManager.Instance.UnlockPower ("brownian");
				brownianTutGame.SetActive (true);
				gameTutActive = true;
			}
			break;
		case powerIntro.inertia:
			if (!GameManager.Instance.isInertiaUnlocked) {
				GameManager.Instance.UnlockPower ("inertia");
				inertiaTutGame.SetActive (true);
				gameTutActive = true;
			}
			break;
		case powerIntro.VDW:
			if (!GameManager.Instance.isVDWUnlocked) {
				GameManager.Instance.UnlockPower ("vdw");
				vdwTutGame.SetActive (true);
				gameTutActive = true;
			}
			break;
		default:
			break;
		}
	}
		
	IEnumerator WaitAndCloseUI() {
		yield return new WaitForSeconds (waitTimeUI);
		gravityTutGame.SetActive (false);
	}
		

}
