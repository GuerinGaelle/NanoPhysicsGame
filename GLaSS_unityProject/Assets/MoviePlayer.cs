using UnityEngine;
using System.Collections;

public class MoviePlayer : MonoBehaviour {

	void Start ()
    {
        ((MovieTexture)GetComponent<Renderer>().material.mainTexture).Play();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!((MovieTexture)GetComponent<Renderer>().material.mainTexture).isPlaying)
        {
            ((MovieTexture)GetComponent<Renderer>().material.mainTexture).Play();
        }
	}
}
