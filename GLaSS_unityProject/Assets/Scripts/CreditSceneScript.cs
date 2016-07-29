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
        if (SceneManager.GetActiveScene().buildIndex == 1 && Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            ReturnToMain();
        }
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