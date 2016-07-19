using UnityEngine;
using System.Collections;

using DG.Tweening;

public class CameraBehaviour : MonoBehaviour {

    public float CloseSmooth = 2f;
    public float NormalSmooth = 0.2f;

    private GameObject player;
    private Rigidbody2D playerRigid;

	// Use this for initialization
	void Start ()
    {
        player = GameManager.Instance.Player.gameObject;
        playerRigid = player.GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (player == null)
            return;

        Vector2 velocityAddedForCam = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * 3.14f;

        Vector3 _newPos = new Vector3(player.transform.position.x + velocityAddedForCam.x, player.transform.position.y + velocityAddedForCam.y, -10);
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