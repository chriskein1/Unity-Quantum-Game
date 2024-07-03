using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using QubitType;
using System.Numerics;
using System;

public enum WinStateOptions
{
    State0,  // |0⟩
    State1,  // |1⟩
    SuperpositionPlus,  // |+⟩ = 1/√2 (|0⟩ + |1⟩)
    SuperpositionMinus, // |−⟩ = 1/√2 (|0⟩ - |1⟩)
    ComplexSuperposition1, // 1/√3 |0⟩ + √(2/3) |1⟩
    ComplexSuperposition2, // 1/2 |0⟩ + √3/2 |1⟩
    NoWinState // No win state
}

public enum StartingStateOptions
{
    State0,  // |0⟩
    State1,  // |1⟩
}

public class GamePuzzleController : MonoBehaviour
{
    [SerializeField] private List<GameObject> SnapPoints = new List<GameObject>();
    [SerializeField] private StartingStateOptions startingStateChoice;
    [SerializeField] private WinStateOptions winStateChoice;
    [SerializeField] private InputState inputTile;
    [SerializeField] private OutputState outputTile;
    [SerializeField] private GameObject WinScreen;
    [SerializeField] private BarChartManager barChartManager;

    private List<Qubit> snapPointStates = new List<Qubit>();
    private Qubit inputState;
    private Qubit winState;
    public UnityEvent OutputChanged; // Event for qubit controller

    // Start is called before the first frame update
    void Start()
    {
        // Convert input state and win state to qubit objects
        inputState = ConvertToQubit(startingStateChoice);
        winState = ConvertToQubit(winStateChoice);

        // Initialize the snap point states
        SetSnapPoints();
    }

    public void SetSnapPoints()
    {
        // Clear list
        snapPointStates.Clear();

        // Each snap point's state will match the state of what comes before it, and then the gate operation will affect that state

        // First state is the input tile's state
        snapPointStates.Add(inputState);
        foreach (GameObject p in SnapPoints)
        {
            Snap snapComp = p.GetComponent<Snap>();

            // Get the gate object on the snap point
            GameObject gateObject = snapComp.GetGateObject();

            // Get the state of last snap point
            Qubit state = snapPointStates[snapPointStates.Count - 1]; // First state is the input tile's state

            // Do a gate operation on the current state
            GateOperation(gateObject, ref state);

            // Add the new state to the list
            snapPointStates.Add(state);

            // Update snap point state
            Debug.Log("Setting state");
            snapComp.SetState(state);
        }

        Qubit finalState = snapPointStates[snapPointStates.Count - 1];

        Debug.Log("input State: " + inputState.ToString());
        Debug.Log("Output State: " + finalState.ToString());
        outputTile.SetState(finalState);
        // Update the bar chart with the final state probabilities
        UpdateBarChart(finalState);

        if (winStateChoice != WinStateOptions.NoWinState && winState.IsApproximatelyEqual(finalState))
        {
            StartCoroutine(WaitAndShowWinScreen());
        }


        IEnumerator WaitAndShowWinScreen()
        {
            yield return new WaitForSeconds(0.02f);
            // Set time to 0
            Time.timeScale = 0;
            WinScreen.SetActive(true);
        }
    }

    // Gate operation function
    private void GateOperation(GameObject gateObject, ref Qubit state)
    {
        // Return if there is no gate on the snap point
        if (gateObject == null)
        {
            Debug.Log("No gate on this snap point");
            return;
        }

        // Gate operations based on the gate object's tag
        switch (gateObject.tag)
        {
            case "XGate":
                // X gate operation
                Debug.Log("X gate operation");
                state = QuantumGates.ApplyPauliX(state);
                break;

            case "YGate":
                // Y gate operation
                Debug.Log("Y gate operation");

                // Bit & phase flip
                state = QuantumGates.ApplyPauliY(state);
                break;

            case "ZGate":
                // Z gate operation
                Debug.Log("Z gate operation");
                state = QuantumGates.ApplyPauliZ(state);
                break;

            case "HGate":
                Debug.Log("H gate operation");
                // H applied flag flipped
                state = QuantumGates.ApplyHadamard(state);
                break;
        }
    }

    public StartingStateOptions GetInputState()
    {
        return startingStateChoice;
    }

    public Qubit GetInputQubit()
    {
        return inputState;
    }

    public WinStateOptions GetWinState()
    {
        return winStateChoice;
    }

    public bool GetWinScreenStatus()
    {
        if (WinScreen == null)
        {
            return false;
        }
        return WinScreen.activeInHierarchy;
    }

    private Qubit ConvertToQubit(StartingStateOptions state)
    {
        switch (state)
        {
            case StartingStateOptions.State0:
                return new Qubit(new Complex(1, 0), new Complex(0, 0));
            case StartingStateOptions.State1:
                return new Qubit(new Complex(0, 0), new Complex(1, 0));
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private Qubit ConvertToQubit(WinStateOptions state)
    {
        switch (state)
        {
            case WinStateOptions.State0:
                return new Qubit(new Complex(1, 0), new Complex(0, 0));
            case WinStateOptions.State1:
                return new Qubit(new Complex(0, 0), new Complex(1, 0));
            case WinStateOptions.SuperpositionPlus:
                return new Qubit(new Complex(1 / Mathf.Sqrt(2), 0), new Complex(1 / Mathf.Sqrt(2), 0));
            case WinStateOptions.SuperpositionMinus:
                return new Qubit(new Complex(1 / Mathf.Sqrt(2), 0), new Complex(-1 / Mathf.Sqrt(2), 0));
            case WinStateOptions.ComplexSuperposition1:
                return new Qubit(new Complex(1 / Mathf.Sqrt(3), 0), new Complex(Mathf.Sqrt(2 / 3f), 0));
            case WinStateOptions.ComplexSuperposition2:
                return new Qubit(new Complex(1 / 2f, 0), new Complex(Mathf.Sqrt(3) / 2f, 0));
            case WinStateOptions.NoWinState:
                return default; // No win state
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void UpdateBarChart(Qubit finalState)
    {
        // Calculate the probabilities
        float probability0 = (float)(finalState.Alpha.Magnitude * finalState.Alpha.Magnitude);
        float probability1 = (float)(finalState.Beta.Magnitude * finalState.Beta.Magnitude);

        // Update the bar chart
        barChartManager.SetSliderValue(0, probability0);
        barChartManager.SetSliderValue(1, probability1);
    }
}
