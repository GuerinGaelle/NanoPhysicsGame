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

            if (other.tag == "Player" && !source.isPlaying)
                source.Play();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<BrownianBehaviour>())
        {
            other.GetComponent<BrownianBehaviour>().currentBrownianIntensity = other.GetComponent<BrownianBehaviour>().baseBrownianIntensity;

            if (other.tag == "Player")
                source.Stop();
        }
    }
}