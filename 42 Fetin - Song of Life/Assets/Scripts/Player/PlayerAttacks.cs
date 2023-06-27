using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerAttacks : MonoBehaviour
{
    //Variables
    [Header("Input")] 
    private PlayerMovement playerMovement;
    private PlayerInput playerAttackInput;
    private float moveSpeedFlag;

    [Header("Timers")]
    [SerializeField] private float spearCoolDown;
    [SerializeField] private float arrowCoolDown;
    [SerializeField] private float fluteCoolDown;
    // Start is called before the first frame update
    void Start()
    {
        playerAttackInput = this.GetComponent<PlayerInput>();
        playerMovement = this.GetComponent<PlayerMovement>();

        moveSpeedFlag = playerMovement.moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //Attacks//
    /*
     Firts it sets the player speed to zero,
    then after a timer it receives original speed again   
    */ 

    //Spear
    public void SpearAttack(CallbackContext context)
    {
        playerMovement.moveSpeed = 0;
        StartCoroutine(SpearAttackCoolDown());
    }

    //Arrow
    public void ArrowAttack(CallbackContext context)
    {
        playerMovement.moveSpeed = 0;
        StartCoroutine(ArrowAttackCoolDown());
    }

    //Flute
    public void FluteAttack(CallbackContext context)
    {
        playerMovement.moveSpeed = 0;
        StartCoroutine(FluteAttackCoolDown());
    }
    
    //Numerators
    IEnumerator SpearAttackCoolDown() //Spear
    {
        yield return new WaitForSeconds(spearCoolDown);
        playerMovement.moveSpeed = moveSpeedFlag;
    }
    
    IEnumerator ArrowAttackCoolDown() //Arrow
    {
        yield return new WaitForSeconds(arrowCoolDown);
        playerMovement.moveSpeed = moveSpeedFlag;
    }
    
    IEnumerator FluteAttackCoolDown() //Flute
    {
        yield return new WaitForSeconds(fluteCoolDown);
        playerMovement.moveSpeed = moveSpeedFlag;
    }
}
