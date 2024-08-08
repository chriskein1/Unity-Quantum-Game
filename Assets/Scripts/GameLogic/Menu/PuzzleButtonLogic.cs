using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleButtonLogic : MonoBehaviour
{
    [SerializeField] private GameObject[] puzzles;
    [SerializeField] private GameObject[] puzzleTexts;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button backButton;
    private int currentPuzzleIndex = 0;

    private void Start()
    {
        MoveAllPuzzlesOffScreen();
        MovePuzzleOnScreen(currentPuzzleIndex);
        UpdateButtonStates();
    }

    public void NextPuzzle()
    {
        if (currentPuzzleIndex < puzzles.Length - 1)
        {
            ClickSound();
            MovePuzzleOffScreen(currentPuzzleIndex);
            currentPuzzleIndex++;
            MovePuzzleOnScreen(currentPuzzleIndex);
            UpdateButtonStates();
            
        }
    }

    public void PreviousPuzzle()
    {
        if (currentPuzzleIndex > 0)
        {
            ClickSound();
            MovePuzzleOffScreen(currentPuzzleIndex);
            currentPuzzleIndex--;
            MovePuzzleOnScreen(currentPuzzleIndex);
            UpdateButtonStates();
        }
    }

    private void MoveAllPuzzlesOffScreen()
    {
        for (int i = 0; i < puzzles.Length; i++)
        {
            MovePuzzleOffScreen(i);
        }
    }

    private void MovePuzzleOffScreen(int index)
    {
        // Move the puzzle first
        puzzles[index].transform.position += new Vector3(0, (index + 1) * 50, 0);
        puzzleTexts[index].transform.position += new Vector3(0, (index + 1) * 50, 0);

        CircuitManager circuitManager = puzzles[index].GetComponentInChildren<CircuitManager>();

        if (circuitManager != null)
        {
            foreach (var snapPoints in circuitManager.GetSnapPoints())
            {
                foreach (var snapPoint in snapPoints)
                {
                    Snap snapComponent = snapPoint.GetComponent<Snap>();
                    if (snapComponent != null && snapComponent.GetGateOnSnapPoint() != null)
                    {
                        GameObject gate = snapComponent.GetGateOnSnapPoint();
                        // Move the gate after the puzzle has been moved
                        gate.transform.position += new Vector3(0, (index + 1) * 50, 0);
                    }
                }
            }
        }
    }

    private void MovePuzzleOnScreen(int index)
    {
        // Move the puzzle first
        puzzles[index].transform.position -= new Vector3(0, (index + 1) * 50, 0);
        puzzleTexts[index].transform.position -= new Vector3(0, (index + 1) * 50, 0);

        CircuitManager circuitManager = puzzles[index].GetComponentInChildren<CircuitManager>();
        if (circuitManager != null)
        {
            foreach (var snapPoints in circuitManager.GetSnapPoints())
            {
                foreach (var snapPoint in snapPoints)
                {
                    Snap snapComponent = snapPoint.GetComponent<Snap>();
                    if (snapComponent != null && snapComponent.GetGateOnSnapPoint() != null)
                    {
                        GameObject gate = snapComponent.GetGateOnSnapPoint();
                        Debug.Log($"-----------------------------------------------{gate.name}");
                        // Move the gate after the puzzle has been moved
                        gate.transform.position -= new Vector3(0, (index + 1) * 50, 0);
                    }
                }
            }
        }
    }

    private void UpdateButtonStates()
    {
        if (currentPuzzleIndex == 0)
        {
            backButton.gameObject.SetActive(false);
        }
        else
        {
            backButton.gameObject.SetActive(true);
        }

        if (currentPuzzleIndex == puzzles.Length - 1)
        {
            nextButton.gameObject.SetActive(false);
        }
        else
        {
            nextButton.gameObject.SetActive(true);
        }
    }

    private void ClickSound()
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
