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

    [Header("Movement")] 
    public float walkSpeed;
    public float runSpeed;
    public float groundDrag;
    public bool isRunning;
    float speed;
    float horizontal;
    float vertical;
    float run;
    bool _attackSpear;
    bool attackBow;
    bool attackFlute;

    [Header("Timers")] 
    public float spearCoolDown;

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

    public bool attackSpear
    {
        get { return _attackSpear; }
        set { _attackSpear = value; }
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
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Calling functions
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
        //Movements
        OnMove();
        OnRun();

        if (_attackSpear)
            StartCoroutine(CoolDown());
    }

    void OnMove()
    {
        _moveDirection = orientation.forward * vertical + orientation.right * horizontal;
        
        rb.AddForce(_moveDirection.normalized * walkSpeed * 10f, ForceMode.Force);
    }

    //Running function
    void OnRun()
    {
        if (!_attackSpear && !attackBow && !attackFlute)
        {
            if (run != 0 && _moveDirection.sqrMagnitude > 0)
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
    
    //Inputs
    void MyInput()
    { 
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        run = Input.GetAxis("Run");
        _attackSpear = Input.GetButtonDown("Spear");
        attackBow = Input.GetButtonDown("Bow");
        attackFlute = Input.GetButtonDown("Flute");
    }
    
    //Numerators
    IEnumerator CoolDown()
    {
        thirdPersonCamScript.velocity = 0;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        horizontal = vertical = 0;
        yield return new WaitForSeconds(spearCoolDown);
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        thirdPersonCamScript.velocity = 7;
    }
}
