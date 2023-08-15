using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("References")] 
    private Animator anim;
    private PlayerInputs playerInput;
    private PlayerMovement playerMovement;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInputs>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        BasicMoveAnim();
        SpearAttackingAnim();
        BowAttackingAnim();
        FluteAttackingAnim();
    }

    void BasicMoveAnim()
    {
        if(playerMovement.moveDirection.sqrMagnitude > 0 && !playerInput.attackSpear) //If is walking, play walk anim
            anim.SetInteger("transition", 1);
        else //Else, play idle anim
            anim.SetInteger("transition", 0);
        if(playerMovement.isRunning)
            anim.SetInteger("transition", 2);
    }

    void SpearAttackingAnim()
    {
        if(playerInput.attackSpear)
            anim.SetTrigger("isAttacking");
    }

    void BowAttackingAnim()
    {
        if(playerInput.attackBow)
            anim.SetTrigger("isShooting");
    }
    
    void FluteAttackingAnim()
    {
        if(playerInput.attackFlute)
            anim.SetTrigger("isPlaying");
    }
}
