using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    //Variables
    [Header("Components")] 
    private Rigidbody rb;
    private Animator anim;
    
    [Header("Scripts")] 
    private PlayerMovement playerMovement;

    [Header("Health")] 
    public int hp;
    public float invinvibleTime;
    private bool takeDamage;

    [Header("Pushing Away Variables")] 
    public float pushDistance;
    private Vector3 direction;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();

        takeDamage = false;
    }

    // Update is called once per frame
    void Update()
    {
        //If health reaches 0 or below
        if (hp <= 0)
        {
            hp = 0;
            Destroy(gameObject);
        }
    }

    //Collision Function
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyHit" && !takeDamage)
        {
            direction = (other.transform.position - playerMovement.transform.position).normalized;
            transform.position += direction * pushDistance;
            playerMovement.walkSpeed = 0f;
            hp--;
            StartCoroutine(DamageCoolDown(invinvibleTime));
        }
    }

    IEnumerator DamageCoolDown(float time)
    {
        takeDamage = true;
        anim.SetBool("takeDamage", takeDamage);
        yield return new WaitForSeconds(time);
        takeDamage = false;
        anim.SetBool("takeDamage", takeDamage);
    }
}
