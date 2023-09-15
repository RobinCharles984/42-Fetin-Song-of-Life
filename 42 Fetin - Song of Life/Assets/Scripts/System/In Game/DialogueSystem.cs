using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    //Variables
    [Header("Components")] 
    public TMP_Text message;
    public TMP_Text messagerName;

    [Header("References")] 
    public Animator anim;

    [Header("Message Variables")] 
    public string name;
    public string messageText;
    private bool isTalking;

    // Start is called before the first frame update
    void Start()
    {
        isTalking = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            anim.SetInteger("transition", 1);
            message.text = messageText;
            messagerName.text = name;
            isTalking = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isTalking)
        {
            anim.SetInteger("transition", 2);
            isTalking = false;
        }
    }
}
