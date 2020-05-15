using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Dialogue
{

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

