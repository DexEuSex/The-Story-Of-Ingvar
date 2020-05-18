using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    private GameObject _currentPlayerDialogueField;
    private GameObject _currentNpcDialogueField;
    private TextMeshProUGUI _currentPlayerDialogueText;
    private TextMeshProUGUI _currentNpcDialogueText;

    private GameObject _currentTrigger;
    private Queue<string> _sentences;
    private Queue<bool> _isPlayerSpeaking;
    private Animator _playerAnimator;
    private Animator _npcAnimator;
    private GameObject _playerActor;
    private GameObject _npcActor;

    private bool _isNpcAgressive;
    public static bool isDialogOpened = false;

    void Start()
    {
        _sentences = new Queue<string>();
        _isPlayerSpeaking = new Queue<bool>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        _sentences.Clear();

        isDialogOpened = true; // Переменная не даст боссу атаковать раньше времени

        // Сбор данных для диалога с того триггера, который сейчас задействован
        _currentPlayerDialogueField = dialogue.playerDialogueField;
        _currentNpcDialogueField = dialogue.npcDialogueField;
        _currentPlayerDialogueText = dialogue.playerDialogueText;
        _currentNpcDialogueText = dialogue.npcDialogueText;

        
        _currentTrigger = dialogue.currentTrigger.gameObject;
        _playerAnimator = dialogue.playerActor.GetComponent<Animator>();
        _npcAnimator = dialogue.npcActor.GetComponent<Animator>();
        _playerActor = dialogue.playerActor.gameObject;
        _npcActor = dialogue.npcActor.gameObject;
        _isNpcAgressive = dialogue.isNpcAgressive;

        _playerAnimator.SetInteger("AnimState", 0); // чтобы персонаж не бежал на месте, присвоение ему состояния IDLE на старте диалога
        PlayerMovement.DeactivateControls(); // Деактивация управления, чтобы игрок не мог бегать во время диалогов
       
        foreach(var phrase in dialogue.listOfPhrases)
        {
            _sentences.Enqueue(phrase.sentenceOfPerson);
        }

        foreach(var whoIsSpeaking in dialogue.listOfPhrases)
        {
            _isPlayerSpeaking.Enqueue(whoIsSpeaking.isPlayer);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(_sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        var sentenceToDisplay = _sentences.Dequeue();
        var isPlayerSpeaking = _isPlayerSpeaking.Dequeue();

        if (isPlayerSpeaking)
        {
            DeactivateNPCDialogueField();
            ActivatePlayerDialogueField();
            StartCoroutine(TypeSentence(sentenceToDisplay, _currentPlayerDialogueText));
        }

        else if (!isPlayerSpeaking)
        {
            DeactivatePlayerDialogueField();
            ActivateNPCDialogueField();
            StartCoroutine(TypeSentence(sentenceToDisplay, _currentNpcDialogueText));
        }

    }

    public void EndDialogue()
    {
        // Деактивация диалоговых полей, активация управления

        if (_isNpcAgressive)
        {
            _npcAnimator.SetInteger("AnimState", 1);
        }

        DeactivateNPCDialogueField();
        DeactivatePlayerDialogueField();
        PlayerMovement.ActivateControls();
        DeactivateTrigger();

        isDialogOpened = false;

    }

    public void ActivatePlayerDialogueField()
    {
        _currentPlayerDialogueField.SetActive(true);
    }

    public void ActivateNPCDialogueField()
    {
        _currentNpcDialogueField.SetActive(true);
    }

    public void DeactivatePlayerDialogueField()
    {
        _currentPlayerDialogueField.SetActive(false);
    }
    public void DeactivateNPCDialogueField()
    {
        _currentNpcDialogueField.SetActive(false);
    }

    public void DeactivateTrigger()
    {
        _currentTrigger.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            DisplayNextSentence(); 
        }
    }

    IEnumerator TypeSentence(string sentence, TextMeshProUGUI whoIsSpeaking)
    {
        string textToType = "";
        foreach (char letter in sentence.ToCharArray())
        {
            textToType += letter;
            whoIsSpeaking.SetText(textToType);
            yield return new WaitForSeconds(0.01f);
        }
    }

}
