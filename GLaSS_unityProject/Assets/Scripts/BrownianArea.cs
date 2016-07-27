using UnityEngine;
using System.Collections;

public class BrownianArea : MonoBehaviour {

    [Range(0f, 20f)]
    public float brownianIntensity;
    private AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.GetComponent<BrownianBehaviour>())
        {
            other.GetComponent<BrownianBehaviour>().currentBrownianIntensity = brownianIntensity;

            if (!source.isPlaying)
                source.Play();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<BrownianBehaviour>())
        {
            other.GetComponent<BrownianBehaviour>().currentBrownianIntensity = other.GetComponent<BrownianBehaviour>().baseBrownianIntensity;
            source.Stop();
        }
    }
}
