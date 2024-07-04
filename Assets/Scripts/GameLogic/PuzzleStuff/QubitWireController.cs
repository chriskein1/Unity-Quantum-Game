using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using QubitType;
using System.Numerics;

public class QubitWireController : MonoBehaviour
{
    [SerializeField] private List<GameObject> SnapPoints = new List<GameObject>();
    [SerializeField] private StartingStateOptions startingStateChoice;
    [SerializeField] private InputState inputTile;
    [SerializeField] private OutputState outputTile;
    private QubitOperations qubitOperations = new QubitOperations();

    private List<Qubit> snapPointStates = new List<Qubit>();
    private Qubit inputState;
    public UnityEvent<Qubit> FinalStateChanged;
    void Start()
    {
        inputState = qubitOperations.ConvertToQubit(startingStateChoice);
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

    public StartingStateOptions GetInputState()
    {
        return startingStateChoice;
    }
    public Qubit GetInputQubit()
    {
        return inputState;
    }

    
}
