using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class RedMetricsCustom : MonoBehaviour
{
    void Start ()
    {
        CustomData customData = new CustomData();
        customData.Add("Level_Name", Application.loadedLevelName);

        RedMetricsManager.get().sendEvent(TrackingEvent.START, customData);
    }
	

	void Update ()
    {
	
	}
}