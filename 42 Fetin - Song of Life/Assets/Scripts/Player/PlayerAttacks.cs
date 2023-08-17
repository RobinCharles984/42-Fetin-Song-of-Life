/*
    Copyright CharlesEntertainment
    Player Attacks Scripts
    
    -When press spear attack button(Square/X):
    . The Spear at the back disappears
    . The Spear at the hand appears
    
    -When press bow attack button(Roll/B):
    . The Bow at the leg disappears
    . The Bow at the hand appears
    
    -When press flute attack button(Triangle/Y):
    . The Flute at the leg disappears
    . The Flute at the hand appears
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    //Variables
    [Header("References")]
    public GameObject spearFigure;
    public GameObject spearHand;
    public GameObject bowFigure;
    public GameObject bowHand;
    public GameObject fluteFigure;
    public GameObject fluteHand;

    [Header("Scripts")] 
    public ThirdPersonCam thirdPersonCamScript;
    private PlayerInputs playerInput;

    [Header("Components")] 
    private Rigidbody rb;
    
    [Header("Timers")] 
    public float spearCoolDown;
    public float bowCoolDown;
    public float fluteCoolDown;
    //Flags
    float spearCoolDownFlag;

    // Start is called before the first frame update
    void Start()
    {
        //Getting Components
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInputs>();
        
        //Deactivating when Start
        spearHand.SetActive(false);
        bowHand.SetActive(false);
        fluteHand.SetActive(false);
        
        //Flags
        spearCoolDownFlag = spearCoolDown;
    }

    // Update is called once per frame
    void Update()
    {
        OnSpearAttack();
        OnBowAttack();
        OnFluteAttack();
    }
    
    //Functions
    private void OnSpearAttack()
    {
        if (playerInput.attackSpear)
        {
            StartCoroutine(SpearCoolDown());
        }
    }

    void OnBowAttack()
    {
        if (!playerInput.attackSpear && playerInput.attackBow && !playerInput.attackFlute)
        {
            
        }
    }

    void OnFluteAttack()
    {
        if (!playerInput.attackSpear && !playerInput.attackBow && playerInput.attackFlute)
        {
            
        }
    }

    IEnumerator SpearCoolDown()
    {
        spearFigure.SetActive(false);
        spearHand.SetActive(true);
        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        thirdPersonCamScript.velocity = 0;
        yield return new WaitForSeconds(spearCoolDown);
        thirdPersonCamScript.velocity = 7;
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        spearFigure.SetActive(true);
        spearHand.SetActive(false);
    }
}
