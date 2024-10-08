﻿using System.Collections.Generic;
using UnityEngine;
using QubitType;
using System;
using System.Numerics;
using Vector3 = UnityEngine.Vector3;
public class CircuitManager : MonoBehaviour
{
    [SerializeField] private List<QubitWireController> qubitWireControllers;
    [SerializeField] private List<SingleQubitStateOptions> qubitInputs;
    [SerializeField] private List<SingleQubitStateOptions> winState;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private BarChartManager barChartManager;
    private List<List<GameObject>> snapPointLists = new List<List<GameObject>>();
    private List<List<Qubit>> snapPointStates = new List<List<Qubit>>();
    private QubitOperations qubitOperations = new QubitOperations();
    public List<VisualQubit> visualInput;
    public List<VisualQubit> visualOutput;
    float yDistance;
    bool win = false;
    private List<Qubit> finalStates = new List<Qubit>();

    private void Start()
    {
        InitializeSnapPointLists();
        if (snapPointLists.Count > 1)
            yDistance = Mathf.Abs(snapPointLists[1][0].transform.position.y - snapPointLists[0][0].transform.position.y) / 2;
        else
            yDistance = 0; // Irrelevant if there is only one row
        SetRowAndDistanceForSnapPoints();
        SetInputs();
        Evaluate();
    }

    private void InitializeSnapPointLists()
    {
        snapPointLists.Clear();
        snapPointStates.Clear();

        foreach (QubitWireController controller in qubitWireControllers)
        {
            List<GameObject> snapPoints = controller.GetSnapPoints();
            snapPointLists.Add(snapPoints);
            List<Qubit> qubitList = new List<Qubit>();
            for (int i = 0; i < snapPoints.Count; i++)
            {
                qubitList.Add(new Qubit());
            }
            snapPointStates.Add(qubitList);
        }

    }
    private void SetRowAndDistanceForSnapPoints()
    {
        for (int row = 0; row < snapPointLists.Count; row++)
        {
            for (int col = 0; col < snapPointLists[row].Count; col++)
            {
                Snap snapComponent = snapPointLists[row][col].GetComponent<Snap>();
                if (snapComponent != null)
                {
                    snapComponent.SetRow(row);
                    snapComponent.SetDistance(yDistance);
                }
            }
        }
    }
    public void Evaluate()
    {
        if (snapPointLists.Count == 0 || snapPointStates.Count == 0)
        {
            Debug.LogError("snapPointLists or snapPointStates is not initialized.");
            return;
        }

        int numColumns = snapPointLists[0].Count;

        // Evaluate each column from left to righ
        for (int col = 0; col < numColumns; col++)
        {
            for (int row = 0; row < qubitWireControllers.Count; row++)
            {
                if (col >= snapPointLists[row].Count)
                {
                    Debug.LogError($"Column index {col} out of range for row {row}");
                    continue;
                }

                GameObject snapPoint = snapPointLists[row][col];
                Snap snapComp = snapPoint.GetComponent<Snap>();
                GameObject gateObject = snapComp.GetGateObject();

                bool skipSecondRow = false;

                // Check for adjacent 2bit gates in the previous row
                if (row > 0)
                {
                    GameObject adjacentSnapPoint = snapPointLists[row - 1][col];
                    Snap adjacentSnapComp = adjacentSnapPoint.GetComponent<Snap>();
                    GameObject adjacentGateObject = adjacentSnapComp.GetGateObject();

                    if (adjacentGateObject != null && (adjacentGateObject.CompareTag("ctrl") || adjacentGateObject.CompareTag("swap")))
                    {
                        skipSecondRow = true;
                    }
                }

                if (gateObject != null)
                {
                    if (gateObject.CompareTag("ctrl"))
                    {
                        HandleCNOTGate(row, col, gateObject);
                    }
                    else if (gateObject.CompareTag("swap"))
                    {
                        HandleSWAPGate(row, col, gateObject);
                    }
                    else
                    {
                        ApplySingleQubitGate(row, col, gateObject);
                    }
                }
                else
                {
                    if (col == 0) // First column should get the input state
                    {
                        snapPointStates[row][col] = qubitOperations.ConvertToQubit(qubitInputs[row]);
                    }
                    else if (!skipSecondRow) // Only set the state from the previous column if no adjacent CNOT or swap
                    {
                        snapPointStates[row][col] = snapPointStates[row][col - 1];
                    }
                }

                snapComp.SetState(snapPointStates[row][col]);
                

                // Set the output state for the current column
                if (col == numColumns - 1)
                {
                    Qubit finalState = snapPointStates[row][col];
                    qubitWireControllers[row].SetOutput(finalState);
                }

                // EvaluateMultipleTimes(100);
            }
        }

        win = false;

        // Update the visual outputs and evaluate the win condition
        if (qubitWireControllers.Count >= 2)
        {
             Qubit finalStateQubit1 = snapPointStates[0][numColumns - 1];
             Qubit finalStateQubit2 = snapPointStates[1][numColumns - 1];
            finalStates.Clear();
            for (int i = 0; i < qubitWireControllers.Count; i++)
            {
                finalStates.Add(snapPointStates[i][numColumns - 1]);
            }
            if (barChartManager != null)
                UpdateBarChart(finalStateQubit1, finalStateQubit2);
            win = EvaluateWin();
            if (visualOutput.Count > 0 && visualOutput[0] != null)
                visualOutput[0].SetQubit(finalStates[0], 0);
            if (visualOutput.Count > 1 && visualOutput[1] != null)
                visualOutput[1].SetQubit(finalStates[1], 1);
            //Debug.Log("Setting output");
            //Debug.Log("Final state is " + finalStates[0] + " and " + finalStates[1]);
        }
        else if (qubitWireControllers.Count == 1)
        {
             Qubit finalStateQubit = snapPointStates[0][numColumns - 1];
            finalStates.Clear();
            finalStates.Add(snapPointStates[0][numColumns - 1]);

            if (barChartManager != null)
                UpdateBarChartSingle(finalStateQubit);
            win = EvaluateWin();
            if (visualOutput.Count > 0 && visualOutput[0] != null)
                visualOutput[0].SetQubit(finalStates[0], 0);
            //Debug.Log("Setting output 0");
            //Debug.Log("Final state is " + finalStates[0]);
        }

        if (win)
        {
            if (winScreen != null)
            {
                Time.timeScale = 0;
                winScreen.SetActive(true);
            }
            Debug.Log("YOU WIN!!!!!!");
        }
    }



