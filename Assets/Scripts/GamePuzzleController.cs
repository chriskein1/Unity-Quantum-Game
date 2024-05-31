using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePuzzleController : MonoBehaviour
{
    [SerializeField] private List<GameObject> SnapPoints = new List<GameObject>();
    
    // qbits are represented as either 0 or 1, and their sign is true for positive and false for negative
    private List<(int, bool)> snapPointStates = new List<(int, bool)>();

    // Function to initialize the snapPointStates list
    // For now they all match the input tile's state
    private void SetSnapPointStates()
    {
        // Get input tile component's state
        InputTile inputTile = GameObject.Find("InputTile").GetComponent<InputTile>();
        
        // Each snap point's state will match the state of what comes before it, and then the gate operation will affect that state

        // First state is the input tile's state
        snapPointStates.Add(inputTile.GetState());
        foreach(GameObject p in SnapPoints)
        {
            Snap snapComp = p.GetComponent<Snap>();

            // Get the gate object on the snap point (implement later)
            // GameObject gateObject = snapComp.GetGateObject();
            
            // Get the state of kast snap point
            (int, bool) state = snapPointStates[snapPointStates.Count - 1]; // First state is the input tile's state
            
            // Do a gate operation on the current state (implement later)
            // state.GateOperation(gateObject);

            // Add the new state to the list
            snapPointStates.Add(state);
        }

        // The output tile will match the last snap point's state
        InputTile outputTile = GameObject.Find("OutputTile").GetComponent<InputTile>();

        // Set the output tile's state
        outputTile.SetState(snapPointStates[snapPointStates.Count - 1]);

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

