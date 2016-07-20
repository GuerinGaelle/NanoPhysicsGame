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
	private UIManager UI;
	private GameManager Game;
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

	void Awake() {
		UI = FindObjectOfType<UIManager> ();
		Game = FindObjectOfType<GameManager> ();
	}


	void OnTriggerEnter2D(Collider2D col) {
		if (col.CompareTag ("Player")) {
			PowerCheckpoints ();
		}
	}

	void PowerCheckpoints() {
		switch (power) {
		case powerIntro.gravity:
			if (!Game.isGravityUnlocked) {
				Game.UnlockPower ("gravity");
				UI.gravityTutGame.SetActive (true);
				UI.gameTutActive = true;
				UI.tutList.Add (UI.gravityTutGame);
			}
			break;
		case powerIntro.brownian:
			if (!Game.isBrownianUnlocked) {
				Game.UnlockPower ("brownian");
				UI.brownianTutGame.SetActive (true);
				UI.gameTutActive = true;
				UI.tutList.Add (UI.brownianTutGame);
			}
			break;
		case powerIntro.inertia:
			if (!Game.isInertiaUnlocked) {
				Game.UnlockPower ("inertia");
				UI.inertiaTutGame.SetActive (true);
				UI.gameTutActive = true;
				UI.tutList.Add (UI.inertiaTutGame);
			}
			break;
		case powerIntro.VDW:
			if (!Game.isVDWUnlocked) {
				Game.UnlockPower ("vdw");
				UI.vdwTutGame.SetActive (true);
				UI.gameTutActive = true;
				UI.tutList.Add (UI.vdwTutGame);
			}
			break;
		default:
			break;
		}
	}
		
	IEnumerator WaitAndCloseUI() {
		//yield return new WaitForSeconds (waitTimeUI);
		yield return new WaitForEndOfFrame();
		UI.saturationTutGame.SetActive (true);

		//UI.gravityTutGame.SetActive (false);
	}
		

}