    private void HandleCNOTGate(int row, int col, GameObject gateObject)
    {
        // Ensure col is not the first column
        if (col == 0)
        {
            Debug.LogError("CNOT gate cannot be applied in the first column.");
            return;
        }

        Qubit controlQubit = snapPointStates[row][col - 1];
        //Debug.Log($"Control qubit state at row {row}, col {col - 1}: {controlQubit}");

        // Apply CNOT gate to the target qubits in adjacent rows
        for (int targetRow = 0; targetRow < qubitWireControllers.Count; targetRow++)
        {
            if (targetRow == row) continue;

            if (col >= snapPointLists[targetRow].Count)
            {
                Debug.LogError($"Column index {col} out of range for target row {targetRow}");
                continue;
            }

            Qubit targetQubit = snapPointStates[targetRow][col - 1];
            //Debug.Log($"Applying CNOT gate: control qubit state at row {row}, col {col - 1}: {controlQubit}, target qubit state at row {targetRow}, col {col - 1}: {targetQubit}");
            QuantumGates.ApplyCNOT(ref controlQubit, ref targetQubit);
            snapPointStates[targetRow][col] = targetQubit;
            snapPointLists[targetRow][col].GetComponent<Snap>().SetState(targetQubit);
            //Debug.Log($"Resulting target qubit state at row {targetRow}, col {col}: {targetQubit}");
        }

        snapPointStates[row][col] = controlQubit;
        //Debug.Log($"Resulting control qubit state at row {row}, col {col}: {controlQubit}");


    }




    private void HandleSWAPGate(int row, int col, GameObject gateObject)
    {
        //Debug.Log($"Handling SWAP gate at row {row}, col {col}");

        // Ensure col is not the first column
        if (col == 0)
        {
            Debug.LogError("SWAP gate cannot be applied in the first column.");
            return;
        }

        // Calculate the adjacent row to swap with (next row, wrapping around)
        int swapRow = (row + 1) % qubitWireControllers.Count;

        // Ensure the swapRow is valid and not out of range
        if (col >= snapPointLists[swapRow].Count)
        {
            Debug.LogError($"Column index {col} out of range for swap row {swapRow}");
            return;
        }

        Qubit qubit1 = snapPointStates[row][col - 1];
        Qubit qubit2 = snapPointStates[swapRow][col - 1];
        QuantumGates.ApplySWAP(ref qubit1, ref qubit2);

        snapPointStates[row][col] = qubit1;
        snapPointStates[swapRow][col] = qubit2;

        Debug.Log($"Swapped qubits: qubit1={qubit1}, qubit2={qubit2} at row {row} and swapRow {swapRow}");

        // Update the state of the snap points
        snapPointLists[row][col].GetComponent<Snap>().SetState(qubit1);
        snapPointLists[swapRow][col].GetComponent<Snap>().SetState(qubit2);
        
    }

    
    private void ApplySingleQubitGate(int row, int col, GameObject gateObject)
    {
        Qubit state;
        if (col == 0)
        {
            state = qubitOperations.ConvertToQubit(qubitInputs[row]);
        }
        else
        {
            state = snapPointStates[row][col - 1];
        }

        qubitOperations.ApplyGateOperation(gateObject, ref state);
        snapPointStates[row][col] = state;
    }

