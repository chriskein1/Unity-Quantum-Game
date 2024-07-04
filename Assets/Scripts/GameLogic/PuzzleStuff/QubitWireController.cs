using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using QubitType;
using System.Numerics;

public class QubitWireController : MonoBehaviour
{
    [SerializeField] private List<GameObject> SnapPoints = new List<GameObject>();
    [SerializeField] private InputState inputTile;
    [SerializeField] private OutputState outputTile;
    private QubitOperations qubitOperations = new QubitOperations();

    private List<Qubit> snapPointStates = new List<Qubit>();
    private Qubit inputState;
    public UnityEvent<Qubit> FinalStateChanged;

    private void Start()
    {
        SetSnapPoints();
    }
    public void SetInputState(SingleQubitStateOptions state)
    {
        inputState = qubitOperations.ConvertToQubit(state);
        inputTile.UpdateText(state); // Update the input text
        SetSnapPoints();
    }
    public void SetSnapPoints()
    {
        snapPointStates.Clear();
        snapPointStates.Add(inputState);

        foreach (GameObject p in SnapPoints)
        {
            Snap snapComp = p.GetComponent<Snap>();
            GameObject gateObject = snapComp.GetGateObject();
            Qubit state = snapPointStates[snapPointStates.Count - 1];

            qubitOperations.ApplyGateOperation(gateObject, ref state);
            snapPointStates.Add(state);

            snapComp.SetState(state);
        }

        Qubit finalState = snapPointStates[snapPointStates.Count - 1];
        outputTile.SetState(finalState);
        

        FinalStateChanged?.Invoke(finalState);
    }

    public Qubit GetInputQubit()
    {
        return inputState;
    }

    
}
