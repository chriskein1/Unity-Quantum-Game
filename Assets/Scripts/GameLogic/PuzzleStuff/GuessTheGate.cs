using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class GuessTheGate : MonoBehaviour
{
    int?[] correct;
    [SerializeField] private List<GatePuzzle> GatePuzzles;
    [SerializeField] private Button NextLevelButton;
    private List<List<CircuitManager>> guessTheGateManagers = new List<List<CircuitManager>>();
    [SerializeField] private List<GameObject> CheckImages;
    [SerializeField] private List<GameObject> XImages;
    [SerializeField] private TextMeshProUGUI ScoreText;
    private TextMeshProUGUI buttonText = null;
    private Image buttonImage= null;
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
            if (NextLevelButton != null)
            {
                int count = getScore();
                Debug.Log($"Score: {count} out of {correct.Length}");
                // stop time
                Time.timeScale = 0;
                Debug.Log("Win Screen Active");

                // Change button text color to black
                if(buttonText == null)
                buttonText = NextLevelButton.GetComponentInChildren<TextMeshProUGUI>();


                if (buttonText != null)
                {
                    buttonText.color = Color.black;
                }

                // Change button background color to yellow
                ColorBlock cb = NextLevelButton.colors;
                cb.normalColor = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, cb.normalColor.a);
                cb.highlightedColor = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, cb.highlightedColor.a);
                cb.pressedColor = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, cb.pressedColor.a);
                cb.selectedColor = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, cb.selectedColor.a);
                NextLevelButton.colors = cb;

                // Change button image color to white
                 buttonImage = NextLevelButton.GetComponent<Image>();
                if (buttonImage != null)
                {
                    buttonImage.color = Color.white;
                }
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
        if (ScoreText != null)
            updateScore();
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

    public void updateScore()
    {
        int count = getScore();
        ScoreText.text = $"Score: {count} out of {correct.Length}";
    }
}
