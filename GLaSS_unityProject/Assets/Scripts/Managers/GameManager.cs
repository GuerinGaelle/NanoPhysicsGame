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
	public bool alreadyDeactivated = false;
	public bool keyDown = false;

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


    private int nbDeathInLevel = 0;
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
	
	void Start() {
		// TODO : Delete it from here when we are dealing with normal level progression. 
		if (SceneManager.GetActiveScene ().name == "Level 2") {			// Level 2 has already gravity unlocked
			Debug.Log ("level 2");
			LockedAllPowers ();
			UnlockPower ("gravity");
		} else if (SceneManager.GetActiveScene().name == "LD_Test_Vicky") {		// All powers are locked
			LockedAllPowers ();
		} 
		else {																 // default level: all powers are already unlocked
			UnlockPower ("no tutorial");
		}
	}

	void Update ()
	{	// Powers get locked if the saturation bar reaches maximum
		if (powersOverheat) {
			if (!alreadyDeactivated) {
                //LockAllPowers();
				DeactivateAllPowers ();
                Player.HasGravity = false;
                ColoriseButton(Player.HasGravity, "Gravity_Button");
                Player.HasInertia = false;
                ColoriseButton(Player.HasInertia, "Inertia_Button");
                Player.brownianBehaviour.canFeelBrownian = true;
                ColoriseButton(!Player.brownianBehaviour.canFeelBrownian, "Brownian_Button");
                Player.CanFeelVDW = true;
                ColoriseButton(!Player.CanFeelVDW, "VDW_Button");

                alreadyDeactivated = true;
            }     
		}
		else 
		{
			if ((Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.G)) && isGravityUnlocked) 
			{
				ToggleGravity(true);
			}
			else if((Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.H)) && isInertiaUnlocked)
			{
				ToggleInertia(true);
			}
			else if ((Input.GetKeyDown(KeyCode.Joystick1Button2) || Input.GetKeyDown(KeyCode.F)) && isBrownianUnlocked)
			{
				ToggleBrownianMovement(false);
			}
			else if ((Input.GetKeyDown(KeyCode.Joystick1Button3) || Input.GetKeyDown(KeyCode.T)) && isVDWUnlocked)
			{
				ToggleVanDerWaals(false);
			}

            if ((Input.GetKeyUp(KeyCode.Joystick1Button0) || Input.GetKeyUp(KeyCode.G)) && isGravityUnlocked)
            {
                ToggleGravity(false);
            }
            else if ((Input.GetKeyUp(KeyCode.Joystick1Button1) || Input.GetKeyUp(KeyCode.H)) && isInertiaUnlocked)
            {
                ToggleInertia(false);
            }
            else if ((Input.GetKeyUp(KeyCode.Joystick1Button2) || Input.GetKeyUp(KeyCode.F)) && isBrownianUnlocked)
            {
                ToggleBrownianMovement(true);
            }
            else if ((Input.GetKeyUp(KeyCode.Joystick1Button3) || Input.GetKeyUp(KeyCode.T)) && isVDWUnlocked)
            {
                ToggleVanDerWaals(true);
            }

        }

		if (Input.GetKeyDown(KeyCode.R)) // TODO remove it for GOLD version
        {
            SceneManager.LoadScene(SceneManager.GetSceneAt(0).ToString());
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

	void FixedUpdate() {
		HandleSaturationBar ();
	}
    
	public void ToggleGravity(bool bActivate)
    {
        if (bActivate == true)
            StopAnyActivePower();

		Player.HasGravity = bActivate;
		ColoriseButton(Player.HasGravity, "Gravity_Button");
    }
    public void ToggleInertia(bool bActivate)
    {
        if (bActivate == true)
            StopAnyActivePower();

        Player.HasInertia = bActivate;
        ColoriseButton(Player.HasInertia, "Inertia_Button");
    }
		
    public void ToggleBrownianMovement(bool bActivate)
    {
        if (bActivate == false)
            StopAnyActivePower();

        Player.brownianBehaviour.canFeelBrownian = bActivate;
		ColoriseButton (!bActivate, "Brownian_Button");
	}

    public void ToggleVanDerWaals(bool bActivate)
    {
        if (bActivate == false)
            StopAnyActivePower();

        Player.CanFeelVDW = bActivate;
        ColoriseButton(!bActivate, "VDW_Button");

        if (!Player.CanFeelVDW)
        {
            Player.IsStuck = false;
            foreach (VDWBehaviour vdwObj in FindObjectsOfType<VDWBehaviour>())
            {
                vdwObj.StopJoint();
            }
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

    public void Death()
    {
        if (Player.animator.GetBool("isAlive"))
        {
            nbDeathInLevel++;
            Player.IsStuck = true;
            Player.animator.SetBool("isAlive", false);
            Invoke("DestroyPlayer", 0.5f);

            Vector2 _deathPos = new Vector2((int)Player.transform.position.x, (int)Player.transform.position.y);

            CustomData customData = new CustomData();
            customData.Add("POSITION", Player.transform.position.ToString());
            customData.Add("LEVEL_NAME", SceneManager.GetActiveScene().name);
            RedMetricsManager.get().sendEvent(TrackingEvent.DEATH_POSITION, customData);

        }  
    }

    void DestroyPlayer()
    {  
        Instantiate(feedbackVDW, Player.transform.position, Quaternion.identity);
        foreach (VDWBehaviour vdwObj in GameObject.FindObjectsOfType<VDWBehaviour>())
        {
            vdwObj.StopJoint();
        }
        Player.IsStuck = false;
        Player.gameObject.transform.position = Checkpoint;
		Player.gameObject.transform.rotation = new Quaternion ();
		Player.animator.SetBool ("isAlive", true);
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
				alreadyDeactivated = false;
				UnlockPower ("all");
			}
		}
	}

    /// <summary>
    /// Stops any the active power
    /// </summary>
    public void StopAnyActivePower()
    {
        if (Player.HasInertia)
            ToggleInertia(false);
        if (Player.HasGravity)
            ToggleGravity(false);
        if (!Player.CanFeelVDW)
            ToggleVanDerWaals(true);
        if (!Player.brownianBehaviour.canFeelBrownian)
            ToggleBrownianMovement(true);
    }

	public void DeactivateAllPowers() {
		lockedPowers = true;

        // DAVID : We stop the powers so we are sure it does'nt create problems.
        //StopAnyActivePower();

        // then we grey out the buttons.
        gravityButtonImage.color = Color.gray;
		inertiaButtonImage.color = Color.gray;
		brownianButtonImage.color = Color.gray;
		vdwButtonImage.color = Color.gray;


	}
		
	public void LockedAllPowers() {
		// grey them out:
		gravityButtonImage.color = Color.gray;
		inertiaButtonImage.color = Color.gray;
		brownianButtonImage.color = Color.gray;
		vdwButtonImage.color = Color.gray;
		// make them unusuable:
		isGravityUnlocked = false;
		isBrownianUnlocked = false;
		isVDWUnlocked = false;
		isInertiaUnlocked = false;

		lockedPowers = true;
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
		} else if (s == "no tutorial") {
			isGravityUnlocked = true;
			isBrownianUnlocked = true;
			isVDWUnlocked = true;
			isInertiaUnlocked = true;

			gravityButtonImage.color = new Color32 (128, 255, 128, 255);
			inertiaButtonImage.color = new Color32 (255, 128, 128, 255);
			brownianButtonImage.color = new Color32 (128, 128, 255, 255);
			vdwButtonImage.color = new Color32 (255, 255, 128, 255);
		}
	}

    public void LevelFinished()
    {
        Scene activeScene = SceneManager.GetActiveScene();

        CustomData customData = new CustomData();
        customData.Add("NUMBER_OF_DEATH", nbDeathInLevel.ToString());
        customData.Add("LEVEL_NAME", activeScene.name);
        RedMetricsManager.get().sendEvent(TrackingEvent.DEATH_AVERAGE, customData);

        // Not necessary since we are not keeping any object between scenes. But if so, we should do this.
        nbDeathInLevel = 0;

        SceneManager.LoadScene(activeScene.buildIndex + 1, LoadSceneMode.Single);
    }
}