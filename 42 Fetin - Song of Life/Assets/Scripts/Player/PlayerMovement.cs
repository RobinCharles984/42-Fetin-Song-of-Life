using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Variables
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerBody;
    private Rigidbody rb; //Body rb
    private Vector3 _moveDirection;

    [Header("Movement")] 
    public float maxSpeed;
    public float groundDrag;
    public bool isRunning;
    float horizontal;
    float vertical;
    float run;

    [Header("Ground Check")] 
    public float playerHeight;
    public LayerMask whatIsGround;
    private bool grounded;
    
    //Capsules
    public Vector3 moveDirection
    {
        get { return _moveDirection; }
        set { _moveDirection = value; }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        InEditor();
        MyInput();
        
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        OnMove();
        OnRun();
    }

    void OnMove()
    {
        _moveDirection = orientation.forward * vertical + orientation.right * horizontal;
        
        rb.AddForce(_moveDirection.normalized * maxSpeed * 10f, ForceMode.Force);
    }

    void OnRun()
    {
        if (run != 0 && _moveDirection.sqrMagnitude > 0)
        {
            maxSpeed = 13;
            isRunning = true;
        }
        else
        {
            maxSpeed = 7;
            isRunning = false;
        }
    }

    // Things showing just in editor
    void InEditor()
    {
        if(grounded)
            Debug.DrawLine(transform.position, Vector3.down, Color.green);
        else
            Debug.DrawLine(transform.position, Vector3.down, Color.red);
    }
    
    //Inputs
    void MyInput()
    { 
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        run = Input.GetAxis("Run");
    }
}
