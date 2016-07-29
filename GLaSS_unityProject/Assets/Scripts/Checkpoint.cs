using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

	public Vector2 energyWellPos;

	void Awake() {
		energyWellPos = transform.GetChild(0).position;
	}
	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.name == "Player") {
            // We do not do anything if we are already in this checkpoint
            if (GameManager.Checkpoint == energyWellPos)
                return;

			GameManager.Checkpoint = energyWellPos;

            // AUDIO feedback
            AudioClip sound = Resources.Load<AudioClip>("Music/son/Checkpoint 2");
            GameManager.Instance.GetComponent<AudioSource>().PlayOneShot(sound);
            // VISUAL feedback
            GameObject feedback = Resources.Load<GameObject>("Prefabs/Signes_feedbacks/SaveFeedback");
            GameObject _feedBackInstantiated =  Instantiate(feedback, new Vector3(Camera.current.transform.position.x, Camera.current.transform.position.y, 1), Quaternion.identity) as GameObject;
            _feedBackInstantiated.transform.SetParent(Camera.current.transform);

        }
	}
}
