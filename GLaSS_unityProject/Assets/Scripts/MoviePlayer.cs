using UnityEngine;
using System.Collections;

public class MoviePlayer : MonoBehaviour {

	void Start ()
    {
        ((MovieTexture)GetComponent<Renderer>().material.mainTexture).loop = true;
        ((MovieTexture)GetComponent<Renderer>().material.mainTexture).Play();
    }
	
	void Update ()
    {

	}
}
