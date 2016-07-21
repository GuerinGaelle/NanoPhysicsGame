using UnityEngine;
using System.Collections;

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
            GameObject _bullet = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
            _bullet.GetComponent<Rigidbody2D>().velocity = GameManager.Instance.Player.transform.right * 30;
        }
	}
}
