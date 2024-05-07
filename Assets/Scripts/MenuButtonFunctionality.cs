using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuButtonFunctionality : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void Quit()
    {
        {
            Application.Quit();
        }
    }
    public void GoToMainMenu()
    {
        if (Time.deltaTime == 0)
            Time.timeScale = 1; 
        SceneManager.LoadScene("MainMenu");
    }
}

