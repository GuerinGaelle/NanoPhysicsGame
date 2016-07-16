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
	public PowerControl PowerController;

    public GameObject Canvas;
	public float increaseSpeed = 100f;
	public float decreaseSpeed = 50f;

    [HideInInspector]
    public GameObject feedbackVDW;
	public static Vector2 Checkpoint;
	//public Slider saturationBar;

	public bool isGravityUnlocked = false;
	public bool isInertiaUnlocked = false;
	public bool isVDWUnlocked = false;
	public bool isBrownianUnlocked = false;
	public bool lockedPowers;
	public bool powersOverheat = false;

	public Image gravityButtonImage;
	public Image inertiaButtonImage;
	public Image brownianButtonImage;
	public Image vdwButtonImage;


	// Saturation bar settings:
	public float barIncreaseSpeed = 13f;
	public float barDecreaseSpeed = 9f;

	public Slider saturationBar;

	private float saturation = 0;
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
	}
    //-------------------------------------------------//

    void Awake()
    {
        if(Instance == null)
            Instance = this;

        Player = FindObjectOfType<CharacterBehaviour>();
        Canvas = GameObject.Find("Canvas");

        feedbackVDW = Resources.Load<GameObject>("Prefabs/Signes_Feedbacks/VDWFeedback");
		//saturationBar = Canvas.transform.FindChild("EnergyBar").GetComponent<Slider>();
		saturationBar = GameObject.Find("Canvas").transform.FindChild("EnergyBar").GetComponent<Slider>();

		gravityButtonImage = GameObject.Find ("Buttons").transform.FindChild ("Gravity_Button").GetComponent<Image> ();
		inertiaButtonImage = GameObject.Find ("Buttons").transform.FindChild ("Inertia_Button").GetComponent<Image> ();
		brownianButtonImage = GameObject.Find ("Buttons").transform.FindChild ("Brownian_Button").GetComponent<Image> ();
		vdwButtonImage = GameObject.Find ("Buttons").transform.FindChild ("VDW_Button").GetComponent<Image> ();

    }
	
	// Update is called once per frame
	void Update ()
	{	// Powers get locked if the saturation bar reaches maximum
		if (powersOverheat) {
			LockAllPowers ();
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
			if ((Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.G)
				|| Input.GetKeyUp(KeyCode.Joystick1Button0) || Input.GetKeyUp(KeyCode.G)) && isGravityUnlocked) 
			{
				ToggleGravity();
			}
			else if((Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.H)
				|| Input.GetKeyUp(KeyCode.Joystick1Button1) || Input.GetKeyUp(KeyCode.H)) && isInertiaUnlocked)
			{
				ToggleInertia();
			}
			else if ((Input.GetKeyDown(KeyCode.Joystick1Button2) || Input.GetKeyDown(KeyCode.F)
				|| Input.GetKeyUp(KeyCode.Joystick1Button2) || Input.GetKeyUp(KeyCode.F)) && isBrownianUnlocked)
			{
				ToggleBrownianMovement();
			}
			else if ((Input.GetKeyDown(KeyCode.Joystick1Button3) || Input.GetKeyDown(KeyCode.T)
				|| Input.GetKeyUp(KeyCode.Joystick1Button3) || Input.GetKeyUp(KeyCode.T)) && isVDWUnlocked)
			{
				ToggleVanDerWaals();
			}
		}

		if (Input.GetKeyDown(KeyCode.R)) // TODO remove it for GOLD version
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

	void FixedUpdate() {
		HandleSaturationBar ();
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

	public void HandleSaturationBar() {
		bool powerActivated = Player.HasGravity || Player.HasInertia || !Player.CanFeelVDW || !Player.brownianBehaviour.canFeelBrownian;
		float currSaturation = saturationBar.value;
		float maxSaturation = saturationBar.maxValue;
		float minSaturation = saturationBar.minValue;

		if (powerActivated && currSaturation < maxSaturation) { 	// if any power is activated, increase the bar
			Saturation += Time.fixedDeltaTime * barIncreaseSpeed; 	
		}
		if (!powerActivated && currSaturation > minSaturation) { 	// if no power is activated, decrease the bar
			Saturation -= Time.fixedDeltaTime * barDecreaseSpeed;
		}

		if (currSaturation == maxSaturation) { 		// if the bar goes to 100% then lock all the powers
			lockedPowers = true;
			powersOverheat = true;
		}
		if (lockedPowers && powersOverheat) {
			if (currSaturation == minSaturation) { // when the bar reaches 0% unlock the powers
				powersOverheat = false;
				UnlockPower ("all");
			}
		}

	}

	public void LockAllPowers() {
		lockedPowers = true;
		gravityButtonImage.color = Color.gray;
		inertiaButtonImage.color = Color.gray;
		brownianButtonImage.color = Color.gray;
		vdwButtonImage.color = Color.gray;
	}

	public void UnlockPower(string s) {
		lockedPowers = false;

		if (s == "gravity") {
			gravityButtonImage.color = new Color32 (128, 255, 128, 255);
			isGravityUnlocked = true;
		} else if (s == "inertia") {
			inertiaButtonImage.color = new Color32 (255, 128, 128, 255);
			isInertiaUnlocked = true;
		} else if (s == "brownian") {
			brownianButtonImage.color = new Color32 (128, 128, 255, 255);
			isBrownianUnlocked = true;
		} else if (s == "vdw") {
			vdwButtonImage.color = new Color32 (255, 255, 128, 255);
			isVDWUnlocked = true;
		} else if (s == "all") {
			if (isGravityUnlocked)
				gravityButtonImage.color = new Color32 (128, 255, 128, 255);
			if (isInertiaUnlocked)
				inertiaButtonImage.color = new Color32 (255, 128, 128, 255);
			if (isBrownianUnlocked)
				brownianButtonImage.color = new Color32 (128, 128, 255, 255);
			if (isVDWUnlocked)
				vdwButtonImage.color = new Color32 (255, 255, 128, 255);
		}


	}
}