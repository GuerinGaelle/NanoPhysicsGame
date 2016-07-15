using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    //-------------------------------------------------//
    //------------------- VARIABLES -------------------//
    //-------------------------------------------------//

    public static GameManager Instance;
    public CharacterBehaviour Player;

    public GameObject Canvas;
	public float increaseSpeed = 100f;
	public float decreaseSpeed = 50f;

    [HideInInspector]
    public GameObject feedbackVDW;
	public static Vector2 Checkpoint;
	public Slider saturationBar;

    //-------------------------------------------------//

    /*private float saturation = 0;
    public float Saturation
    {
        get
        {
            return saturation;
        }
        set
        {
            saturation = value;
			saturationBar.value = value;
        }
    }*/

    //-------------------------------------------------//
    void Awake()
    {
        if(Instance == null)
            Instance = this;

        Player = FindObjectOfType<CharacterBehaviour>();
        Canvas = GameObject.Find("Canvas");

        feedbackVDW = Resources.Load<GameObject>("Prefabs/Signes_Feedbacks/VDWFeedback");
		saturationBar = Canvas.transform.FindChild("EnergyBar").GetComponent<Slider>();
    }
	
	// Update is called once per frame
	void Update ()
	{	// Powers get locked if the saturation bar reaches maximum
		if (Player.lockedPowers) {
			// TODO : Colorise gray so they look disactivated
			Player.HasGravity = false; 
			ColoriseButton (Player.HasGravity, "Gravity_Button");
			Player.HasInertia = false;
			ColoriseButton (Player.HasInertia, "Inertia_Button");
			Player.brownianBehaviour.canFeelBrownian = true;
			ColoriseButton (!Player.brownianBehaviour.canFeelBrownian, "Brownian_Button");
			Player.CanFeelVDW = true;
			ColoriseButton (!Player.CanFeelVDW, "VDW_Button");
		}
        else
        {
			if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.G)
                || Input.GetKeyUp(KeyCode.Joystick1Button0) || Input.GetKeyUp(KeyCode.G))
			{
				ToggleGravity();
			}
			else if(Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.H)
                || Input.GetKeyUp(KeyCode.Joystick1Button1) || Input.GetKeyUp(KeyCode.H))
			{
				ToggleInertia();
			}
			else if (Input.GetKeyDown(KeyCode.Joystick1Button2) || Input.GetKeyDown(KeyCode.F)
                || Input.GetKeyUp(KeyCode.Joystick1Button2) || Input.GetKeyUp(KeyCode.F))
			{
				ToggleBrownianMovement();
			}
			else if (Input.GetKeyDown(KeyCode.Joystick1Button3) || Input.GetKeyDown(KeyCode.T)
                || Input.GetKeyUp(KeyCode.Joystick1Button3) || Input.GetKeyUp(KeyCode.T))
			{
				ToggleVanDerWaals();
			}
		}

		if (Input.GetKeyDown(KeyCode.R)) // TODO remove it for GOLD version
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void ToggleGravity()
    {
		Player.HasGravity = !Player.HasGravity;
		ColoriseButton (Player.HasGravity, "Gravity_Button");

		// We stop the others
		if (Player.HasGravity) {
			if (Player.HasInertia)
				ToggleInertia ();
			if (!Player.brownianBehaviour.canFeelBrownian)
				ToggleBrownianMovement ();
			if (!Player.CanFeelVDW)
				ToggleVanDerWaals ();
		}
		if (Player.IsStuck) {
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
            if (!Player.brownianBehaviour.canFeelBrownian)
                ToggleBrownianMovement();
            if (!Player.CanFeelVDW)
                ToggleVanDerWaals();
        }
    }
		
    public void ToggleBrownianMovement()
	{
		Player.brownianBehaviour.canFeelBrownian = !Player.brownianBehaviour.canFeelBrownian;
		ColoriseButton (!Player.brownianBehaviour.canFeelBrownian, "Brownian_Button");

		// We stop the others
		if (!Player.brownianBehaviour.canFeelBrownian) {
			if (Player.HasGravity)
				ToggleGravity ();
			if (Player.HasInertia)
				ToggleInertia ();
			if (!Player.CanFeelVDW)
				ToggleVanDerWaals ();
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
            if (!Player.brownianBehaviour.canFeelBrownian)
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
            Invoke("DestroyPlayer", 0.5f);
        }  
    }

    void DestroyPlayer()
    {  
        Instantiate(feedbackVDW, Player.transform.position, Quaternion.identity);
        //Destroy(Player.gameObject);
		Player.gameObject.transform.position = Checkpoint;
		Player.gameObject.transform.rotation = new Quaternion ();
		Player.animator.SetBool ("isAlive", true);
		Player.IsStuck = false;
    }
}