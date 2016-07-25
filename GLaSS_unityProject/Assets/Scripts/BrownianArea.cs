using UnityEngine;
using System.Collections;

public class BrownianArea : MonoBehaviour {

    [Range(0f, 20f)]
    public float brownianIntensity;

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.GetComponent<BrownianBehaviour>())
        {
            other.GetComponent<BrownianBehaviour>().currentBrownianIntensity = brownianIntensity;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<BrownianBehaviour>())
        {
            other.GetComponent<BrownianBehaviour>().currentBrownianIntensity = other.GetComponent<BrownianBehaviour>().baseBrownianIntensity;
        }
    }
}
