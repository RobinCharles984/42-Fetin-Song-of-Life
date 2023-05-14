using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerMovement : MonoBehaviour
{
    //Variaveis
    [Header("Movement")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float moveSpeed;
    private float velocityX;
    private float velocityZ;
    
    [Header("Gravity")] 
    [SerializeField] private float gForce;
    private float gFlag;

    [Header("Components")] 
    private CharacterController controller;
    private PlayerInput playerInput;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        
        gFlag = gForce;
    }

    // Update is called once per frame
    void Update()
    {
        Gravity();

        controller.Move(new Vector3(velocityX * moveSpeed * Time.deltaTime, 0, velocityZ * moveSpeed *Time.deltaTime));
    }

    public void Gravity()
    {
        if (!controller.isGrounded)
            controller.Move(Vector3.down * gForce * Time.deltaTime);
    }

    public void Move(CallbackContext context)
    {
        velocityX = context.ReadValue<Vector3>().x;
        velocityZ = context.ReadValue<Vector3>().z;
    }

    public void Jump()
    {

    }
}