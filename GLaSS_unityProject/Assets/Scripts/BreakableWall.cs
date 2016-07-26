﻿using UnityEngine;
using System.Collections;

public class BreakableWall : MonoBehaviour {

    public float minVelocityToBreak = 20;
    public RuntimeAnimatorController anim;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    void BreakWall()
    {
        animator.enabled = true;
        Invoke("DestroyMe", 0.6f);       
    }

    void DestroyMe()
    {
        Destroy(this.gameObject);
    }

    void Update()
    {
        //Debug.Log("Velocity of Player hitting wall : " + GameManager.Instance.Player.transform.GetComponent<Rigidbody2D>().velocity.magnitude); // TODO remove debug
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.tag == "Player")
        {           
            if(other.transform.GetComponent<Rigidbody2D>().velocity.magnitude > minVelocityToBreak)
            {
                BreakWall();
            }
        }
    }
}
