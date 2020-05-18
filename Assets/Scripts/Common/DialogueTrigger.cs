using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    

    private BoxCollider2D _currentTrigger; 
    public Dialogue dialogue;

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogManager>().StartDialogue(dialogue);
    }

    void Start()
    {
        _currentTrigger = transform.GetComponent<BoxCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            TriggerDialogue();
        }
    }

    public void DeactivateTrigger()
    {
        _currentTrigger.enabled = false;
    }

}
