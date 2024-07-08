using System.Collections;
using UnityEngine;
using QubitType;

public class OneQubitGameManager : MonoBehaviour
{
    [SerializeField] private GameObject winScreen;
    [SerializeField] private SingleQubitStateOptions startingStateChoice;
    [SerializeField] private SingleQubitStateOptions winStateChoice;
    [SerializeField] private QubitWireController qubitWireController;
    [SerializeField] private BarChartManager barChartManager;
    private Qubit winState;
    
    private QubitOperations qubitOperations = new QubitOperations();

    private void OnEnable()
    {
        if (qubitWireController != null)
        {
            qubitWireController.FinalStateChanged.AddListener(CheckWinState);
        }
        else
        {
            Debug.LogError("QubitWireController reference is missing in OneQubitGameManager.");
        }
    }

    private void OnDisable()
    {
        if (qubitWireController != null)
        {
            qubitWireController.FinalStateChanged.RemoveListener(CheckWinState);
        }
    }

    void Awake()
    {
        winState = qubitOperations.ConvertToQubit(winStateChoice);

        if (qubitWireController != null)
        {
            qubitWireController.SetInputState(startingStateChoice);
        }
    }

    private void CheckWinState(Qubit finalState)
    {
        
        UpdateBarChart(finalState);
        print( finalState);
        if (winStateChoice != SingleQubitStateOptions.NoState && winState.IsApproximatelyEqual(finalState))
        {
            StartCoroutine(WaitAndShowWinScreen());
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

    private void UpdateBarChart(Qubit finalState)
    {
        float probability0 = (float)(finalState.Alpha.Magnitude * finalState.Alpha.Magnitude);
        float probability1 = (float)(finalState.Beta.Magnitude * finalState.Beta.Magnitude);

        barChartManager.SetSliderValue(0, probability0);
        barChartManager.SetSliderValue(1, probability1);
    }
    public SingleQubitStateOptions GetInputState()
    {
        return startingStateChoice;
    }
}
