using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CrushingWalls : MonoBehaviour {

    public bool isCrushing;
    public float speed;
    public GameObject wall_1, wall_2;
    public Transform wall_1_endPoint, wall_2_endPoint;

    private Vector3 basePos_1, basePos_2;
    private float time = 0;

	void Start ()
    {
        basePos_1 = wall_1.transform.position;
        basePos_2 = wall_2.transform.position;

        Move();
    }

    void Update()
    {
        time += Time.deltaTime;

        if ((time >= 0 && time < 0.2f) || time > (speed * 2) - 0.2f)
        {
            isCrushing = true;
        }
        else
            isCrushing = false;
    }

    void Move()
    {       
        wall_1.GetComponent<Rigidbody2D>().DOMove(wall_1_endPoint.position, speed).OnComplete(ReturnToBasePos);
        wall_2.GetComponent<Rigidbody2D>().DOMove(wall_2_endPoint.position, speed);
    }

    void ReturnToBasePos()
    {
        time = 0;
        wall_1.GetComponent<Rigidbody2D>().DOMove(basePos_1, speed).OnComplete(Move);
        wall_2.GetComponent<Rigidbody2D>().DOMove(basePos_2, speed);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (isCrushing && other.transform.tag == "Player")
        {
            GameManager.Instance.TouchedEnemy();
        }
    }
}