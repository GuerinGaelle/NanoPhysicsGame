using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

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
	public static bool inLevelSelectMenu = false;

	// powers booleans:
	public bool isGravityUnlocked = false;
	public bool isInertiaUnlocked = false;
	public bool isVDWUnlocked = false;
	public bool isBrownianUnlocked = false;

	public bool lockedPowers;
	public bool powersOverheat = false;
	public bool alreadyDeactivated = false;
	public bool keyDown = false;

	// scientific tutorials booleans:
	public bool visitedGravitySc = false;
	public bool visitedInertiaSc = false;
	public bool visitedBrownianSc = false;
	public bool visitedVDWSc = false;
	public bool visitedViscositySc = false;

	public bool visitedRotiferaPop = false;
	public bool visitedCheckpointPop = false;
	public bool visitedParticlesPop = false;

	// icons of powers
	public Image gravityButtonImage;
	public Image inertiaButtonImage;
	public Image brownianButtonImage;
	public Image vdwButtonImage;


	// Saturation bar settings:
	public float barIncreaseSpeed = 13f;
	public float barDecreaseSpeed = 9f;

	public Slider saturationBar;
	public GameObject barObject;
	public bool isBarDecreasing = false;

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

	public Image saturationColor;
	public bool isRed = false;
	public int count = 0;
	public bool isBarVisible = true;
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
		barObject = GameObject.Find ("EnergyBar");
        saturationColor = barObject.transform.FindChild("Fill Area").GetChild(0).GetComponent<Image>();

        gravityButtonImage = GameObject.Find ("Buttons").transform.FindChild ("Gravity_Button").GetComponent<Image> ();
		inertiaButtonImage = GameObject.Find ("Buttons").transform.FindChild ("Inertia_Button").GetComponent<Image> ();
		brownianButtonImage = GameObject.Find ("Buttons").transform.FindChild ("Brownian_Button").GetComponent<Image> ();
		vdwButtonImage = GameObject.Find ("Buttons").transform.FindChild ("VDW_Button").GetComponent<Image> ();
		inLevelSelectMenu = false;
    }
	
	void Start()
    {
        // We set the checkpoint to the spawning point
        if (Checkpoint == null || Checkpoint == Vector2.zero)
            Checkpoint = Player.transform.position;

		// TODO : Delete it from here when we are dealing with normal level progression. 
		if (SceneManager.GetActiveScene ().name == "Level 0 version Adrien" || SceneManager.GetActiveScene ().name == "Level 0") {
			LockedAllPowers ();
		} else if (SceneManager.GetActiveScene ().name == "Level 1") {		// Level 1  has already gravity unlocked
			LockedAllPowers ();
			UnlockPower ("gravity cheat");
			UnlockPower ("saturation cheat");
		} else if (SceneManager.GetActiveScene ().name == "Level 2") {			// Level 2 has already gravity unlocked
			LockedAllPowers ();
			UnlockPower ("gravity cheat");
			UnlockPower ("saturation cheat");
		} else if (SceneManager.GetActiveScene ().name == "LD_Test_Vicky") {		// All powers are locked
			LockedAllPowers ();
		} else if (SceneManager.GetActiveScene ().name == "Level 3" || SceneManager.GetActiveScene ().name == "Level 3 version Adrien") {
			LockedAllPowers ();
			UnlockPower ("gravity cheat");
			UnlockPower ("saturation cheat");
			UnlockPower ("brownian cheat");
			UnlockPower ("vdw cheat");
		} else if (SceneManager.GetActiveScene().name == "Level 4")
        {
            LockedAllPowers();
            UnlockPower("gravity cheat");
            UnlockPower("saturation cheat");
            UnlockPower("brownian cheat");
            UnlockPower("vdw cheat");
            UnlockPower("inertia cheat");
        }
        else if (SceneManager.GetActiveScene().name == "Level 5")
        {
            LockedAllPowers();
            UnlockPower("gravity cheat");
            UnlockPower("saturation cheat");
            UnlockPower("brownian cheat");
            UnlockPower("vdw cheat");
            UnlockPower("inertia cheat");
        }

        else {																 // default level: all powers are already unlocked
			UnlockPower ("no tutorial");
			//UIManager.Instance.CloseAllPopups();
			UIManager.Instance.CloseGameplayPopups();
			UIManager.Instance.CloseScientificPopups ();
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
            AudioClip deathClip = Resources.Load<AudioClip>("Music/son/Death");
            GetComponentInChildren<AudioSource>().PlayOneShot(deathClip);

            nbDeathInLevel++;
            transform.localScale = new Vector3(1, 1, 1);
            transform.rotation = Quaternion.identity;
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

		Saturation = 0f;					
		Player.animator.SetBool ("isAlive", true);
    }

	public void HandleSaturationBar()
    {
        bool powerActivated = Player.HasGravity || Player.HasInertia || !Player.CanFeelVDW || !Player.brownianBehaviour.canFeelBrownian;
		float currSaturation = saturationBar.value;
		float maxSaturation = saturationBar.maxValue;
		float minSaturation = saturationBar.minValue;

		float saturPercentage;


		saturPercentage = currSaturation / maxSaturation * 100;

		if (powerActivated && currSaturation < maxSaturation) { 	// if any power is activated, increase the bar
			Saturation += Time.fixedDeltaTime * barIncreaseSpeed; 
			isBarDecreasing = false;
		}
		if (!powerActivated && currSaturation > minSaturation) { 	// if no power is activated, decrease the bar
			Saturation -= Time.fixedDeltaTime * barDecreaseSpeed;
			isBarDecreasing = true;
			if (!barObject.activeSelf)
				barObject.SetActive (true);
		}


		// ----- MAKING THE BAR RED & FLASHING WHEN OVERLOADING -----
		if (saturPercentage > 70 && !isRed) {
			isRed = true;
			saturationColor.color = Color.red;
		}
		if (saturPercentage <= 1f && isRed && powersOverheat) {
			isRed = false;
			count = 0;

			saturationColor.color = Color.yellow;
		}

		if (saturPercentage <= 70 && isRed && !powersOverheat) {
			isRed = false;
			count = 0;

			saturationColor.color = Color.yellow;
		}

		if (isRed && !isBarDecreasing) {					// flash the bar
			count++;
			if (count == 10) {
				count = 0;
				if (barObject.activeSelf)
					barObject.SetActive (false);
				else
					barObject.SetActive (true);
			}
		}
		// ----------------------------------------------------------


		if (currSaturation == maxSaturation) { 		// if the bar goes to 100% then lock all the powers
			lockedPowers = true;
			powersOverheat = true;

            AudioClip FullCharge = Resources.Load<AudioClip>("Music/son/Full charge");
            GetComponent<AudioSource>().PlayOneShot(FullCharge);

        }
		if (lockedPowers && powersOverheat) {
			if (currSaturation == minSaturation) { // when the bar reaches 0% unlock the powers
				powersOverheat = false;
				alreadyDeactivated = false;
				UnlockPower ("all");
				isBarDecreasing = false;
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
		// normal unlocking
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
		
		}					// unlocking powers through GameManager (cheat!)
							// TODO: delete for gold version
		else if (s == "gravity cheat") {
				gravityButtonImage.color = new Color32 (128, 255, 128, 255);
				isGravityUnlocked = true;
				UIManager.Instance.tutListGame.Add (UIManager.Instance.gravityTutGame);
			} else if (s == "saturation cheat") {
				UIManager.Instance.tutListGame.Add (UIManager.Instance.saturationTutGame);
			}
			else if (s == "inertia cheat") {
				inertiaButtonImage.color = new Color32 (255, 128, 128, 255);
				isInertiaUnlocked = true;
				UIManager.Instance.tutListGame.Add (UIManager.Instance.inertiaTutGame);
			} else if (s == "brownian cheat") {
				brownianButtonImage.color = new Color32 (128, 128, 255, 255);
				isBrownianUnlocked = true;
				UIManager.Instance.tutListGame.Add (UIManager.Instance.brownianTutGame);
			} else if (s == "vdw cheat") {
				vdwButtonImage.color = new Color32 (255, 255, 128, 255);
				isVDWUnlocked = true;
				UIManager.Instance.tutListGame.Add (UIManager.Instance.vdwTutGame);	
		}

		else if (s == "all") {				// after saturation bar is to 0%:
			if (isGravityUnlocked)
				gravityButtonImage.color = new Color32 (128, 255, 128, 255);
			if (isInertiaUnlocked)
				inertiaButtonImage.color = new Color32 (255, 128, 128, 255);
			if (isBrownianUnlocked)
				brownianButtonImage.color = new Color32 (128, 128, 255, 255);
			if (isVDWUnlocked)
				vdwButtonImage.color = new Color32 (255, 255, 128, 255);
		} else if (s == "no tutorial") {			// cheat for developing! unlocks everything
			isGravityUnlocked = true;
			isBrownianUnlocked = true;
			isVDWUnlocked = true;
			isInertiaUnlocked = true;

			gravityButtonImage.color = new Color32 (128, 255, 128, 255);
			inertiaButtonImage.color = new Color32 (255, 128, 128, 255);
			brownianButtonImage.color = new Color32 (128, 128, 255, 255);
			vdwButtonImage.color = new Color32 (255, 255, 128, 255);

			UIManager.Instance.tutListGame.Add (UIManager.Instance.gravityTutGame);
			UIManager.Instance.tutListGame.Add (UIManager.Instance.saturationTutGame);
			UIManager.Instance.tutListGame.Add (UIManager.Instance.brownianTutGame);
			UIManager.Instance.tutListGame.Add (UIManager.Instance.vdwTutGame);
			UIManager.Instance.tutListGame.Add (UIManager.Instance.inertiaTutGame);
		}
	}

    public void LevelFinished()
    {
        Player.IsStuck = true;

        Scene activeScene = SceneManager.GetActiveScene();
        int nbScene = activeScene.buildIndex - 2; // TODO watch if we modify the build order

        if (LevelManager.UnlockedLevels == nbScene)
        {
            Debug.Log("CurrentScene:" + nbScene);

            int nbScene1 = nbScene + 1;
            LevelManager.SaveData(nbScene1);
        }

        // Invoke to return to main menu
        if (SceneManager.GetActiveScene().buildIndex == 8)
            Invoke("ReturnToMenu", 12);
        else
            Invoke("ReturnToMenu", 6);


        // Handles the movie playing
        int nbMovie = nbScene++;
        MovieTexture _movTexture = Resources.Load<MovieTexture>("Movies/Carte_" + nbMovie) as MovieTexture;

        if (SceneManager.GetActiveScene().buildIndex == 8) // END LEVEL
            _movTexture = Resources.Load<MovieTexture>("Movies/ANIMATION FIN") as MovieTexture;

        GameObject.Find("MoviePlayer").GetComponent<MeshRenderer>().enabled = true;
        GameObject.Find("MoviePlayer").GetComponent<Renderer>().material.mainTexture = _movTexture;

        ((MovieTexture)GameObject.Find("MoviePlayer").GetComponent<Renderer>().material.mainTexture).Play();
        _movTexture.Play();



        // Not necessary since we are not keeping any object between scenes. But if so, we should do this.
        nbDeathInLevel = 0;

        CustomData customData = new CustomData();
        customData.Add("NUMBER_OF_DEATH", nbDeathInLevel.ToString());
        customData.Add("LEVEL_NAME", activeScene.name);
        RedMetricsManager.get().sendEvent(TrackingEvent.DEATH_AVERAGE, customData);
    }

    void ReturnToMenu()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Single); // Menu
    }
}