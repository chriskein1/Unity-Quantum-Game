using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputTile : MonoBehaviour
{
    [Header("Starting Qubit State")]
    [SerializeField] private int state;
    [SerializeField] private bool PositiveState;
    private TextMeshPro text;

    void Start()
    {
        text = GetComponentInChildren<TextMeshPro>();
        UpdateText();
    }

    void UpdateText()
    {
        string sign = PositiveState ? "": "-";
        text.text = $"|{sign}{state}>";
    }

    // Function to return the state and its sign
    public (int, bool) GetState()
    {
        return (state, PositiveState);
    }

    // Function to set the state and its sign
    public void SetState((int, bool) newState)
    {
        state = newState.Item1;
        PositiveState = newState.Item2;
        UpdateText();
    }
}