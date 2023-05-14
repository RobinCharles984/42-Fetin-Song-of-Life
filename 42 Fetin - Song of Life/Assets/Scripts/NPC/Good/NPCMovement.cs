using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    //Variables
    [Header("Components")] 
    private Vector3 dest;
    private NavMeshAgent nav;

    [Header("Movement")] 
    [SerializeField] private float speed;

    [Header("Other Values")] 
    [SerializeField] private float minTimeToChange;
    [SerializeField] private float maxTimeToChange;
    [SerializeField] private Vector3[] nextDestination;
    private int position;
    private int i = 0;
    private bool canChange;

        // Start is called before the first frame update
    void Start()
    {
        //Seting Private Components
        nav = GetComponent<NavMeshAgent>();
        
        i = 0; //Starting position counter
        canChange = true; //Starting with the path
    }

    // Update is called once per frame
    void Update()
    {
        nav.SetDestination(dest);

        //Checking if can change (variable at ChangeDestination())
        if (canChange)
        {
            canChange = false;
            StartCoroutine(ChangeDestination());
            i++;
        }

        //Checking if number of destinations it's equals to the counter
        if (i == nextDestination.Length)
        {
            i = 0;
        }
    }

    IEnumerator ChangeDestination()
    {
        dest = nextDestination[i]; //Virtual Vector3 receiving the nextDestination Vector3
        yield return new WaitForSeconds(Random.Range(minTimeToChange, maxTimeToChange));
        canChange = true;
    }
}
