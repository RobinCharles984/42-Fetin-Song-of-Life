using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    //Variables
    [Header("Movement")] 
    public float speed;
    public float minDistance;
    public float maxDistance;
    
    [SerializeField] private float damage;

    [Header("Attacks")] 
    private bool _canAttack;

    [Header("AI")] 
    private bool _canChase;
    
    [Header("Components")] 
    [SerializeField]private Transform playerTransform;

    //Capsules
    public bool canAttack
    {
        get { return _canAttack; }
        set { _canAttack = value; }
    }

    public bool canChase
    {
        get { return _canChase; }
        set { _canChase = value; }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        //Getting Components
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_canChase)
        {
            ChasePlayer();
        }
    }

    private void FixedUpdate()
    {
        
    }

    public void FoundPlayer()
    {
        StartCoroutine(FoundPlayerTimer());
    }

    //Chasing Player
    public void ChasePlayer()
    {
        //If it's at the chase range -> run at player direction
        if (Vector3.Distance(transform.position, playerTransform.position) >= minDistance)
        {
            //Loking Y-Axis
            var v3 = transform.forward;
            v3.y = 0.0f;
            //Always look at player
            transform.LookAt(playerTransform);
            transform.position += v3 * speed * Time.deltaTime;
            _canAttack = false;
        }
        else
        {
            transform.position = transform.position;
            _canAttack = true;
        }
    }
    
    //Searching
    void SearchingPlayer()
    {
        
    }
    
    //Spawning

    //Timers and Numerators
    public IEnumerator FoundPlayerTimer()
    {
        float speedFlag = speed;
        speed = 0;
        yield return new WaitForSeconds(2f);
        speed = speedFlag;
    }
}
