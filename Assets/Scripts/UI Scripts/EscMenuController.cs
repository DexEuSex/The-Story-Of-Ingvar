using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscMenuController : MonoBehaviour
{
    [SerializeField] private GameObject _escapeMenu;
    public static bool isMenuActive;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && _escapeMenu.activeSelf == false)
        {
            isMenuActive = true;
            _escapeMenu.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && _escapeMenu.activeSelf == true)
        {
            isMenuActive = false;
            _escapeMenu.SetActive(false);
        }
    }


}
