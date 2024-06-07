using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputTile : MonoBehaviour
{
    [Header("Starting Qubit State")]
    [SerializeField] private int state;
    [SerializeField] private bool PositiveState;
    [SerializeField] private bool SuperPosition;
    [SerializeField] private ChangeState blochSphere;
    private TextMeshPro text;

    void Start()
    {
        text = GetComponentInChildren<TextMeshPro>();
        UpdateText();
    }
    // updates sprites text 
    void UpdateText()
    {
        string sign = PositiveState ? "": "-";
        text.text = $"{sign}|{state}>";
    }

    // Function to return the state and its sign
    public (int, bool, bool) GetState()
    {
        return (state, PositiveState, SuperPosition);
    }

    // Function to set the state and its sign
    public void SetState((int, bool, bool) newState)
    {
        state = newState.Item1;
        PositiveState = newState.Item2;
        UpdateText();
        UpdateBlochSphere();
    }

    public void UpdateBlochSphere()
    {
        if (blochSphere == null)
        {
            Debug.LogWarning("BlochSphere reference is not set.");
            return;
        }

        if (state == 0)
        {
            if (PositiveState)
            {
                blochSphere.SetZeroState();
                print("Zero State");
            }
            else
            {
                blochSphere.SetNegativeState();
                print("Negative Zero State");
            }
        }
        else if (state == 1)
        {
            if (PositiveState)
            {
                blochSphere.SetOneState();
                print("One State");
            }
            else
            {
                blochSphere.SetPositiveState();
                print("Negative One State");
            }

        }
    }
}