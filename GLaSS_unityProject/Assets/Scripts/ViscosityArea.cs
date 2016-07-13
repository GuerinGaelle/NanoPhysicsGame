using UnityEngine;
using System.Collections;

public class ViscosityArea : MonoBehaviour {

    [Range(0f, 15f)]
    public float NewSpeed;

	void Start ()
    {
	    
	}

	void Update ()
    {
	    
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<CharacterBehaviour>().Speed = NewSpeed;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<CharacterBehaviour>().Speed = other.GetComponent<CharacterBehaviour>().BaseSpeed;
        }
    }
}
