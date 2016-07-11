using UnityEngine;
using System.Collections;

public class VDWBehaviour : MonoBehaviour {

    public bool StuckToMe = true; // true = player is child. False = player is parent
    public bool AmIStatic = true; // Walls = true, big animals + small particules = false

    void Awake()
    {

    }

	void Start ()
    {
	
	}
	
	void Update ()
    {
	
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.GetComponent<Rigidbody2D>() && other.GetComponent<CharacterBehaviour>().CanFeelVDW)
        {
            MoveToSurface(other.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.GetComponent<Rigidbody2D>() && other.gameObject.GetComponent<CharacterBehaviour>().CanFeelVDW)
        {
            Stuck(other.gameObject);
        }
    }

    void MoveToSurface(GameObject obj)
    {
        Vector2 dir = transform.position - obj.transform.position;
        obj.GetComponent<Rigidbody2D>().AddForce(dir * 100 * Time.fixedDeltaTime);
    }

    void Stuck(GameObject obj)
    {
        // TODO add an in-game feedback to better know when we're stuck

        if (StuckToMe)
        {
            obj.GetComponent<CharacterBehaviour>().IsStuck = true;
            if (!AmIStatic)
            {
                obj.GetComponent<DistanceJoint2D>().enabled = true;
                obj.GetComponent<DistanceJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();

                //obj.transform.SetParent(this.transform, true);
            }           
        }
        else
        {
            //this.transform.SetParent(obj.transform, true);
            //this.GetComponent<Rigidbody2D>().isKinematic = true;
            obj.GetComponent<DistanceJoint2D>().enabled = true;
            obj.GetComponent<DistanceJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();
        }
    }
}