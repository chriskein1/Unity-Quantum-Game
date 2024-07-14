using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using QubitType;
public class GuessTheOutput : MonoBehaviour
{
    int guessCount = 0;
    [SerializeField] private List<OutputPuzzle> OutputPuzzles;
    [SerializeField] private GameObject WinScreen;
    private List<List<Snap>> OutputGuesses = new List<List<Snap>>();
    private List<List<SingleQubitStateOptions>> CorrectOutputs = new List<List<SingleQubitStateOptions>>();
    // [SerializeField] private List<GameObject> CheckImages;
    // [SerializeField] private List<GameObject> XImages;
    
    void Start()
    {
        // Initialize OutputGuesses and CorrectOutputs
        for (int i = 0; i < OutputPuzzles.Count; i++)
        {
            OutputGuesses.Add(OutputPuzzles[i].GetOutputSnaps());
            CorrectOutputs.Add(OutputPuzzles[i].GetCorrectOutput());
        }
    }

    public void CheckPuzzle()
    {
        Debug.Log("Checking Puzzle");
        bool fillInOutputs = CheckOutputPuzzles();
        if (fillInOutputs)
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

    private bool CheckOutputPuzzles()
    {
        bool isSolved = true;
        Debug.Log("Puzzles: " + OutputPuzzles.Count);
        // Compare tag of output guess with correct output enum to string
        for (int i = 0; i < OutputPuzzles.Count; i++)
        {
            Debug.Log("Checking output puzzle " + i);
            if (OutputGuesses[i].Count != CorrectOutputs[i].Count)
            {
                Debug.Log("Incorrect number of outputs");
                isSolved = false;
            }
            else
            {
                List<bool> correctGuesses = new List<bool>();
                for (int j = 0; j < OutputGuesses[i].Count; j++)
                {
                    correctGuesses.Add(false);
                    GameObject guessObject = OutputGuesses[i][j].GetGateObject();
                    if (guessObject == null)
                    {
                        Debug.Log("No output guess");
                        isSolved = false;
                        continue;
                    }
                    string guessTag = guessObject.tag;
                    string correctOutput = CorrectOutputs[i][j].ToString();
                    Debug.Log("Guess: " + guessTag + " Correct: " + correctOutput);
                    Debug.Log("Correct output contains guess: " + guessTag.Contains(correctOutput));
                    Debug.Log("Guess contains Circle: " + guessTag.Contains("Circle"));
                    
                    if (j == 0 && guessTag.Contains(correctOutput) && guessTag.Contains("Circle"))
                    {
                        Debug.Log("Correct for q0!");
                        correctGuesses[0] = true;
                    }
                    else if (j == 1 && guessTag.Contains(correctOutput) && guessTag.Contains("Square"))
                    {
                        Debug.Log("Correct for q1!");
                        correctGuesses[1] = true;
                    }
                    else
                    {
                        Debug.Log("Incorrect somewhere...");
                        isSolved = false;
                        // CheckImages[i].SetActive(false);
                        // XImages[i].SetActive(true);
                    }
                }
                if (!correctGuesses.Contains(false))
                {
                    Debug.Log("Correct output for qubits!");
                    // CheckImages[i].SetActive(true);
                    // XImages[i].SetActive(false);
                }
            }
        }

        return isSolved;        
    }
}
