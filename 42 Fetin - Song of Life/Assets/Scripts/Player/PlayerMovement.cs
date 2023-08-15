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
    public Transform dustPosition;
    public GameObject dust;
    public ThirdPersonCam thirdPersonCamScript;
    private Rigidbody rb; //Body rb
    private Vector3 _moveDirection;
    private PlayerInputs playerInput;

    [Header("Movement")] 
    public float walkSpeed;
    public float runSpeed;
    public float groundDrag;
    public bool isRunning;
    float speed;

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

    //Firts thing called
    private void Awake()
    {
        //Flag variables
        speed = walkSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Getting Componnets
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInputs>();
        
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Calling functions
        InEditor();
        
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
        //Movements
        OnMove();
        OnRun();
    }

    void OnMove()
    {
        _moveDirection = orientation.forward * playerInput.vertical + orientation.right * playerInput.horizontal;
        
        rb.AddForce(_moveDirection.normalized * walkSpeed * 10f, ForceMode.Force);
    }

    //Running function
    void OnRun()
    {
        if (!playerInput.attackSpear && !playerInput.attackBow && !playerInput.attackFlute)
        {
            if (playerInput.run != 0 && _moveDirection.sqrMagnitude > 0)
            {
                isRunning = true;
            }
            else
            {
                walkSpeed = speed;
                isRunning = false;
            }

            if (isRunning)
            {
                walkSpeed = runSpeed;
            }
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
    
    //Playing Dust FX
    void CreateDust()
    {
        Instantiate(dust, transform);
    }
}
