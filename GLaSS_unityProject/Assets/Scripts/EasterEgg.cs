using UnityEngine;
using System.Collections;
using DG.Tweening;

public class EasterEgg : MonoBehaviour {

    private GameObject bullet;

	void Start ()
    {
        bullet = Resources.Load<GameObject>("Prefabs/other/Bullet");
    }
	

	void Update ()
    {
	    if(GameManager.Instance.Player.HasInertia && Input.GetAxis("RT") > 0.8f)
        {
            Camera.main.DOOrthoSize(15, 1);

            GameObject _bullet = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
            _bullet.GetComponent<Rigidbody2D>().velocity = GameManager.Instance.Player.transform.right * 30;
        }
        else
        {
            Camera.main.DOOrthoSize(5, 1);
        }
	}
}
