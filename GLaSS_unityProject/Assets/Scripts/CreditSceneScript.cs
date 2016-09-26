using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CreditSceneScript : MonoBehaviour
{
	private StartOptions options;
	private static GameObject UIMenu;


	void Start ()
    {
		options = FindObjectOfType<StartOptions> ();
		//UIMenu = GameObject.FindGameObjectWithTag("UIMenu");
	}
	
	void Update ()
    {
        //if ((SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 2) && Input.GetKeyDown(KeyCode.Joystick1Button1))
		if ((SceneManager.GetActiveScene().name == "Credits") && (Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.Joystick1Button0)))
		{
           ReturnToMain();
        }
	}

    public void ReturnToMain()
    {
        SceneManager.LoadScene(0);
		options.sceneToStart = 2;
		options.inCreditsMenu = false;
		options.inMainMenu = true;
    }

    public void GoToCredits()
    {
        options.sceneToStart = 1;
        options.StartButtonClicked();
		options.inCreditsMenu = true;
    }
		
}