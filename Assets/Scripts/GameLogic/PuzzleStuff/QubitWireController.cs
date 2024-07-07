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
    public QubitOperations qubitOperations = new QubitOperations();

    private List<Qubit> snapPointStates = new List<Qubit>();
    private Qubit inputState;
    public UnityEvent<Qubit> FinalStateChanged;
    public UnityEvent<int> CNot;
    public UnityEvent<int> CNotRemoved;

    private void OnEnable()
    {
        // Listener for Control node event in Qubit Operations
        qubitOperations.Control.AddListener(OnControl);
        qubitOperations.ControlRemoved.AddListener(OnControlRemoved);
    }

    private void OnDisable()
    {
        qubitOperations.Control.RemoveListener(OnControl);
        qubitOperations.ControlRemoved.RemoveListener(OnControlRemoved);
    }

    private void OnControl(int qubitIndex)
    {
        CNot?.Invoke(qubitIndex);
    }

    private void OnControlRemoved(int qubitIndex)
    {
        CNotRemoved?.Invoke(qubitIndex);
    }

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

        for (int i = 0; i < SnapPoints.Count; i++)
        {
            Snap snapComp = SnapPoints[i].GetComponent<Snap>();
            GameObject gateObject = snapComp.GetGateObject();
            Qubit state = snapPointStates[snapPointStates.Count - 1];

            qubitOperations.ApplyGateOperation(snapComp, ref state, i);
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

    // Function to return the state at a given index
    public Qubit GetSnapPointState(int qubitIndex)
    {
        return snapPointStates[qubitIndex];
    }

    public void DisableGate(int qubitIndex)
    {
        SnapPoints[qubitIndex].GetComponent<Snap>().DeactivateGate();
    }

    public void EnableGate(int qubitIndex)
    {
        SnapPoints[qubitIndex].GetComponent<Snap>().ActivateGate();
    }

    public GameObject GetGateObject(int qubitIndex)
    {
        return SnapPoints[qubitIndex].GetComponent<Snap>().GetGateObject();
    }
    
}
