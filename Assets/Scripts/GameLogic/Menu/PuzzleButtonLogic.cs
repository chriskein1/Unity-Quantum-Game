using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleButtonLogic : MonoBehaviour
{
    [SerializeField] private GameObject[] puzzles;
    [SerializeField] private GameObject[] puzzleTexts;
    private int currentPuzzleIndex = 0;

    private Dictionary<int, List<GameObject>> puzzleGates;

    private void Start()
    {
        puzzleGates = new Dictionary<int, List<GameObject>>();
        UpdatePuzzleVisibility();
    }

    public void NextPuzzle()
    {
        if (currentPuzzleIndex < puzzles.Length - 1)
        {
            UpdatePuzzleVisibility();
            SaveCurrentPuzzleGates();
            currentPuzzleIndex++;
            UpdatePuzzleVisibility();
            RestoreCurrentPuzzleGates();
        }
    }

    public void PreviousPuzzle()
    {
        if (currentPuzzleIndex > 0)
        {
            UpdatePuzzleVisibility();
            SaveCurrentPuzzleGates();
            currentPuzzleIndex--;
            RestoreCurrentPuzzleGates();
            UpdatePuzzleVisibility();
        }
    }

    private void UpdatePuzzleVisibility()
    {
        for (int i = 0; i < puzzles.Length; i++)
        {
            bool isActive = (i == currentPuzzleIndex);
            puzzles[i].SetActive(isActive);
            puzzleTexts[i].SetActive(isActive);
        }
    }

    private void SaveCurrentPuzzleGates()
    {
        List<GameObject> gates = new List<GameObject>();

        foreach (string tag in new string[] { "XGate", "YGate", "ZGate", "HGate", "State0Circle", "State0Square", "State1Circle", "State1Square", "NegativeState1Circle", "NegativeState1Square", "NegativeState0Circle", "NegativeState0Square" })
        {
            GameObject[] taggedGates = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject gate in taggedGates)
            {
                if (gate.activeSelf)
                {
                    gates.Add(gate);
                    gate.SetActive(false);
                }
            }
        }

        if (puzzleGates.ContainsKey(currentPuzzleIndex))
        {
            puzzleGates[currentPuzzleIndex] = gates;
        }
        else
        {
            puzzleGates.Add(currentPuzzleIndex, gates);
        }
    }

    private void RestoreCurrentPuzzleGates()
    {
        if (puzzleGates.ContainsKey(currentPuzzleIndex))
        {
            List<GameObject> gates = puzzleGates[currentPuzzleIndex];
            foreach (GameObject gate in gates)
            {
                gate.SetActive(true);
                Drag dragcomp = gate.GetComponent<Drag>();
                if (dragcomp != null)
                {
                    dragcomp.Unsnap();
                }
            }
        }
    }
}
