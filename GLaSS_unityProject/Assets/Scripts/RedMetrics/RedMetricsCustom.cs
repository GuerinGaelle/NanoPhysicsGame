using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class RedMetricsCustom : MonoBehaviour
{
    // Use this for initialization
    void Start ()
    {
        RedMetricsManager.get().sendEvent(TrackingEvent.START);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
