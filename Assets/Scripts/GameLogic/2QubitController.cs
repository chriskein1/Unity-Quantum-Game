// Script will handle the output of two one-qubit Game Controllers to create one cohesive output
// Using q1q0 notation

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using QubitType;
public class TwoQubitController : MonoBehaviour
{
    // Array of 2 qubits
    [SerializeField] private Qubit[] WinState = new Qubit[2];
    [SerializeField] private ChangeTileState outputTile1;
    [SerializeField] private ChangeTileState outputTile2;
    [SerializeField] private BarChartManager barChartManager;
    [SerializeField] private GameObject WinScreen;
    
    // Start is called before the first frame update
    void Start()
    {
        // Prevent -0 win state
        for (int i = 0; i < 2; i++)
        {
            if (WinState[i].state == 0 && !WinState[i].PositiveState && !WinState[i].ImaginaryState)
            {
                WinState[i].PositiveState = true;
            }
        }
        UpdateOutput();
    }

    public void UpdateOutput()
    {
        // Get the state of the two output tiles
        Qubit[] qubits = {outputTile1.GetState(), outputTile2.GetState()};

        // Update bar chart
        if (barChartManager != null)
        {
            DisplayAnimation(qubits);
        }
        
        // Check if the output matches the win state
        if (WinScreen != null && qubits[0].state == WinState[0].state 
            && qubits[1].state == WinState[1].state 
            && qubits[0].PositiveState == WinState[0].PositiveState 
            && qubits[1].PositiveState == WinState[1].PositiveState 
            && qubits[0].ImaginaryState == WinState[0].ImaginaryState 
            && qubits[1].ImaginaryState == WinState[1].ImaginaryState
            && qubits[0].HApplied == WinState[0].HApplied
            && qubits[1].HApplied == WinState[1].HApplied)
        {
            Debug.Log("You Win!!!");
            // Set time to 0
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

    private void DisplayAnimation(Qubit[] qubits)
    {
        barChartManager.ResetBars();
        // Two H gates means 25% chance of each state
        if (qubits[0].HApplied && qubits[1].HApplied)
        {
            barChartManager.SetSliderValue(0, 0.25f);
            barChartManager.SetSliderValue(1, 0.25f);
            barChartManager.SetSliderValue(2, 0.25f);
            barChartManager.SetSliderValue(3, 0.25f);
        }
        // If q1 is in superposition and q0 is not
        else if (qubits[1].HApplied && !qubits[0].HApplied)
        {
            // 50% chance of |00> and |10>
            if (qubits[0].state == 0)
            {
                barChartManager.SetSliderValue(0, 0.5f);
                barChartManager.SetSliderValue(2, 0.5f);
            }
            // 50% chance of |01> and |11>
            else
            {
                barChartManager.SetSliderValue(1, 0.5f);
                barChartManager.SetSliderValue(3, 0.5f);
            }
        }
        // If q0 is in superposition and q1 is not
        else if (!qubits[1].HApplied && qubits[0].HApplied)
        {
            // 50% chance of |00> and |01>
            if (qubits[1].state == 0)
            {
                barChartManager.SetSliderValue(0, 0.5f);
                barChartManager.SetSliderValue(1, 0.5f);
            }
            // 50% chance of |10> and |11>
            else
            {
                barChartManager.SetSliderValue(2, 0.5f);
                barChartManager.SetSliderValue(3, 0.5f);
            }
        }
        // If neither qubit is in superposition
        else
        {
            // 100% chance of |00>
            if (qubits[1].state == 0 && qubits[0].state == 0)
            {
                barChartManager.SetSliderValue(0, 1f);
            }
            // 100% chance of |01>
            else if (qubits[1].state == 0 && qubits[0].state == 1)
            {
                barChartManager.SetSliderValue(1, 1f);
            }
            // 100% chance of |10>
            else if (qubits[1].state == 1 && qubits[0].state == 0)
            {
                barChartManager.SetSliderValue(2, 1f);
            }
            // 100% chance of |11>
            else
            {
                barChartManager.SetSliderValue(3, 1f);
            }
        }
    }

}
