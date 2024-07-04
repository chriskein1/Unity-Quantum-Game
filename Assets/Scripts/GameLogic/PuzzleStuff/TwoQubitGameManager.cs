using System.Collections;
using UnityEngine;
using QubitType;
using System.Numerics;


public class TwoQubitGameManager : MonoBehaviour
{
    [SerializeField] private GameObject winScreen;
    [SerializeField] private WinStateOptions winStateChoiceQubit1;
    [SerializeField] private WinStateOptions winStateChoiceQubit2;
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
        }
        else
        {
            Debug.LogError("QubitWireController1 reference is missing in TwoQubitGameManager.");
        }

        if (qubitWireController2 != null)
        {
            qubitWireController2.FinalStateChanged.AddListener(UpdateQubit2State);
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
        }

        if (qubitWireController2 != null)
        {
            qubitWireController2.FinalStateChanged.RemoveListener(UpdateQubit2State);
        }
    }

    void Awake()
    {
        winStateQubit1 = qubitOperations.ConvertToQubit(winStateChoiceQubit1);
        winStateQubit2 = qubitOperations.ConvertToQubit(winStateChoiceQubit2);
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

    private void CheckWinState()
    {
        if (finalStateQubit1 != null && finalStateQubit2 != null)
        {
            UpdateBarChart(finalStateQubit1, finalStateQubit2);
            if (winStateChoiceQubit1 != WinStateOptions.NoWinState && winStateChoiceQubit2 != WinStateOptions.NoWinState &&
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
