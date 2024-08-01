using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// This class handles the functionality for the menu buttons,
/// including loading levels, quitting the application, and navigating to the main menu.
/// </summary>
public class MenuButtonFunctionality : MonoBehaviour
{
    public Animator transition; //this is the animation for when we transition to another scene
    public float transitionTime = 1.0f; //duration of animation

    /// <summary>
    /// Initiates the loading of a specified level.
    /// </summary>
    public void PlayLevel(int levelIndex)
    {
        ClickSound();  
        StartCoroutine(LoadLevel(levelIndex)); //this calls the loadLevel method 
        
    }
    /// <summary>
    /// Loads the next level in the build index.
    /// </summary>
    public void LoadNextLevel()
    {
        Time.timeScale = 1.0f;
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1)); //loads next scene in according to build index
    }
    /// <summary>
    /// Coroutine to handle the transition animation and scene loading.
    /// </summary>
    IEnumerator LoadLevel(int levelIndex)
    {
        ClickSound();
        Debug.Log("Going to NextLevel");
        transition.SetTrigger("Start"); //calls trigger for the animation
        yield return new WaitForSeconds(transitionTime);  //this waits transitionTime amount of seconds 
        SceneManager.LoadScene(levelIndex); //loads selected scene 

    }
    /// <summary>
    /// Quits the application.
    /// </summary>
    public void Quit()
    {
        {
            ClickSound();
            Application.Quit();

        }
    }
    /// <summary>
    /// Navigates to the main menu.
    /// </summary>
    public void GoToMainMenu()
    {
        if (Time.deltaTime == 0)
            Time.timeScale = 1;
        transition.SetTrigger("Start"); //calls trigger for the animation
        StartCoroutine(LoadLevel(0));
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

