using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[System.Serializable]
public class Dialogue
{

    [SerializeField] public GameObject playerDialogueField;
    [SerializeField] public GameObject npcDialogueField;
    [SerializeField] public TextMeshProUGUI playerDialogueText;
    [SerializeField] public TextMeshProUGUI npcDialogueText;

    public GameObject playerActor;
    public GameObject npcActor;
    public GameObject currentTrigger;
    public bool isNpcAgressive;

    public List<Speeсh> listOfPhrases = new List<Speeсh>();

    

}

[System.Serializable]
public class Speeсh
{
    public bool isPlayer;

    public string nameOfPerson; // name of the actor who is currently talking

    [TextArea(3, 10)]
    public string sentenceOfPerson; // his sentence

    


}

