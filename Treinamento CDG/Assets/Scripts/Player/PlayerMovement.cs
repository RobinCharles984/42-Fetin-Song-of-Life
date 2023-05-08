using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Variables
    [Header("Movement Variables")] 
    public float speed;

    [Header("Jump Variables")] 
    public float jumpForce;
    private bool isGrounded;
    public Transform groundCheck;
    public LayerMask groundLayer;

    [Header("Flip Variables")] 
    public bool isLookingLeft;
    
    [Header("Components")] 
    private Rigidbody2D rb;
    private Animator anim;
        
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        //Player Walking
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxis("Vertical");
        rb.velocity = new Vector2(h * speed, rb.velocity.y);
        
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, .2f, groundLayer); //Checking ground
        
        //Player Jumpping
        if (isGrounded && v > 0)
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        
        //Flipping
        if(h > 0 && isLookingLeft)
            Flip();
        else if (h < 0 && !isLookingLeft)
            Flip();
        
        //Animations
        anim.SetInteger("h", (int)h);
        anim.SetBool("jump", !isGrounded);
    }

    void Flip()
    {
        isLookingLeft = !isLookingLeft;

        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
    }
}
