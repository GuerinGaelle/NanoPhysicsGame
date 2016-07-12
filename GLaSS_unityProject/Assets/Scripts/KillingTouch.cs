using UnityEngine;
using System.Collections;

public class KillingTouch : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D co)
    {
        if (co.gameObject.tag == "Player")
        {
            GameManager.Instance.TouchedEnemy();
        }
    }
}
