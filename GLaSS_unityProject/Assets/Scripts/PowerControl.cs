using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PowerControl : MonoBehaviour {

	public enum powerIntro {
		gravity,
		inertia,
		brownian,
		VDW
	}

	public powerIntro power;
	public float waitTimeUI = 6f;
	public bool tutorialActive = false;

	private GameObject gravityTutorial;
	private GameObject inertiaTutorial;
	private GameObject brownianTutorial;
	private GameObject vdwTutorial;
	//-------------------------------------------//

	void Awake() {
		gravityTutorial = GameObject.Find ("Tutorial Gravity");
		inertiaTutorial = GameObject.Find ("Tutorial Inertia");
		brownianTutorial = GameObject.Find ("Tutorial Brownian");
		vdwTutorial = GameObject.Find ("Tutorial VDW");
	}

	void Start() {
		
		// if we are at level 1/tutorial:
	
		GameManager.Instance.LockAllPowers ();
		gravityTutorial.SetActive (false);
		inertiaTutorial.SetActive (false);
		brownianTutorial.SetActive (false);
		vdwTutorial.SetActive (false);
	}
		
	void Update() {
		if (tutorialActive) {
			Time.timeScale = 0f;
			if (Input.GetKeyDown(KeyCode.Joystick8Button5) || Input.GetKeyDown(KeyCode.P)) {
				gravityTutorial.SetActive (false);
				inertiaTutorial.SetActive (false);
				brownianTutorial.SetActive (false);
				vdwTutorial.SetActive (false);
				Time.timeScale = 1f;
				tutorialActive = false;
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
				gravityTutorial.SetActive (true);
				tutorialActive = true;
				//StartCoroutine ("WaitAndCloseUI");
			}
			break;
		case powerIntro.brownian:
			if (!GameManager.Instance.isBrownianUnlocked) {
				GameManager.Instance.UnlockPower ("brownian");
				brownianTutorial.SetActive (true);
				tutorialActive = true;
			}
			break;
		case powerIntro.inertia:
			if (!GameManager.Instance.isInertiaUnlocked) {
				GameManager.Instance.UnlockPower ("inertia");
				inertiaTutorial.SetActive (true);
				tutorialActive = true;
			}
			break;
		case powerIntro.VDW:
			if (!GameManager.Instance.isVDWUnlocked) {
				GameManager.Instance.UnlockPower ("vdw");
				vdwTutorial.SetActive (true);
				tutorialActive = true;
			}
			break;
		}
	}
		
	IEnumerator WaitAndCloseUI() {
		yield return new WaitForSeconds (waitTimeUI);
		gravityTutorial.SetActive (false);
	}
		

}
