using UnityEngine;
using System.Collections;

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
    //--------------- RIVATE VARIABLES ----------------//

    private Rigidbody2D rigid;

    //--------------- PUBLIC VARIABLES ----------------//
  
    public float Speed = 15; // Speed of the player (control)
    public float BrownianIntensity = 0.1f; // Intensity of the random movement

    //-------------------------------------------------//

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
	
	void FixedUpdate()
    {
        // Take the inputs
        Vector2 movDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (movDirection.magnitude > 1)
            movDirection.Normalize();

        // Create random movement
        Vector2 brownian = AddbrownianMovement(new Vector2());

        // Move the player + add brownian movement
        Move(movDirection + brownian);
    }

    /// <summary>
    /// Takes a Vector2 and add forces to the rigidbody2D
    /// </summary>
    void Move(Vector2 dir)
    {
        rigid.AddForce(dir * Time.fixedDeltaTime * Speed * 1000);
    }

    /// <summary>
    /// Adds random noise to the movement
    /// </summary>
    Vector2 AddbrownianMovement(Vector2 movement)
    {
        float _noise = Mathf.PerlinNoise(Time.time, Time.time);
        movement += new Vector2(Random.Range(-_noise, _noise), Random.Range(-_noise, _noise));

        return movement * BrownianIntensity;
    } 
}