using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuFunctionality : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject OptionsMenu;
    private bool isMenuOpen = false;

    void Start()
    {
        PauseMenu.SetActive(false);
        OptionsMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseMenu.activeSelf || OptionsMenu.activeSelf)
            {
                CloseAllMenus();
            }
            else
            {
                TogglePauseMenu();
            }
        }
    }

    private void TogglePauseMenu()
    {
        PauseMenu.SetActive(!PauseMenu.activeSelf);

        
        if (PauseMenu.activeSelf)
        {
            Time.timeScale = 0;  
        }
        else
        {
            Time.timeScale = 1;  
        }
    }

    private void CloseAllMenus()
    {
        PauseMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        Time.timeScale = 1;  
    }

    public void OpenOptionsMenu()
    {
        PauseMenu.SetActive(false);
        OptionsMenu.SetActive(true);
    }
}
    




