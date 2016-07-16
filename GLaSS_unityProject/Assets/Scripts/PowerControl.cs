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

	//-------------------------------------------//

	void Start() {
		// if we are at level 1:
		GameManager.Instance.LockAllPowers ();
	}
		
	void OnTriggerEnter2D(Collider2D col) {
		if (col.CompareTag ("Player")) {
			PowerCheckpoints ();
		}
	}

	void PowerCheckpoints() {
		switch (power) {
		case powerIntro.gravity:
			GameManager.Instance.UnlockPower ("gravity");
			break;
		case powerIntro.brownian:
			GameManager.Instance.UnlockPower ("brownian");
			break;
		case powerIntro.inertia:
			GameManager.Instance.UnlockPower ("inertia");
			break;
		case powerIntro.VDW:
			GameManager.Instance.UnlockPower ("vdw");
			break;
		}
	}
		


}
