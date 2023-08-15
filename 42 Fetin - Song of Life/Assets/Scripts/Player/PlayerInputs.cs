using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    [Header("Input Values")]
    public float horizontal;
    public float vertical;
    public float run;
    public bool attackSpear;
    public bool attackBow;
    public bool attackFlute;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        run = Input.GetAxis("Run");
        attackSpear = Input.GetButtonDown("Spear");
        attackBow = Input.GetButtonDown("Bow");
        attackFlute = Input.GetButtonDown("Flute");
    }
}
