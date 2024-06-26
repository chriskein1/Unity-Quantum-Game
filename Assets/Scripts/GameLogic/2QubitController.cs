// Script will handle the output of two one-qubit Game Controllers to create one cohesive output

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
    [SerializeField] private TwoQubitTile twoQubitTile;
    
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

        // Update the result state
        if (twoQubitTile != null)
        {
            twoQubitTile.SetState(qubits[0], qubits[1]);
        }
        
        bool win = qubits[0] == WinState[0] && qubits[1] == WinState[1];

        bool negativeSign, negativeWin;
        bool imaginaryOutput, imaginaryWin;
        // Check output qubits for negative sign
        negativeSign = qubits[0].PositiveState != qubits[1].PositiveState;

        // i * i = -1
        if (qubits[0].ImaginaryState && qubits[1].ImaginaryState)
        {
            negativeSign = !negativeSign;
        }
        // Check for one i
        imaginaryOutput = qubits[0].ImaginaryState != qubits[1].ImaginaryState;

        // Check win state for negative sign
        negativeWin = WinState[0].PositiveState != WinState[1].PositiveState;
            
        // i * i = -1
        if (WinState[0].ImaginaryState && WinState[1].ImaginaryState)
        {
                negativeWin = !negativeWin;
        }
        // Check for one i
        imaginaryWin = WinState[0].ImaginaryState != WinState[1].ImaginaryState;

        // If the two qubits do not match exactly, check if it is because
        // of a sign mismatch (i.e. a negative sign on a different qubit or i*i = -1)
        if (!win)
        {
            Debug.Log("Negative goal: " + negativeWin);
            Debug.Log("Negative output: " + negativeSign);

            // Check if the initial states match/H gate order
            if (qubits[0].state == WinState[0].state && qubits[1].state == WinState[1].state
                && qubits[0].HApplied == WinState[0].HApplied && qubits[1].HApplied == WinState[1].HApplied)
            {
                // Check if the signs and imaginary parts match
                if (negativeSign == negativeWin && imaginaryOutput == imaginaryWin)
                {
                    win = true;
                }
            }
        }

        // H gate mismatch
        if (!win)
        {
            // Case 1) only q1 has an H gate
            if (qubits[0].HApplied && !qubits[1].HApplied)
            {
                // H|0> = -H|1>
                if (qubits[0].state == 0)
                {
                    // Check if q2 matches win state
                    if (negativeSign && qubits[1].state == WinState[1].state
                        && imaginaryOutput == imaginaryWin)
                        {
                            win = true;
                        }
                }
                // iH|1> = -iH|0>
                else
                {
                    // check if q2 matches win state
                    if (negativeSign && qubits[1].state == WinState[1].state
                        && imaginaryOutput == imaginaryWin)
                        {
                            win = true;
                        }
                } 
            }
            // Case 2) Only q2 has an H gate
            else if (!qubits[0].HApplied && qubits[1].HApplied)
            {
                // H|0> = -H|1>
                if (qubits[1].state == 0)
                {
                    // Check if q1 matches win state
                    if (negativeSign && qubits[0].state == WinState[0].state
                        && imaginaryOutput == imaginaryWin)
                        {
                            win = true;
                        }
                }
                // iH|1> = -iH|0>
                else
                {
                    // Check if q1 matches win state
                    if (negativeSign && qubits[0].state == WinState[0].state
                        && imaginaryOutput == imaginaryWin)
                        {
                            win = true;
                        }
                }
            }
            // Case 3) Both qubits have an H gate
            if (imaginaryOutput == imaginaryWin && negativeSign == negativeWin)
            {
                // State combinations (with 1/2 or i/2 factored out):
                // 1) |00> + |01> + |10> + |11>
                    // H|0> * H|0>
                // 2) |00> - |01> + |10> - |11>
                    // H|0> * H|1>
                // 3) |00> + |01> - |10> - |11>
                    // H|1> * H|0>
                // 4) |00> - |01> - |10> + |11>
                    // H|1> * H|1>

                // 1)
                if ((qubits[0].state == 0 && qubits[1].state == 0 && !negativeSign)
                    || (qubits[0].state == 1 && qubits[1].state == 0 && negativeSign)
                    || (qubits[0].state == 0 && qubits[1].state == 1 && negativeSign))
                {
                    if (WinState[0].state == 0 && WinState[1].state == 0 && !negativeWin
                        || (WinState[0].state == 1 && WinState[1].state == 0 && negativeWin)
                        || (WinState[0].state == 0 && WinState[1].state == 1 && negativeWin))
                    {
                        win = true;
                    }
                }
                // 2)
                else if ((qubits[0].state == 0 && qubits[1].state == 1 && !negativeSign)
                        || (qubits[0].state == 1 && qubits[1].state == 0 && negativeSign))
                {
                    if ((WinState[0].state == 0 && WinState[1].state == 1 && !negativeWin)
                        || (WinState[0].state == 1 && WinState[1].state == 0 && negativeWin))
                    {
                        win = true;
                    }
                }
                // 3)
                else if ((qubits[0].state == 1 && qubits[1].state == 0 && !negativeSign)
                        || (qubits[0].state == 0 && qubits[1].state == 1 && negativeSign))
                {
                    if ((WinState[0].state == 1 && WinState[1].state == 0 && !negativeWin)
                        || (WinState[0].state == 0 && WinState[1].state == 1 && negativeWin))
                    {
                        win = true;
                    }
                }
                // 4)
                else if ((qubits[0].state == 1 && qubits[1].state == 1 && !negativeSign)
                        || (qubits[0].state == 0 && qubits[1].state == 0 && negativeSign))
                {
                    if ((WinState[0].state == 1 && WinState[1].state == 1 && !negativeWin)
                        || (WinState[0].state == 0 && WinState[1].state == 0 && negativeWin))
                    {
                        win = true;
                    }
                }
            }
        }

        // Check if the output matches the win state
        if (WinScreen != null && win)
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
        else if (qubits[0].HApplied && !qubits[1].HApplied)
        {
            // 50% chance of |00> and |10>
            if (qubits[1].state == 0)
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
        else if (!qubits[0].HApplied && qubits[1].HApplied)
        {
            // 50% chance of |00> and |01>
            if (qubits[0].state == 0)
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
            if (qubits[0].state == 0 && qubits[1].state == 0)
            {
                barChartManager.SetSliderValue(0, 1f);
            }
            // 100% chance of |01>
            else if (qubits[0].state == 0 && qubits[1].state == 1)
            {
                barChartManager.SetSliderValue(1, 1f);
            }
            // 100% chance of |10>
            else if (qubits[0].state == 1 && qubits[1].state == 0)
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

    public Qubit[] GetWinState()
    {
        // Prevent -0 win state
        for (int i = 0; i < 2; i++)
        {
            if (WinState[i].state == 0 && !WinState[i].PositiveState && !WinState[i].ImaginaryState)
            {
                WinState[i].PositiveState = true;
            }
        }
        return WinState;
    }
}
