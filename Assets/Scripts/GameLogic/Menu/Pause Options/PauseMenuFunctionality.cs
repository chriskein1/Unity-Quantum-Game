using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class handles the functionality of the pause menu in the game.
/// </summary>
public class PauseMenuFunctionality : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenu; //holds reference to pause menu
    [SerializeField] private GameObject OptionsMenu; //holds reference to optionsMenu

    private bool isTransitioning = true;    //checks if we are in transition animation

    void Start()
    {
        PauseMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        StartCoroutine(WaitForTransition()); //waits for transition to be over
    }

    void Update()
    {
        if (isTransitioning) return;

        if (Input.GetKeyDown(KeyCode.Escape)) //if we press escape
        {
            if (PauseMenu.activeSelf || OptionsMenu.activeSelf) //if a menu is open, close all menus
            {
                CloseAllMenus();
            }
            else
            {
                TogglePauseMenu();  //open pause menu
            }
        }
    }

    /// <summary>
    /// Toggles the pause menu on and off.
    /// </summary>
    public void TogglePauseMenu()
    {
        PauseMenu.SetActive(!PauseMenu.activeSelf);

        if (PauseMenu.activeSelf) //if pause menu is active 
        {
            Time.timeScale = 0;   //freezes time 
        }
        else
        {
            Time.timeScale = 1;  //unfreezes time
        }
    }

    public void CloseAllMenus()
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

    /// <summary>
    /// Waits for the transition to complete before allowing the pause menu to be opened.
    /// </summary>
    IEnumerator WaitForTransition()
    {
        MenuButtonFunctionality menuScript = GetComponent<MenuButtonFunctionality>();
        yield return new WaitForSeconds(menuScript.transitionTime);
        isTransitioning = false;
    }

    public void ClickSound()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.Play("ButtonClick");
        }
        else
        {
            Debug.LogError("AudioManager instance not found!");
        }
    }
}
