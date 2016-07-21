using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CreditSceneScript : MonoBehaviour
{

	void Start ()
    {
	
	}
	
	void Update ()
    {
	
	}

    public void ReturnToMain()
    {
        SceneManager.LoadScene(0);
        GetComponent<StartOptions>().sceneToStart = 2;
    }

    public void GoToCredits()
    {
        GetComponent<StartOptions>().sceneToStart = 1;
        GetComponent<StartOptions>().StartButtonClicked();
    }
}
