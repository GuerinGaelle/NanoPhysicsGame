using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PowerControl : MonoBehaviour {

	public enum powerIntro {
		gravity,
		inertia,
		brownian,
		VDW,
		//manager
	}

	public powerIntro power;
	private float waitTimeUI = 6f;		// to delete if not used!

	/*// Gameplay Tutorial popups UI
	private GameObject gravityTutGame;
	private GameObject inertiaTutGame;
	private GameObject brownianTutGame;
	private GameObject vdwTutGame;

	// Scientific Tutorial popups UI: ++++ not done yet
	private GameObject gravityTutScience;
	private GameObject inertiaTutScience;
	private GameObject brownianTutScience;
	private GameObject vdwTutScience;*/

	//-------------------------------------------//




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
				UIManager.Instance.gravityTutGame.SetActive (true);
				UIManager.Instance.gameTutActive = true;
			}
			break;
		case powerIntro.brownian:
			if (!GameManager.Instance.isBrownianUnlocked) {
				GameManager.Instance.UnlockPower ("brownian");
				UIManager.Instance.brownianTutGame.SetActive (true);
				UIManager.Instance.gameTutActive = true;
			}
			break;
		case powerIntro.inertia:
			if (!GameManager.Instance.isInertiaUnlocked) {
				GameManager.Instance.UnlockPower ("inertia");
				UIManager.Instance.inertiaTutGame.SetActive (true);
				UIManager.Instance.gameTutActive = true;
			}
			break;
		case powerIntro.VDW:
			if (!GameManager.Instance.isVDWUnlocked) {
				GameManager.Instance.UnlockPower ("vdw");
				UIManager.Instance.vdwTutGame.SetActive (true);
				UIManager.Instance.gameTutActive = true;
			}
			break;
		default:
			break;
		}
	}
		
	IEnumerator WaitAndCloseUI() {
		yield return new WaitForSeconds (waitTimeUI);
		UIManager.Instance.gravityTutGame.SetActive (false);
	}
		

}
