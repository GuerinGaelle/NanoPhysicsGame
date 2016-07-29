using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

	public Vector2 energyWellPos;

	void Awake() {
		energyWellPos = transform.GetChild(0).position;
	}
	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.name == "Player") {
			GameManager.Checkpoint = energyWellPos;

            AudioClip sound = Resources.Load<AudioClip>("Music/son/Checkpoint 2");
            GameManager.Instance.GetComponent<AudioSource>().PlayOneShot(sound);
        }
	}
}
