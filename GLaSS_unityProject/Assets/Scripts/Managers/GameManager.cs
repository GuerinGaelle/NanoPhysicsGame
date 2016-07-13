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

    [HideInInspector]
    public GameObject feedbackVDW;
    //-------------------------------------------------//

    void Awake()
    {
        if(Instance == null)
            Instance = this;

        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBehaviour>();
        Canvas = GameObject.Find("Canvas");

        feedbackVDW = Resources.Load<GameObject>("Prefabs/Signes_Feedbacks/VDWFeedback");
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

        // We stop the others
        if (Player.HasGravity)
        {
            if (Player.HasInertia)
                ToggleInertia();
            if (!Player.CanFeelBrownian)
                ToggleBrownianMovement();
            if (!Player.CanFeelVDW)
                ToggleVanDerWaals();
        }
        

        if (Player.IsStuck)
        {
            Player.IsStuck = false;
        }
            
    }

    public void ToggleInertia()
    {
        Player.HasInertia = !Player.HasInertia;
        ColoriseButton(Player.HasInertia, "Inertia_Button");

        // We stop the others
        if (Player.HasInertia)
        {
            if (Player.HasGravity)
                ToggleGravity();
            if (!Player.CanFeelBrownian)
                ToggleBrownianMovement();
            if (!Player.CanFeelVDW)
                ToggleVanDerWaals();
        }
    }

    public void ToggleBrownianMovement()
    {
        Player.CanFeelBrownian = !Player.CanFeelBrownian;
        ColoriseButton(!Player.CanFeelBrownian, "Brownian_Button");

        // We stop the others
        if (!Player.CanFeelBrownian)
        {
            if (Player.HasGravity)
                ToggleGravity();
            if (Player.HasInertia)
                ToggleInertia();
            if (!Player.CanFeelVDW)
                ToggleVanDerWaals();
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

        // We stop the others
        if (!Player.CanFeelVDW)
        {
            if (Player.HasGravity)
                ToggleGravity();
            if (Player.HasInertia)
                ToggleInertia();
            if (!Player.CanFeelBrownian)
                ToggleBrownianMovement();
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

    public void TouchedEnemy()
    {
        if (Player.animator.GetBool("isAlive"))
        {
            Player.IsStuck = true;
            Player.animator.SetBool("isAlive", false);
        }  
    }
}