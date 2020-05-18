using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private GameObject _escapeMenu;
    [SerializeField] private GameObject _mainButtons;
    [SerializeField] private GameObject _optionButtons;

    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _optionsButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _exitButton;

    [SerializeField] private Button _switchToWASDButton;
    [SerializeField] private Button _switchToArrowsButton;
    [SerializeField] private Button _audioToggleActiveButton;
    [SerializeField] private Button _audioToggleInactiveButton;
    [SerializeField] private Button _backToMenuButton;



    void Awake()
    {
        _continueButton.onClick.AddListener(OnContinueButtonClick);
        _optionsButton.onClick.AddListener(OnOptionsButtonClick);
        _backToMenuButton.onClick.AddListener(OnBackToMenuButtonClick);
        _switchToWASDButton.onClick.AddListener(OnSwitchToWASDButtonClick);
        _switchToArrowsButton.onClick.AddListener(OnSwitchToArrowsButtonClick);
        _audioToggleActiveButton.onClick.AddListener(OnAudioToggleAciveButtonClick);
        _audioToggleInactiveButton.onClick.AddListener(OnAudioToggleInactiveButtonClick);
    }
    
   


    private void OnContinueButtonClick()
    {
        EscMenuController.isMenuActive = false;
        _escapeMenu.SetActive(false);
    }

    private void OnRestartButtonClick()
    {

    }

    private void OnOptionsButtonClick()
    {
        _mainButtons.SetActive(false);
        _optionButtons.SetActive(true);
    }

    private void OnExitButtonClick()
    {
        // this code will make the game to shut down
    }

    private void OnBackToMenuButtonClick()
    {
        _mainButtons.SetActive(true);
        _optionButtons.SetActive(false);
    }

    private void OnSwitchToWASDButtonClick()
    {
        _switchToWASDButton.interactable = false;
        _switchToArrowsButton.interactable = true;
    }

    private void OnSwitchToArrowsButtonClick()
    {
        _switchToWASDButton.interactable = true;
        _switchToArrowsButton.interactable = false;
    }

    private void OnAudioToggleAciveButtonClick()
    {
        _audioToggleActiveButton.interactable = false;
        _audioToggleInactiveButton.interactable = true;
    }

    private void OnAudioToggleInactiveButtonClick()
    {
        _audioToggleActiveButton.interactable = true;
        _audioToggleInactiveButton.interactable = false;
    }
}
