using UnityEngine;
using System.Collections;

public class BrownianArea : MonoBehaviour {

    [Range(0f, 10f)]
    public float BrownianIntensity;

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
            other.GetComponent<CharacterBehaviour>().BrownianIntensity = BrownianIntensity;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            // TODO : Change magic number to base BrownianIntensity.
            other.GetComponent<CharacterBehaviour>().BrownianIntensity = 0.1f;
        }
    }
}
