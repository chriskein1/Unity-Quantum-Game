using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuessTheGate : MonoBehaviour
{
    int guessCount = 0;
    [SerializeField] private List<CircuitManager> guessTheGateManagers;
    [SerializeField] private GameObject WinScreen;
    // [SerializeField] private List<GameObject> CheckImages;
    // [SerializeField] private List<GameObject> XImages;
    // Start is called before the first frame update

    public void CheckPuzzle()
    {
        Debug.Log("Checking Puzzle");
        bool fillInGates = CheckGatePuzzles();
        if (fillInGates)
        {
            Debug.Log("Puzzle Solved!!!!");
            if (WinScreen != null)
            {
                // stop time
                Time.timeScale = 0;
                Debug.Log("Win Screen Active");
                WinScreen.SetActive(true);
            }
        }
    }

    private bool CheckGatePuzzles()
    {
        bool isSolved = true;
        for (int i = 0; i < guessTheGateManagers.Count; i++)
        {
            if (!guessTheGateManagers[i].HasGate())
            {
                Debug.Log("No Gate in Circuit");
                isSolved = false;
                // XImages[i].SetActive(true);
                // CheckImages[i].SetActive(false);
            }
            else if (!guessTheGateManagers[i].IsWin())
            {
                Debug.Log("Puzzle Not Solved");
                // XImages[i].SetActive(true);
                // CheckImages[i].SetActive(false);
                isSolved = false;
            }
            else if (guessTheGateManagers[i].IsWin())
            {
                // CheckImages[i].SetActive(true);
                // XImages[i].SetActive(false);
            }
        }
        return isSolved;
    }
}
