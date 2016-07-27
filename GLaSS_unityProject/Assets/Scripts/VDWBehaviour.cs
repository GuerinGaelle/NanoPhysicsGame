using UnityEngine;
using System.Collections;

public enum VDW_Type
{
    StuckToPlayer,
    StuckToObject_moving,
    StuckToObject_static,
	KillingWall
}

[RequireComponent(typeof(DistanceJoint2D))]
public class VDWBehaviour : MonoBehaviour {

    private GameObject feedbackVDW;

    private DistanceJoint2D joint;
    public VDW_Type typeOfVDW;

    void Start()
    {
        joint = this.GetComponent<DistanceJoint2D>();
        StopJoint();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.GetComponent<CharacterBehaviour>() && other.GetComponent<CharacterBehaviour>().CanFeelVDW)
        {
            MoveToSurface(other.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<CharacterBehaviour>() && other.gameObject.GetComponent<CharacterBehaviour>().CanFeelVDW)
        {
            Stuck(other.gameObject);
        }
    }

    void MoveToSurface(GameObject obj)
    {
        Vector2 dir = transform.position - obj.transform.position;
        obj.GetComponent<Rigidbody2D>().AddForce(dir * 10000 * Time.fixedDeltaTime);
    }

    void Stuck(GameObject obj)
    {
        if (obj.GetComponent<CharacterBehaviour>().HasInertia)
            GameManager.Instance.ToggleInertia(false);

        switch (typeOfVDW)
        {
            case VDW_Type.StuckToPlayer:                
                joint.enabled = true;
                joint.connectedBody = obj.gameObject.GetComponent<Rigidbody2D>();
                this.transform.gameObject.layer = LayerMask.NameToLayer("Player");
                break;
            case VDW_Type.StuckToObject_moving:
                obj.GetComponent<CharacterBehaviour>().IsStuck = true;
                joint.enabled = true;
                joint.connectedBody = obj.gameObject.GetComponent<Rigidbody2D>();
                break;
            case VDW_Type.StuckToObject_static:
                obj.GetComponent<CharacterBehaviour>().IsStuck = true;
                break;
			case VDW_Type.KillingWall:
				GameManager.Instance.Death();
				break;
            default:
                break;
        }
    }

    public void StopJoint()
    {
        if(joint != null)
            joint.enabled = false;

        this.transform.gameObject.layer = LayerMask.NameToLayer("Default");

    }
}