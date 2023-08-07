using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("References")] 
    private Animator anim;
    private PlayerMovement player;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player.moveDirection.sqrMagnitude > 0) //If is walking, play walk anim
            anim.SetInteger("transition", 1);
        else //Else, play idle anim
            anim.SetInteger("transition", 0);
        if(player.isRunning)
            anim.SetInteger("transition", 2);
    }
}
