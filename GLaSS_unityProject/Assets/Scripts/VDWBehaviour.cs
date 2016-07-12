using UnityEngine;
using System.Collections;

public enum VDW_Type
{
    StuckToPlayer,
    StuckToObject_moving,
    StuckToObject_static
}

[RequireComponent(typeof(DistanceJoint2D))]
public class VDWBehaviour : MonoBehaviour {

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
        obj.GetComponent<Rigidbody2D>().AddForce(dir * 5000 * Time.fixedDeltaTime);
    }

    void Stuck(GameObject obj)
    {
        if (obj.GetComponent<CharacterBehaviour>().HasInertia)
            GameManager.Instance.ToggleInertia();

        switch (typeOfVDW)
        {
            case VDW_Type.StuckToPlayer:                
                joint.enabled = true;
                joint.connectedBody = obj.gameObject.GetComponent<Rigidbody2D>();
                break;
            case VDW_Type.StuckToObject_moving:
                obj.GetComponent<CharacterBehaviour>().IsStuck = true;
                joint.enabled = true;
                joint.connectedBody = obj.gameObject.GetComponent<Rigidbody2D>();
                break;
            case VDW_Type.StuckToObject_static:
                obj.GetComponent<CharacterBehaviour>().IsStuck = true;
                break;
            default:
                break;
        }
    }

    public void StopJoint()
    {
        if(joint != null)
            joint.enabled = false;
    }
}