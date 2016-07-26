using UnityEngine;
using System.Collections;

using DG.Tweening;

public class CameraBehaviour : MonoBehaviour {

    private float CloseSmooth = 2f;
    private float NormalSmooth = 0.2f;

    private Transform target;
    private CharacterBehaviour player;

    private Rigidbody2D playerRigid;

	// Use this for initialization
	void Start ()
    {
        player = GameManager.Instance.Player;
        target = player.transform.FindChild("TargetCam");
        playerRigid = target.GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (target == null)
            return;

        //Vector2 velocityAddedForCam = player.actualWantedVelocity * 2;

        Vector3 _newPos = new Vector3(target.transform.position.x /*+ velocityAddedForCam.x*/, target.transform.position.y /*+ velocityAddedForCam.y */, -10);
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