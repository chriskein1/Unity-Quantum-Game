using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using QubitType;
public class GamePuzzleController : MonoBehaviour
{
    [SerializeField] private List<GameObject> SnapPoints = new List<GameObject>();

    [SerializeField] private Qubit WinState;
    [SerializeField] private ChangeTileState inputTile;
    [SerializeField] private ChangeTileState outputTile;
    [SerializeField] private GameObject WinScreen;
    [SerializeField] private BarChartManager barChartManager;
    
    // qbits are represented as either 0 or 1, and their sign is true for positive and false for negative
    private List<Qubit> snapPointStates = new List<Qubit>();

    public UnityEvent OutputChanged;

    // Start is called before the first frame update
    void Start()
    {
        // Prevent -0 win state
        if (WinState.state == 0 && !WinState.PositiveState && !WinState.ImaginaryState)
        {
            WinState.PositiveState = true;
        }
        SetSnapPoints();
    }

    // Function to initialize the snapPointStates list
    // For now they all match the input tile's state
    public void SetSnapPoints()
    {
        // Clear list
        snapPointStates.Clear();

        // Each snap point's state will match the state of what comes before it, and then the gate operation will affect that state

        // First state is the input tile's state
        snapPointStates.Add(inputTile.GetState());
        foreach(GameObject p in SnapPoints)
        {
            Snap snapComp = p.GetComponent<Snap>();

            // Get the gate object on the snap point
            GameObject gateObject = snapComp.GetGateObject();
            
            // Get the state of last snap point
            Qubit state = snapPointStates[snapPointStates.Count - 1]; // First state is the input tile's state
            
            // Prevent -0
            if (state.state == 0 && !state.PositiveState && !state.ImaginaryState)
            {
                state.PositiveState = true;
            }
            // Do a gate operation on the current state
            GateOperation(gateObject, ref state);

            // Add the new state to the list
            snapPointStates.Add(state);

            // Update snap point state
            snapComp.SetState(state);
        }

        Qubit finalState = snapPointStates[snapPointStates.Count - 1];
        // Display animation
        if (barChartManager != null)
        {
            DisplayAnimation(finalState);
        }
        
        // Additional item for the output tile
        snapPointStates.Add(finalState);

        // The output tile will match the last snap point's state
        // Set the output tile's state
        outputTile.SetState(finalState);
        OutputChanged.Invoke();

        // Check if the final state matches the win state
        bool win = finalState == WinState;

        // Check if the final state is equivalent but not directly equal
        if (!win)
        {
            if (WinState.HApplied)
            {
                // H|0> = -H|1>
                if (WinState.state == 0)
                {
                    if (finalState.state == 1 && finalState.HApplied && !finalState.PositiveState
                        && finalState.ImaginaryState == WinState.ImaginaryState)
                    {
                        win = true;
                    }
                }
                // iH|1> = -iH|0>
                else
                {
                    if (finalState.state == 0 && finalState.HApplied && !finalState.PositiveState
                        && finalState.ImaginaryState == WinState.ImaginaryState)
                    {
                        win = true;
                    }
                }
            }
        }
        
        if (WinScreen != null 
            && win)
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
        // Print the snap point states
        // DisplaySnapPointStates();
    }

    private void DisplayAnimation(Qubit finalState)
    {
        // Reset all bars
        barChartManager.ResetBars();

        // Display animation
        if (finalState.HApplied)
        {
            // Set both sliders to 50%
            barChartManager.SetSliderValue(0, 0.5f); // Slider 0 is being set to 50% (0.5f)
            barChartManager.SetSliderValue(1, 0.5f); // Slider 1 is being set to 50% (0.5f)
        }
        else if (finalState.state == 1)
        {
            barChartManager.SetSliderValue(1, 1f); // Slider 1 is being set to 100% (1f)
        }
        else
        {
            barChartManager.SetSliderValue(0, 1f); // Slider 0 is being set to 100% (1f)
        }
    }


    // Function to Print the snap point states
    public void DisplaySnapPointStates()
    {
        foreach(Qubit state in snapPointStates)
        {
            if (state.PositiveState)
            {
                Debug.Log($"|{state.state}>");
            }
            else
            {
                Debug.Log($"-|{state.state}>");
            }
        }
    }

    // Gate operation function
    private void GateOperation (GameObject gateObject, ref Qubit state)
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
                state.state = (state.state + 1) % 2;
                break;
            
            case "YGate":
                // Y gate operation
                Debug.Log("Y gate operation");

                // Bit & phase flip

                if (state.state == 1)
                {
                    state.PositiveState = !state.PositiveState;
                }
                state.state = (state.state + 1) % 2;
                state.ImaginaryState = !state.ImaginaryState;

                // Prevent -0
                if (state.state == 0 && !state.PositiveState && !state.ImaginaryState)
                {
                    state.PositiveState = true;
                }
                break;
            
            case "ZGate":
                // Z gate operation
                Debug.Log("Z gate operation");
                if (state.state == 1)
                {
                    state.PositiveState = !state.PositiveState;
                }
                break;

            case "HGate":
                Debug.Log("H gate operation");
                // H applied flag flipped
                state.HApplied = !state.HApplied;
                break;
        }
    }
    public Qubit GetWinState()
    {
        return WinState;
    }


    public bool GetWinScreenStatus()
    {
        if (WinScreen == null)
        {
            return false;
        }
        return WinScreen.activeInHierarchy;
    }
}