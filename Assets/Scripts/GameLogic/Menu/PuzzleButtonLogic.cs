using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleButtonLogic : MonoBehaviour
{
    [SerializeField] private GameObject[] puzzles;
    [SerializeField] private GameObject[] puzzleTexts;
    private int currentPuzzleIndex = 0;

    private void Start()
    {
        MoveAllPuzzlesOffScreen();
        MovePuzzleOnScreen(currentPuzzleIndex);
    }

    public void NextPuzzle()
    {
        if (currentPuzzleIndex < puzzles.Length - 1)
        {
            MovePuzzleOffScreen(currentPuzzleIndex);
            currentPuzzleIndex++;
            MovePuzzleOnScreen(currentPuzzleIndex);
        }
    }

    public void PreviousPuzzle()
    {
        if (currentPuzzleIndex > 0)
        {
            MovePuzzleOffScreen(currentPuzzleIndex);
            currentPuzzleIndex--;
            MovePuzzleOnScreen(currentPuzzleIndex);
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
}
