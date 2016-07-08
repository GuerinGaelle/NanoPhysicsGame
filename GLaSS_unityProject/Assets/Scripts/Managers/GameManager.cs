using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    //-------------------------------------------------//
    //------------------- VARIABLES -------------------//
    //-------------------------------------------------//

    public static GameManager Instance;
    public CharacterBehaviour Player;

    public GameObject Canvas;

    //-------------------------------------------------//

    void Awake()
    {
        if(Instance == null)
            Instance = this;

        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBehaviour>();
        Canvas = GameObject.Find("Canvas");
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Player.IsStuck = true;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            Player.IsStuck = false;
        }
    }


    public void ToggleGravity()
    {
        Player.HasGravity = !Player.HasGravity;
    }

    public void ToggleInertia()
    {
        Player.HasInertia = !Player.HasInertia;
    }

    public void ToggleBrownianMovement()
    {
        if (Player.BrownianIntensity > 0)
            Player.BrownianIntensity = 0;
        else
            Player.BrownianIntensity = 0.1f; // TODO change from Magic number to static variable maybe ?
    }

    public void ToggleVanDerWaals()
    {
        // TODO toggle VDWaals function (to do when the behaviour is working well)
    }
}