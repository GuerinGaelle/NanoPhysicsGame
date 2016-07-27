using UnityEngine;
using System.Collections;

public class taxiMusic : MonoBehaviour {

    private Joint2D joint;
    private AudioSource source;

    void Start()
    {
        joint = GetComponent<Joint2D>();
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (joint.isActiveAndEnabled && !source.isPlaying)
            source.Play();

        else if (!joint.isActiveAndEnabled && source.isPlaying)
            source.Stop();
    }
}
