using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuessTheGate : MonoBehaviour
{
    int guessCount = 0;
    [SerializeField] private List<GatePuzzle> GatePuzzles;
    [SerializeField] private GameObject WinScreen;
    private List<List<CircuitManager>> guessTheGateManagers = new List<List<CircuitManager>>();
    // [SerializeField] private List<GameObject> CheckImages;
    // [SerializeField] private List<GameObject> XImages;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize guessTheGateManagers
        for (int i = 0; i < GatePuzzles.Count; i++)
        {
            guessTheGateManagers.Add(GatePuzzles[i].GetCircuitManagers());
        }
    }

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
        for (int i = 0; i < GatePuzzles.Count; i++)
        {
            Debug.Log("Checking gate puzzle " + i);
            List<bool> correctGuesses = new List<bool>();

            for (int j = 0; j < guessTheGateManagers[i].Count; j++)
            {
                correctGuesses.Add(false);
                Debug.Log("Checking puzzle " + i + j);
                if (!guessTheGateManagers[i][j].HasGate())
                {
                    Debug.Log("No Gate in Circuit");
                    isSolved = false;
                    // XImages[i].SetActive(true);
                    // CheckImages[i].SetActive(false);
                }
                else if (!guessTheGateManagers[i][j].IsWin())
                {
                    Debug.Log("Puzzle Not Solved");
                    // XImages[i].SetActive(true);
                    // CheckImages[i].SetActive(false);
                    isSolved = false;
                }
                else if (guessTheGateManagers[i][j].IsWin())
                {
                    correctGuesses[j] = true;
                    // CheckImages[i].SetActive(true);
                    // XImages[i].SetActive(false);
                }
            }
            if (!correctGuesses.Contains(false))
            {
                Debug.Log("Puzzle Solved");
                // CheckImages[i].SetActive(true);
                // XImages[i].SetActive(false);
            }
        }
        return isSolved;
    }
}
