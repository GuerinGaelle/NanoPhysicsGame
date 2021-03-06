﻿using UnityEngine;
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
                foreach (AreaEffector2D areaEffect2d in FindObjectsOfType<AreaEffector2D>())
                {
                    areaEffect2d.useColliderMask = true;
                    //rigid.drag = 300;
                }
            }
            else
            {
                foreach (AreaEffector2D areaEffect2d in FindObjectsOfType<AreaEffector2D>())
                {
                    areaEffect2d.useColliderMask = false;
                    //rigid.drag = 50;
                }
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
                foreach (BrownianArea broArea in FindObjectsOfType<BrownianArea>())
                {
                    broArea.brownianIntensity *= 3;
                }
            }
            else
            {
                rigid.drag = 50;
                rigid.angularDrag = 10;
                foreach (BrownianArea broArea in FindObjectsOfType<BrownianArea>())
                {
                    broArea.brownianIntensity /= 3;
                }
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


    [HideInInspector]
    public Animator animator;

    private bool isInForceCurrent;
    private AreaEffector2D forceCurrent;

    public Vector2 actualWantedVelocity;

    private Vector3 scaleRight = new Vector3(1, 1, 1);
    private Vector3 scaleLeft = new Vector3(1, -1, 1);

    //-------------------------------------------------//

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();    
		animator = transform.GetChild(0).GetComponent<Animator>();
        brownianBehaviour = GetComponent<BrownianBehaviour>();
    }		


	void FixedUpdate()
    {
        // Take the inputs
        actualWantedVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (actualWantedVelocity.magnitude > 1)
            actualWantedVelocity.Normalize();

        // We change the scale depending on the orientation
        if (actualWantedVelocity.x < 0 && transform.localScale != scaleLeft)
            transform.localScale = scaleLeft;

        if (actualWantedVelocity.x > 0 && transform.localScale != scaleRight)
            transform.localScale = scaleRight;

        // Move the player + add brownian movement
        if (!IsStuck)
        {
            Move(actualWantedVelocity);
            RotateDirection(actualWantedVelocity);
        }

        if(isInForceCurrent && HasInertia)
        {
            rigid.drag = 35; // will be overiten when HasInertia = false or when leaving forceCurrent
        }

    }

    /// <summary>
    /// Takes a Vector2 and add forces to the rigidbody2D
    /// Also takes the drag in account to keep a stable speed accross all drag values.. (not perfect but gets the job done xD)
    /// </summary>
    void Move(Vector2 inputDir)
    {
        #region Scripting for direction forbiddeness
        if (isInForceCurrent && forceCurrent.forceMagnitude != 0)
        {
            switch ((int)forceCurrent.forceAngle)
            {
                case 90:
                    // Pas le droit d'aller vers le bas
                    if (inputDir.y < 0)
                        inputDir.y = 0;
                    break;
                case 180:
                    // Pas le droit d'aller vers la droite
                    if (inputDir.x > 0)
                        inputDir.x = 0;
                    break;
                case 270:
                    // Pas le droit d'aller vers le haut
                    if (inputDir.y > 0)
                        inputDir.y = 0;
                    break;
                case 0:
                    // Pas le droit d'aller vers la gauche
                    if (inputDir.x < 0)
                        inputDir.x = 0;
                    break;
                default:
                    break;
            }
        }

        if (HasGravity)
        {
            // Pas le droit d'aller doublement vers le bas
            if (inputDir.y < 0)
                inputDir.y = 0;
        }
        #endregion

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
            //rigid.constraints = RigidbodyConstraints2D.FreezeAll;
        }           
    }

    public void RestoreMovement()
    {
        rigid.mass = 1;
        //rigid.constraints = RigidbodyConstraints2D.None;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetComponent<AreaEffector2D>() == true)
        {
            isInForceCurrent = true;
            forceCurrent = other.GetComponent<AreaEffector2D>();
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<AreaEffector2D>() == true)
        {
            isInForceCurrent = false;
            forceCurrent = null;

            if (!HasInertia)
                rigid.drag = 50;
            else
                rigid.drag = 1;
        }
    }
}