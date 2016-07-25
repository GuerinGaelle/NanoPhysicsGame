using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

    public AudioClip introMusic;
    public AudioClip loopingMusic;

    private AudioSource source;
    
    void Start ()
    {
        source = GetComponent<AudioSource>();
        source.clip = introMusic;
        source.Play();
	}

	void Update ()
    {
	    if(source.clip == introMusic && !source.isPlaying)
        {
            source.clip = loopingMusic;
            source.Play();
            source.loop = true;
        }
	}
}