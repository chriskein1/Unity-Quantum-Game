using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePuzzleController : MonoBehaviour
{
    [SerializeField] private List<GameObject> SnapPoints = new List<GameObject>();
    
    // qbits are represented as either 0 or 1, and their sign is true for positive and false for negative
    private List<(int, bool, bool)> snapPointStates = new List<(int, bool, bool)>();

    // Start is called before the first frame update
    void Start()
    {
        SetSnapPoints();
    }

    // Function to initialize the snapPointStates list
    // For now they all match the input tile's state
    public void SetSnapPoints()
    {
        // Clear list
        snapPointStates.Clear();

        // Get input tile component's state
        InputTile inputTile = GameObject.Find("InputTile").GetComponent<InputTile>();
        
        // Each snap point's state will match the state of what comes before it, and then the gate operation will affect that state

        // First state is the input tile's state
        snapPointStates.Add(inputTile.GetState());
        foreach(GameObject p in SnapPoints)
        {
            Snap snapComp = p.GetComponent<Snap>();

            // Get the gate object on the snap point
            GameObject gateObject = snapComp.GetGateObject();
            
            // Get the state of last snap point
            (int, bool, bool) state = snapPointStates[snapPointStates.Count - 1]; // First state is the input tile's state
            
            // Do a gate operation on the current state
            GateOperation(gateObject, ref state);

            // Add the new state to the list
            snapPointStates.Add(state);
        }

        // Additional item for the output tile
        snapPointStates.Add(snapPointStates[snapPointStates.Count - 1]);

        // The output tile will match the last snap point's state
        InputTile outputTile = GameObject.Find("OutputTile").GetComponent<InputTile>();

        // Set the output tile's state
        outputTile.SetState(snapPointStates[snapPointStates.Count - 1]);

        // Display the snap point states
        DisplaySnapPointStates();

    }

    // Function to display the snap point states
    public void DisplaySnapPointStates()
    {
        foreach((int, bool, bool) state in snapPointStates)
        {
            if (state.Item2)
            {
                Debug.Log($"|{state.Item1}>");
            }
            else
            {
                Debug.Log($"-|{state.Item1}>");
            }
        }
    }

    // Gate operation function
    private void GateOperation (GameObject gateObject, ref (int, bool, bool) state)
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
                state.Item1 = (state.Item1 + 1) % 2;
                break;
            
            case "YGate":
                // Y gate operation
                Debug.Log("Y gate operation");
                state.Item2 = !state.Item2;
                break;
            
            case "ZGate":
                // Z gate operation
                Debug.Log("Z gate operation");
                if (state.Item1 == 1)
                {
                    state.Item2 = !state.Item2;
                }
                break;

            case "HGate":
                Debug.Log("H gate operation");
                // Superposition
                state.Item3 = !state.Item3;
                break;
        }

    }



    // private bool complete=false;
    // private void Update()
    // {
    //     foreach( GameObject p in SnapPoints)
    //     {
    //         complete = true;
    //         Snap snapComp = p.GetComponent<Snap>();
    //         if (snapComp.GetGateStatus() == false)
    //         {
    //             complete=false;
    //             break;
    //         }
    //     }
    //     if (complete == true)
    //     {
    //         print("Puzzle Complete!!!!");
    //     }
  
    // }
}

