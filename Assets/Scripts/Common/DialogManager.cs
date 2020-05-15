using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    private Queue<Speeсh> sentences;

    void Start()
    {
        sentences = new Queue<Speeсh>();
    }

    public void StartDialogue(Dialogue dialogue)
    {

        sentences.Clear();

        foreach (var sentence in dialogue.listOfPhrases)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();

    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        var sentence = sentences.Dequeue();
        Debug.Log(sentence);

    }

    public void EndDialogue()
    {
        Debug.Log("End of conversation");
    }

}
