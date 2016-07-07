using UnityEngine;
using System.Collections;

using DG.Tweening;

public class CameraBehaviour : MonoBehaviour {

    public float Smooth = 0.15f;
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

        if(Vector2.Distance(_newPos, transform.position) > 0.2f) // We're not moving for small Brownian effects
        {
            transform.DOMove(_newPos, Smooth).SetEase(Ease.InOutSine); // using the tweening engine
        }
    }
}