using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class BrownianBehaviour : MonoBehaviour{

    private Rigidbody2D rigid;
    

    public bool canFeelBrownian = true;
    public float baseBrownianIntensity = 0.1f; // Intensity of the random movement 
    [HideInInspector]
    public float currentBrownianIntensity;

    void Start ()
    {
        rigid = GetComponent<Rigidbody2D>();
        currentBrownianIntensity = baseBrownianIntensity;
    }

	void FixedUpdate ()
    {
        // Create random movement
        Vector2 _brownian = GetBrownianMovement();
        rigid.AddForce(_brownian * Time.fixedDeltaTime * 15 * 100 * ((rigid.drag / 4) + 1)); // f***ing drag !
    }

    /// <summary>
    /// Adds random noise to the movement
    /// </summary>
    Vector2 GetBrownianMovement()
    {
        Vector2 _movement = new Vector2();

        if (canFeelBrownian)
        {
            float _noise = Mathf.PerlinNoise(Time.time, Time.time);
            _movement += new Vector2(Random.Range(-_noise, _noise), Random.Range(-_noise, _noise));
        }

        return _movement * currentBrownianIntensity;
    }
}