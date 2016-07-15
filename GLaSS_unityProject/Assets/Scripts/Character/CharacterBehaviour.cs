using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

// CODING STANDARDS / CONVENTIONS  
// Static fields : UpperCamelCase
// Public fields : UpperCamelCase
// Private fields : lowerCamelCase
// Functions / Methodes : UpperCamelCase
// fields created in a methods : _lowerCamelCase

public class CharacterBehaviour : MonoBehaviour {

    //-------------------------------------------------//
    //------------------- VARIABLES -------------------//
    //-------------------------------------------------//
    //--------------- PRIVATE VARIABLES ---------------//

    private Rigidbody2D rigid;

    //--------------- PUBLIC VARIABLES ----------------//

    // Gravity
    private bool hasGravity = false;
    public bool HasGravity
    {
        get
        {
            return hasGravity;
        }
        set
        {
            hasGravity = value;
            if (value == true)
            {
                foreach (AreaEffector2D areaEffect2d in GameObject.FindObjectsOfType<AreaEffector2D>())
                {
                    areaEffect2d.colliderMask -= 1 << LayerMask.NameToLayer("Player");
                }

                rigid.mass = 3;
                IsStuck = false;
            }
            else
            {
                foreach (AreaEffector2D areaEffect2d in GameObject.FindObjectsOfType<AreaEffector2D>())
                {
                    areaEffect2d.colliderMask += 1 << LayerMask.NameToLayer("Player");
                }

                rigid.mass = 1;
            }    
        }
    }

    // Inertia
    private bool hasInertia = false;
    public bool HasInertia
    {
        get
        {
            return hasInertia;
        }
        set
        {
            hasInertia = value;
            if (value == true)
            {
                rigid.drag = 1;
                rigid.angularDrag = 0;
            }
            else
            {
                rigid.drag = 50;
                rigid.angularDrag = 10;
            }
        }
    }

    public float BaseSpeed = 15;
    [HideInInspector]
    public float Speed = 15; // Speed of the player (control)

    public BrownianBehaviour brownianBehaviour;

    private bool isStuck; // Don't Get or Set isStuck -> use IsStuck
    public bool IsStuck
    {
        get
        {
            return isStuck;
        }
        set
        {
            isStuck = value;
            if (value == true)
                FreezeMovement();
            else
                RestoreMovement();
        }
    }

	public bool CanFeelVDW = true;

	// Saturation bar settings:
	public float barIncreaseSpeed = 13f;
	public float barDecreaseSpeed = 9f;
	public bool lockedPowers = false;
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
		
    [HideInInspector]
    public Animator animator;

    //-------------------------------------------------//

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        //energyBar = GameManager.Instance.Canvas.transform.FindChild("EnergyBar").GetComponent<Slider>();
		saturationBar = GameManager.Instance.Canvas.transform.FindChild("EnergyBar").GetComponent<Slider>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        brownianBehaviour = GetComponent<BrownianBehaviour>();
    }
		

	public void HandleSaturationBar() {
		bool powerActivated = HasGravity || HasInertia || !CanFeelVDW || !brownianBehaviour.canFeelBrownian;
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
		}
		if (lockedPowers) {
			if (currSaturation == minSaturation) { // when the bar reaches 0% unlock the powers
				lockedPowers = false;
			}
		}

	}

	void FixedUpdate()
    {
		// Upodate the saturation bar 
		HandleSaturationBar ();
        // Take the inputs
        Vector2 movDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (movDirection.magnitude > 1)
            movDirection.Normalize();

        // Move the player + add brownian movement
        if (!IsStuck)
        {
            Move(movDirection);
            RotateDirection(movDirection);
        }

    }

    /// <summary>
    /// Takes a Vector2 and add forces to the rigidbody2D
    /// Also takes the drag in account to keep a stable speed accross all drag values.. (not perfect but gets the job done xD)
    /// </summary>
    void Move(Vector2 inputDir)
    {
        rigid.AddForce((inputDir * Time.fixedDeltaTime * Speed * 400) * ((rigid.drag / 15) + 0.1f));

        // Not good since at each frame
        if(inputDir != Vector2.zero)
            animator.SetBool("isMoving", true);
        else
            animator.SetBool("isMoving", false);

        if (HasGravity)
        {
            rigid.AddForce(Vector2.down * Time.fixedDeltaTime * 20000);
        }           
    }

    void RotateDirection(Vector2 direction)
    {
        if (direction.magnitude <= 0.1f) // yep, to avoid dead zones
        {
            transform.DOKill();
            return;
        }

        float _angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion _q = Quaternion.AngleAxis(_angle, Vector3.forward);

        transform.DORotate(_q.eulerAngles, 0.5f, RotateMode.Fast);
    }

    private void FreezeMovement()
    {
        if (!HasGravity) // yolo
        {
            Instantiate(GameManager.Instance.feedbackVDW, transform.position, Quaternion.identity);
            rigid.mass = 500;
        }           
    }

    public void RestoreMovement()
    {
        rigid.mass = 1;
    }
}