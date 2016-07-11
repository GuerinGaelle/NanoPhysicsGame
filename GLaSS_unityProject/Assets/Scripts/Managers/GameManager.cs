﻿using UnityEngine;
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
        if (Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            ToggleGravity();
        }
        else if(Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            ToggleInertia();
        }
        else if (Input.GetKeyDown(KeyCode.Joystick1Button2))
        {
            ToggleBrownianMovement();
        }
        else if (Input.GetKeyDown(KeyCode.Joystick1Button3))
        {
            ToggleVanDerWaals();
        }
    }

    public void ToggleGravity()
    {
        Player.HasGravity = !Player.HasGravity;
        ColoriseButton(Player.HasGravity, "Gravity_Button");

        if (Player.IsStuck)
        {
            Player.IsStuck = false;
        }
            
    }

    public void ToggleInertia()
    {
        Player.HasInertia = !Player.HasInertia;
        ColoriseButton(Player.HasInertia, "Inertia_Button");
    }

    public void ToggleBrownianMovement()
    {
        if (Player.BrownianIntensity > 0)
        {
            Player.BrownianIntensity = 0;
            ColoriseButton(true, "Brownian_Button");
        }
        else
        {
            Player.BrownianIntensity = 0.1f; // TODO change from Magic number to static variable maybe ?
            ColoriseButton(false, "Brownian_Button");
        }          
    }

    public void ToggleVanDerWaals()
    {
        if (Player.CanFeelVDW)
        {
            Player.IsStuck = false;
            Player.CanFeelVDW = false;

            foreach (VDWBehaviour vdwObj in GameObject.FindObjectsOfType<VDWBehaviour>())
            {
                vdwObj.StopJoint();
            }
            ColoriseButton(true, "VDW_Button");
        }
        else
        {
            Player.CanFeelVDW = true;
            ColoriseButton(false, "VDW_Button");
        }
    }

    private void ColoriseButton(bool boolean, string button)
    {
        // COLORS of the Button
        ColorBlock newColorBlock = new ColorBlock();
        newColorBlock = ColorBlock.defaultColorBlock;

        if (boolean)
        {
            newColorBlock.normalColor = Color.grey;
            newColorBlock.highlightedColor = Color.grey;
        }
        GameObject.Find(button).GetComponent<Button>().colors = newColorBlock;
    }
}