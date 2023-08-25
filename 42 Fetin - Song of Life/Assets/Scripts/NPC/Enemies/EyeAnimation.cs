using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeAnimation : MonoBehaviour
{
    //Variables
    [Header("References")] 
    private Animator anim;
    private EnemyMovement enemyMovement;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        enemyMovement = GetComponent<EnemyMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        //anim.SetInteger("transition", 0);
        
        if (enemyMovement.canAttack)
            anim.SetTrigger("isAttacking");
        else
            anim.SetInteger("transition", 0);
        
        
        if(enemyMovement.canChase)
            anim.SetInteger("transition", 1);
        
    }
}