    private void SetInputs()
    {
        for (int i = 0; i < qubitWireControllers.Count; i++)
        {
            if (snapPointStates[i].Count > 0)  // Ensure the list has been initialized
            {
                Qubit inputQubit = qubitOperations.ConvertToQubit(qubitInputs[i]);
                snapPointStates[i][0] = inputQubit;
                qubitWireControllers[i].SetInput(qubitInputs[i]);
                if (visualInput.Count > i && visualInput[i] != null){
                    visualInput[i].SetQubit(inputQubit, i);
                Debug.Log("Setting input" + i);
                }
            }
        }
    }

    private void UpdateBarChart(Qubit finalStateQubit1, Qubit finalStateQubit2)
    {
        // Calculate probabilities for the states |00⟩, |01⟩, |10⟩, and |11⟩
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
    private void UpdateBarChartSingle(Qubit finalStateQubit)
    {
        // Calculate probabilities for the states |0> and |1>
        float prob0 = MagnitudeSquared(finalStateQubit.Alpha);
        float prob1 = MagnitudeSquared(finalStateQubit.Beta);

        // Update the bar chart using the same method but with only two bars
        if(barChartManager != null)
        barChartManager.UpdateBarChart(prob0, prob1);
    }

    // Determine if the win condition is met for the qubits
    public bool EvaluateWin()
    {
        Debug.Log("Evaluating win state...");

        if (winState.Count == 0)
        {
            Debug.LogError("Win state is not initialized.");
            return false;
        }
        else if (finalStates.Count != winState.Count)
        {
            Debug.LogError($"Final states count ({finalStates.Count}) does not match win state count ({winState.Count}).");
            return false;
        }

        // Combine final states into a state vector
        Complex[] finalStateVector = CombineStatesIntoVector(finalStates);

        // Combine win states into a state vector
        List<Qubit> winStateQubits = new List<Qubit>();
        foreach (SingleQubitStateOptions state in winState)
        {
            winStateQubits.Add(qubitOperations.ConvertToQubit(state));
        }
        Complex[] winStateVector = CombineStatesIntoVector(winStateQubits);

        // Compare the final state vector with the win state vector
        bool result = AreVectorsEqual(finalStateVector, winStateVector);
        Debug.Log($"Final win condition result: {result}");
        return result;
    }

    private Complex[] CombineStatesIntoVector(List<Qubit> states)
    {
        if (states.Count == 1)
        {
            // For a single qubit, the state vector is just the alpha and beta components
            return new Complex[] { states[0].Alpha, states[0].Beta };
        }
        else if (states.Count == 2)
        {
            // For two qubits, the state vector is the tensor product of the two qubit states
            return new Complex[]
            {
            states[0].Alpha * states[1].Alpha,  // |00⟩
            states[0].Alpha * states[1].Beta,   // |01⟩
            states[0].Beta * states[1].Alpha,   // |10⟩
            states[0].Beta * states[1].Beta     // |11⟩
            };
        }
        else
        {
            Debug.LogError("Unexpected number of qubits.");
            return null;
        }
    }

    private bool AreVectorsEqual(Complex[] vector1, Complex[] vector2, double tolerance = 1e-10)
    {
        if (vector1 == null || vector2 == null || vector1.Length != vector2.Length)
        {
            Debug.LogError("Vectors are not of the same length or one is null.");
            return false;
        }

        for (int i = 0; i < vector1.Length; i++)
        {
            if (Math.Abs(vector1[i].Real - vector2[i].Real) > tolerance ||
                Math.Abs(vector1[i].Imaginary - vector2[i].Imaginary) > tolerance)
            {
                Debug.Log($"Vectors differ at index {i}: {vector1[i]} vs {vector2[i]}");
                return false;
            }
        }

        return true;
    }



    public void EvaluateMultipleTimes(int numberOfEvaluations)
    {
        if (snapPointLists.Count == 0 || snapPointStates.Count == 0)
        {
            Debug.LogError("snapPointLists or snapPointStates is not initialized.");
            return;
        }

        int numColumns = snapPointLists[0].Count;
        int numRows = qubitWireControllers.Count;

        // Dictionary to track probabilities of each possible state
        Dictionary<string, int> stateCounts = new Dictionary<string, int>
    {
        { "|00⟩", 0 },
        { "|01⟩", 0 },
        { "|10⟩", 0 },
        { "|11⟩", 0 }
    };

        for (int eval = 0; eval < numberOfEvaluations; eval++)
        {
            // Reset the states before each evaluation
            //for (int row = 0; row < numRows; row++)
            //{
            //    for (int col = 0; col < numColumns; col++)
            //    {
            //        snapPointStates[row][col] = new Qubit();
            //    }
            //}

            for (int col = 0; col < numColumns; col++)
            {
                for (int row = 0; row < numRows; row++)
                {
                    if (col >= snapPointLists[row].Count)
                    {
                        Debug.LogError($"Column index {col} out of range for row {row}");
                        continue;
                    }

                    GameObject snapPoint = snapPointLists[row][col];
                    Snap snapComp = snapPoint.GetComponent<Snap>();
                    GameObject gateObject = snapComp.GetGateObject();

                    bool skipSecondRow = false;

                    if (row > 0)
                    {
                        GameObject adjacentSnapPoint = snapPointLists[row - 1][col];
                        Snap adjacentSnapComp = adjacentSnapPoint.GetComponent<Snap>();
                        GameObject adjacentGateObject = adjacentSnapComp.GetGateObject();

                        if (adjacentGateObject != null && (adjacentGateObject.CompareTag("ctrl") || adjacentGateObject.CompareTag("swap")))
                        {
                            skipSecondRow = true;
                        }
                    }

                    if (gateObject != null)
                    {
                        if (gateObject.CompareTag("ctrl"))
                        {
                            HandleCNOTGate(row, col, gateObject);
                        }
                        else if (gateObject.CompareTag("swap"))
                        {
                            HandleSWAPGate(row, col, gateObject);
                        }
                        else
                        {
                            ApplySingleQubitGate(row, col, gateObject);
                        }
                    }
                    else
                    {
                        if (col == 0)
                        {
                            snapPointStates[row][col] = qubitOperations.ConvertToQubit(qubitInputs[row]);
                        }
                        else if (!skipSecondRow)
                        {
                            snapPointStates[row][col] = snapPointStates[row][col - 1];
                        }
                    }

                    snapComp.SetState(snapPointStates[row][col]);

                    if (col == numColumns - 1)
                    {
                        Qubit finalState = snapPointStates[row][col];
                        qubitWireControllers[row].SetOutput(finalState);
                    }
                }
            }

            // Measure the final states and update the state counts
            if (numRows >= 2)
            {
                Qubit finalStateQubit1 = snapPointStates[0][numColumns - 1].Measure();
                Qubit finalStateQubit2 = snapPointStates[1][numColumns - 1].Measure();
                string state = $"|{(finalStateQubit1.Beta.Magnitude > 0 ? "1" : "0")}{(finalStateQubit2.Beta.Magnitude > 0 ? "1" : "0")}⟩";
                stateCounts[state]++;
            }
            else if (numRows == 1)
            {
                Qubit finalStateQubit = snapPointStates[0][numColumns - 1].Measure();
                string state = $"|0{(finalStateQubit.Beta.Magnitude > 0 ? "1" : "0")}⟩";
                stateCounts[state]++;
            }
        }

        // Calculate probabilities
        Dictionary<string, float> stateProbabilities = new Dictionary<string, float>();
        foreach (var state in stateCounts.Keys)
        {
            stateProbabilities[state] = (float)stateCounts[state] / numberOfEvaluations;
        }
        // Update the bar chart
        if (numRows >= 2)
        {
             //barChartManager.UpdateBarChart(stateProbabilities["|00⟩"], stateProbabilities["|01⟩"], stateProbabilities["|10⟩"], stateProbabilities["|11⟩"]);
        }
        else if (numRows == 1)
        {
            // barChartManager.UpdateBarChart(stateProbabilities["|00⟩"], stateProbabilities["|01⟩"]);
        }

        // Output probabilities
        foreach (var state in stateProbabilities)
        {
            // Debug.Log($"{state.Key}: {state.Value * 100}%");
        }
    }
    
    // Return if any wire has a gate on it everywhere
    public bool HasGate()
    {
        bool hasGate = true;
        for (int i = 0; i < qubitWireControllers.Count; i++)
        { 
            // Skip snap point 1 because it is the input
            for (int j = 1; j < snapPointLists[i].Count; j++)
            {
                GameObject gateObject = snapPointLists[i][j].GetComponent<Snap>().GetGateObject();
                if (gateObject == null)
                {
                    hasGate = false;
                }
            }
        }
        return hasGate;
    }

    public bool IsWin()
    {
        return win;
    }

    public List<Qubit> GetFinalStates()
    {
        return finalStates;
    }

    public List<List<GameObject>> GetSnapPoints()
    {
        return snapPointLists;
    }
}
