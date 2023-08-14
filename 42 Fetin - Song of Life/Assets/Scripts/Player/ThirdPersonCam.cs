using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    //Variables
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerBody;
    public Transform camHolder;
    public float camLerp;

    [Header("Values")] public float velocity;

    // Update is called once per frame
    void Update()
    {
        //Rotate orientation
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;
        
        //Rotate player body
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 inputDir = orientation.forward * vertical + orientation.right * horizontal;

        if (inputDir != Vector3.zero)
            playerBody.forward = Vector3.Slerp(playerBody.forward, inputDir.normalized, velocity * Time.deltaTime);
        camHolder.position = playerBody.position;
    }
}
