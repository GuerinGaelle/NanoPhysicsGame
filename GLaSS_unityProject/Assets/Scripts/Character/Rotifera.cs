using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Rotifera : MonoBehaviour {

    public Transform endPoint;

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.transform.tag == "Particule")
        {
            Destroy(other.gameObject);
            transform.DOMove(endPoint.position, 2);
        }
    }
}