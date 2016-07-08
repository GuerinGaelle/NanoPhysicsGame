using UnityEngine;
using System.Collections;

using DG.Tweening;

public class CameraBehaviour : MonoBehaviour {

    public float CloseSmooth = 2f;
    public float NormalSmooth = 0.2f;

    private GameObject player;

	// Use this for initialization
	void Start ()
    {
        player = GameManager.Instance.Player.gameObject;
    }
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 _newPos = new Vector3(player.transform.position.x, player.transform.position.y, -10);
        float _dist = Vector2.Distance(_newPos, transform.position);

        if (_dist < 0.2f)
        {
            transform.DOMove(_newPos, CloseSmooth).SetEase(Ease.InOutSine);
        }
        else
        {
            transform.DOMove(_newPos, NormalSmooth).SetEase(Ease.InOutSine);
        }
    }
}