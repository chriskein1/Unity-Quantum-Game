using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuessTheGate : MonoBehaviour
{
    int?[] correct;
    [SerializeField] private List<GatePuzzle> GatePuzzles;
    [SerializeField] private GameObject WinScreen;
    private List<List<CircuitManager>> guessTheGateManagers = new List<List<CircuitManager>>();
    [SerializeField] private List<GameObject> CheckImages;
    [SerializeField] private List<GameObject> XImages;
    bool[] prevSolvedPuzzles;

    // Start is called before the first frame update
    void Start()
    {
        prevSolvedPuzzles = new bool[GatePuzzles.Count];
        correct = new int?[GatePuzzles.Count];
        // Initialize guessTheGateManagers
        for (int i = 0; i < GatePuzzles.Count; i++)
        {
            guessTheGateManagers.Add(GatePuzzles[i].GetCircuitManagers());
            prevSolvedPuzzles[i] = false;
            correct[i] = null;
        }

        // By default, no image is active
        for (int i = 0; i < CheckImages.Count; i++)
        {
            CheckImages[i].SetActive(false);
            XImages[i].SetActive(false);
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
                int count = getScore();
                Debug.Log($"Score: {count} out of {correct.Length}");
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
                if (!guessTheGateManagers[i][j].HasGate() && !prevSolvedPuzzles[i])
                {
                    Debug.Log("No Gate in Circuit");
                    isSolved = false;
                    CheckImages[i].SetActive(false);
                }
                else if (!guessTheGateManagers[i][j].IsWin())
                {
                    Debug.Log("Puzzle Not Solved");
                    XImages[i].SetActive(true);
                    CheckImages[i].SetActive(false);
                    isSolved = false;
                    prevSolvedPuzzles[i] = true;
                    if (correct[i] == null)
                    {
                        correct[i] = 0;
                    }
                }
                else if (guessTheGateManagers[i][j].IsWin())
                {
                    correctGuesses[j] = true;
                    CheckImages[i].SetActive(true);
                    XImages[i].SetActive(false);
                    prevSolvedPuzzles[i] = true;
                }
                // if previously solved but now missing a gate:
                if (prevSolvedPuzzles[i] && !guessTheGateManagers[i][j].HasGate())
                {
                    Debug.Log("Puzzle Not Solved");
                    XImages[i].SetActive(true);
                    CheckImages[i].SetActive(false);
                    isSolved = false;
                    prevSolvedPuzzles[i] = false;
                    correctGuesses[j] = false;
                    if (correct[i] == null)
                    {
                        correct[i] = 0;
                    }
                }
            }
            if (!correctGuesses.Contains(false))
            {
                Debug.Log("Puzzle Solved");
                CheckImages[i].SetActive(true);
                XImages[i].SetActive(false);
                if (correct[i] == null)
                {
                    correct[i] = 1;
                }
            }
        }
        return isSolved;
    }

    public int getScore()
    {
        int count = 0;
        foreach (int? i in correct)
        {
            if (i == 1)
            {
                count++;
            }
        }
        return count;
    }
}
