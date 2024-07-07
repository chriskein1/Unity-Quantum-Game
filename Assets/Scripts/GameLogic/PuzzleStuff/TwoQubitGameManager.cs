using System.Collections;
using UnityEngine;
using QubitType;
using System.Numerics;


public class TwoQubitGameManager : MonoBehaviour
{
    [SerializeField] private GameObject winScreen;
    [SerializeField] private SingleQubitStateOptions InputStateChoice1;
    [SerializeField] private SingleQubitStateOptions InputStateChoice2;
    [SerializeField] private SingleQubitStateOptions winStateChoiceQubit1;
    [SerializeField] private SingleQubitStateOptions winStateChoiceQubit2;
    [SerializeField] private QubitWireController qubitWireController1;
    [SerializeField] private QubitWireController qubitWireController2;
    [SerializeField] private BarChartManager barChartManager;

    private Qubit winStateQubit1;
    private Qubit winStateQubit2;
    private QubitOperations qubitOperations = new QubitOperations();
    private Qubit finalStateQubit1;
    private Qubit finalStateQubit2;

    private void OnEnable()
    {
        if (qubitWireController1 != null)
        {
            qubitWireController1.FinalStateChanged.AddListener(UpdateQubit1State);
            qubitWireController1.CNot.AddListener(ControlNode1);
            qubitWireController1.CNotRemoved.AddListener(ControlNode1);
        }
        else
        {
            Debug.LogError("QubitWireController1 reference is missing in TwoQubitGameManager.");
        }

        if (qubitWireController2 != null)
        {
            qubitWireController2.FinalStateChanged.AddListener(UpdateQubit2State);
            qubitWireController2.CNot.AddListener(ControlNode2);
            qubitWireController2.CNotRemoved.AddListener(ControlNode2);
        }
        else
        {
            Debug.LogError("QubitWireController2 reference is missing in TwoQubitGameManager.");
        }
    }

    private void OnDisable()
    {
        if (qubitWireController1 != null)
        {
            qubitWireController1.FinalStateChanged.RemoveListener(UpdateQubit1State);
            qubitWireController1.CNot.RemoveListener(ControlNode1);
            qubitWireController1.CNotRemoved.RemoveListener(ControlNode1Removed);
        }

        if (qubitWireController2 != null)
        {
            qubitWireController2.FinalStateChanged.RemoveListener(UpdateQubit2State);
            qubitWireController2.CNot.RemoveListener(ControlNode2);
            qubitWireController2.CNotRemoved.RemoveListener(ControlNode2Removed);
        }
    }

    void Awake()
    {
        winStateQubit1 = qubitOperations.ConvertToQubit(winStateChoiceQubit1);
        winStateQubit2 = qubitOperations.ConvertToQubit(winStateChoiceQubit2); 
        if (qubitWireController1 != null && qubitWireController2 != null)
        {
            qubitWireController1.SetInputState(InputStateChoice1);
            qubitWireController2.SetInputState(InputStateChoice2);
        }
    }

    private void UpdateQubit1State(Qubit finalState)
    {
        finalStateQubit1 = finalState;
        CheckWinState();
    }

    private void UpdateQubit2State(Qubit finalState)
    {
        finalStateQubit2 = finalState;
        CheckWinState();
    }

    private void ControlNode1(int qubitIndex)
    {
        // Get the current state of wire 1
        Qubit qubit1 = qubitWireController1.GetSnapPointState(qubitIndex);

        // Get gate type of snap point on wire 2
        GameObject gateObject = qubitWireController2.GetGateObject(qubitIndex);

        // Check if gate type is X
        if (gateObject == null || gateObject.tag != "XGate") return;

        // If control node is state |0>, prevent applying X gate to wire 2
        if (qubit1.Alpha.Real == 0)
        {
            // Disable the gate on wire 2
            qubitWireController2.DisableGate(qubitIndex);
        }
    }

    private void ControlNode2(int qubitIndex)
    {
        // Get the current state of wire 2
        Qubit qubit2 = qubitWireController2.GetSnapPointState(qubitIndex);

        // Get gate type of snap point on wire 1
        GameObject gateObject = qubitWireController1.GetGateObject(qubitIndex);

        // Check if gate type is X
        if (gateObject == null || gateObject.tag != "XGate") return;

        // If control node is state |0>, prevent applying X gate to wire 1
        if (qubit2.Alpha.Real == 0)
        {
            // Disable the gate on wire 1
            qubitWireController1.DisableGate(qubitIndex);
        }        
    }

    private void ControlNode1Removed(int qubitIndex)
    {
        // Enable the gate on wire 2
        qubitWireController2.EnableGate(qubitIndex);
    }

    private void ControlNode2Removed(int qubitIndex)
    {
        // Enable the gate on wire 1
        qubitWireController1.EnableGate(qubitIndex);
    }

    private void CheckWinState()
    {
        if (finalStateQubit1 != null && finalStateQubit2 != null)
        {
            UpdateBarChart(finalStateQubit1, finalStateQubit2);
            if (winStateChoiceQubit1 != SingleQubitStateOptions.NoState && winStateChoiceQubit2 != SingleQubitStateOptions.NoState &&
                winStateQubit1.IsApproximatelyEqual(finalStateQubit1) && winStateQubit2.IsApproximatelyEqual(finalStateQubit2))
            {
                StartCoroutine(WaitAndShowWinScreen());
            }
        }
    }

    private IEnumerator WaitAndShowWinScreen()
    {
        yield return new WaitForSeconds(0.02f);
        Time.timeScale = 0;
        winScreen.SetActive(true);
    }

    public bool GetWinScreenStatus()
    {
        return winScreen != null && winScreen.activeInHierarchy;
    }

    private void UpdateBarChart(Qubit finalStateQubit1, Qubit finalStateQubit2)
    {
        // Calculate probabilities for the states 00, 01, 10, and 11
        float prob00 = MagnitudeSquared(finalStateQubit1.Alpha * finalStateQubit2.Alpha);
        float prob01 = MagnitudeSquared(finalStateQubit1.Alpha * finalStateQubit2.Beta);
        float prob10 = MagnitudeSquared(finalStateQubit1.Beta * finalStateQubit2.Alpha);
        float prob11 = MagnitudeSquared(finalStateQubit1.Beta * finalStateQubit2.Beta);

        barChartManager.UpdateBarChart(prob00, prob01, prob10, prob11);
    }
    private float MagnitudeSquared(Complex c)
    {
        return (float)(c.Real * c.Real + c.Imaginary * c.Imaginary);
    }
}
