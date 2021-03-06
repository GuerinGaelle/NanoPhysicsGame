﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PowerControl : MonoBehaviour {

	public enum powerIntro {
		gravity,
		inertia,
		brownian,
		VDW,
		VDWScience,
		gravityScience,
		inertiaScience,
		viscocityScience,
		brownianScience,
		checkpoint,
		rotifera,
		particles
	}

	public powerIntro power;
	private UIManager UI;
	private GameManager Game;

	//-------------------------------------------//

	void Awake() {
		UI = FindObjectOfType<UIManager> ();
		Game = FindObjectOfType<GameManager> ();
	}


	void OnTriggerEnter2D(Collider2D col) {
		if (col.CompareTag ("Player")) {
			PowerCheckpoints ();

            AudioClip sound = Resources.Load<AudioClip>("Music/son/Power finding");
            GameManager.Instance.GetComponent<AudioSource>().PlayOneShot(sound);
            Destroy(this.gameObject);
		}
	}

	void PowerCheckpoints() {
		switch (power) {
		case powerIntro.gravity:
			if (!Game.isGravityUnlocked) {				// if gravity is not unlocked yet, unlock it and show the tutorial
				Game.UnlockPower ("gravity");
				UI.gravityTutGame.SetActive (true);
				UI.gameTutActive = true;
				UI.tutListGame.Add (UI.gravityTutGame);
			}
			break;
		case powerIntro.brownian:
			if (!Game.isBrownianUnlocked) {
				Game.UnlockPower ("brownian");
				UI.brownianTutGame.SetActive (true);
				UI.gameTutActive = true;
				UI.tutListGame.Add (UI.brownianTutGame);
			}
			break;
		case powerIntro.inertia:
			if (!Game.isInertiaUnlocked) {
				Game.UnlockPower ("inertia");
				UI.inertiaTutGame.SetActive (true);
				UI.gameTutActive = true;
				UI.tutListGame.Add (UI.inertiaTutGame);
			}
			break;
		case powerIntro.VDW:
			if (!Game.isVDWUnlocked) {
				Game.UnlockPower ("vdw");
				UI.vdwTutGame.SetActive (true);
				UI.gameTutActive = true;
				UI.tutListGame.Add (UI.vdwTutGame);
			}
			break;

			// SCIENTIFIC TUTORIALS:
		case powerIntro.VDWScience:
			if (!Game.visitedVDWSc) {
				Game.visitedVDWSc = true;
				UI.vdwTutScience.SetActive (true);
				UI.tutListScience.Add (UI.vdwTutScience);
				UI.tutListScienceExtra.Add (UI.extraVDW);
				UI.scienceTutActive = true;
				UI.firstVisitSc = true;
			}
			break;

		case powerIntro.gravityScience:
			if (!Game.visitedGravitySc) {
				Game.visitedGravitySc = true;
				UI.gravityTutScience.SetActive (true);
				UI.tutListScience.Add (UI.gravityTutScience);
				UI.tutListScienceExtra.Add (UI.extraGravity);
				UI.scienceTutActive = true;
				UI.firstVisitSc = true;
			}
			break;

		case powerIntro.inertiaScience:
			if (!Game.visitedInertiaSc) {
				Game.visitedInertiaSc = true;
				UI.inertiaTutScience.SetActive (true);
				UI.tutListScience.Add (UI.inertiaTutScience);
				UI.tutListScienceExtra.Add (UI.extraInertia);
				UI.scienceTutActive = true;
				UI.firstVisitSc = true;
			}
			break;

		case powerIntro.viscocityScience:
			if (!Game.visitedViscositySc) {
				Game.visitedViscositySc = true;
				UI.viscosityTutScience.SetActive (true);
				UI.tutListScience.Add (UI.viscosityTutScience);
				UI.tutListScienceExtra.Add (UI.extraViscosity);
				UI.scienceTutActive = true;
				UI.firstVisitSc = true;
			}
			break;
		case powerIntro.brownianScience:
			if (!Game.visitedBrownianSc) {
				Game.visitedBrownianSc = true;
				UI.brownianTutScience.SetActive (true);
				UI.tutListScience.Add (UI.brownianTutScience);
				UI.tutListScienceExtra.Add (UI.extraBrownian);
				UI.scienceTutActive = true;
				UI.firstVisitSc = true;
			}
			break;
		case powerIntro.checkpoint: 
			if (!Game.visitedCheckpointPop) {
				Game.visitedCheckpointPop = true;
				UI.item = UI.checkpoint;
				UI.item.SetActive (true);
				UI.oneTimePopupActive = true;
			}
			break;
		case powerIntro.rotifera:
			if (!Game.visitedRotiferaPop) {
				Game.visitedRotiferaPop = true;
				UI.item = UI.rotifera;
				UI.item.SetActive (true);
				UI.oneTimePopupActive = true;
			}
			break;
		case powerIntro.particles:
			if (!Game.visitedParticlesPop) {
				Game.visitedParticlesPop = true;
				UI.item = UI.particles;
				UI.item.SetActive (true);
				UI.oneTimePopupActive = true;
			}
			break;
		default:
			break;
		}
	}
}
