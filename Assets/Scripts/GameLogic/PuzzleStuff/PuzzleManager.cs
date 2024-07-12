using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    int guessCount = 0;
    [SerializeField] private List<CircuitManager> qubitCircuitManagers;
    [SerializeField] private GameObject WinScreen;
    // [SerializeField] private List<GameObject> CheckImages;
    // [SerializeField] private List<GameObject> XImages;
    // Start is called before the first frame update

    public void CheckPuzzle()
    {
        Debug.Log("Checking Puzzle");
        bool isSolved = true;
        for (int i = 0; i < qubitCircuitManagers.Count; i++)
        {
            if (!qubitCircuitManagers[i].HasGate())
            {
                Debug.Log("No Gate in Circuit");
                isSolved = false;
                // XImages[i].SetActive(true);
                // CheckImages[i].SetActive(false);
            }
            else if (!qubitCircuitManagers[i].IsWin())
            {
                Debug.Log("Puzzle Not Solved");
                // XImages[i].SetActive(true);
                // CheckImages[i].SetActive(false);
                isSolved = false;
            }
            else if (qubitCircuitManagers[i].IsWin())
            {
                // CheckImages[i].SetActive(true);
                // XImages[i].SetActive(false);
            }
        }
        if (isSolved)
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
}
