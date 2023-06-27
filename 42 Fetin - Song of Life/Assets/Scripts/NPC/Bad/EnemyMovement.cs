using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    //Variables
    [Header("Movement")] 
    [SerializeField] private float damage;

    [Header("Enemy Behaviour")] 
    [SerializeField] private Collider[] enemyArea;
    [SerializeField] private float enemyAreaRadius;
    [SerializeField] private Transform enemyAreaPosition;
    [SerializeField] private int enemyAreaLayer;
    

    [Header("Components")] 
    [SerializeField]private Transform playerTransform;
    private NavMeshAgent navMesh;
    
    // Start is called before the first frame update
    void Start()
    {
        //Getting Components
        navMesh = this.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    //Chasing Player
    public void ChasePlayer()
    {
        navMesh.SetDestination(playerTransform.position);
    }
    
    //Searching
    void SearchingPlayer()
    {
        
    }
    
    //Spawning
    
}
