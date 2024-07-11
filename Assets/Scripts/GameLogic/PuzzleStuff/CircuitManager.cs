using System.Collections.Generic;
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

        // Evaluate each column from left to right
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

                EvaluateMultipleTimes(100);
            }
        }

        bool win = false;

        // Update the visual outputs and evaluate the win condition
        if (qubitWireControllers.Count >= 2)
        {
            Qubit finalStateQubit1 = snapPointStates[0][numColumns - 1];
            Qubit finalStateQubit2 = snapPointStates[1][numColumns - 1];
            //UpdateBarChart(finalStateQubit1, finalStateQubit2);
            win = EvaluateWin(new List<Qubit> { finalStateQubit1, finalStateQubit2 });
            visualOutput[0].SetQubit(finalStateQubit1, 0);
            visualOutput[1].SetQubit(finalStateQubit2, 1);
            Debug.Log("Setting output");
            Debug.Log("Final state is " + finalStateQubit1 + " and " + finalStateQubit2);
        }
        else if (qubitWireControllers.Count == 1)
        {
            Qubit finalStateQubit = snapPointStates[0][numColumns - 1];
            

            //UpdateBarChartSingle(finalStateQubit);
            win = EvaluateWin(new List<Qubit> { finalStateQubit });
            visualOutput[0].SetQubit(finalStateQubit, 0);
            Debug.Log("Setting output 0");
            Debug.Log("Final state is " + finalStateQubit);
        }

        if (win)
        {
            winScreen.SetActive(true);
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
                visualInput[i].SetQubit(inputQubit, i);
            }
        }
    }

    //private void UpdateBarChart(Qubit finalStateQubit1, Qubit finalStateQubit2)
    //{
    //    // Calculate probabilities for the states |00⟩, |01⟩, |10⟩, and |11⟩
    //    float prob00 = MagnitudeSquared(finalStateQubit1.Alpha * finalStateQubit2.Alpha);
    //    float prob01 = MagnitudeSquared(finalStateQubit1.Alpha * finalStateQubit2.Beta);
    //    float prob10 = MagnitudeSquared(finalStateQubit1.Beta * finalStateQubit2.Alpha);
    //    float prob11 = MagnitudeSquared(finalStateQubit1.Beta * finalStateQubit2.Beta);

    //    barChartManager.UpdateBarChart(prob00, prob01, prob10, prob11);
    //}

    //private float MagnitudeSquared(Complex c)
    //{
    //    return (float)(c.Real * c.Real + c.Imaginary * c.Imaginary);
    //}
    //private void UpdateBarChartSingle(Qubit finalStateQubit)
    //{
    //    // Calculate probabilities for the states |0> and |1>
    //    float prob0 = MagnitudeSquared(finalStateQubit.Alpha);
    //    float prob1 = MagnitudeSquared(finalStateQubit.Beta);

    //    // Update the bar chart using the same method but with only two bars
    //    barChartManager.UpdateBarChart(prob0, prob1);
    //}

    // Determine if the win condition is met for the qubits
    private bool EvaluateWin(List<Qubit> finalStates)
    {
        if (winState.Count == 0)
        {
            Debug.LogError("Win state is not initialized.");
            return false;
        }
        else if (finalStates.Count != winState.Count)
        {
            Debug.LogError("Final states and win state do not match in length.");
            return false;
        }
        // If either win qubit is "no state"
        if (winState.Contains(SingleQubitStateOptions.NoState))
        {
            return false;
        }

        if(qubitWireControllers.Count == 1)
        {
            return winState[0] == qubitOperations.ConvertToStateOption(finalStates[0]); 
        }
        List<Qubit> winStateQ = new List<Qubit>();
        foreach (SingleQubitStateOptions state in winState)
        {
            winStateQ.Add(qubitOperations.ConvertToQubit(state));
        }

        if (winStateQ.Count == 1)
        {
            // Check if they are approximately equal
            return finalStates[0].IsApproximatelyEqual(winStateQ[0]);
        }
        else if (winStateQ.Count == 2)
        {
            // Imaginary and negative values can be on either qubit, so check both possibilities
            bool imaginaryWin = false;
            bool imaginaryFinal = false;
            for (int i = 0; i < 2; i++)
            {
                Qubit q = winStateQ[i];
                Qubit qFinal = finalStates[i];
                if (q.Alpha.Imaginary != 0)
                {
                    imaginaryWin = !imaginaryWin;
                } 
                if (q.Beta.Imaginary != 0)
                {
                    imaginaryWin = !imaginaryWin;
                }
                if (qFinal.Alpha.Imaginary != 0)
                {
                    imaginaryFinal = !imaginaryFinal;
                }
                if (qFinal.Beta.Imaginary != 0)
                {
                    imaginaryFinal = !imaginaryFinal;
                }
            }
            bool negativeWin = false;
            bool negativeFinal = false;
            for (int i = 0; i < 2; i++)
            {
                Qubit q = winStateQ[i];
                Qubit finalQ = finalStates[i];
                if (q.Alpha.Real < 0 || q.Alpha.Imaginary < 0)
                {
                    negativeWin = !negativeWin;
                } 
                if (q.Beta.Real < 0 || q.Beta.Imaginary < 0)
                {
                    negativeWin = !negativeWin;
                }
                if (finalQ.Alpha.Real < 0 || finalQ.Alpha.Imaginary < 0)
                {
                    negativeFinal = !negativeFinal;
                }
                if (finalQ.Beta.Real < 0 || finalQ.Beta.Imaginary < 0)
                {
                    negativeFinal = !negativeFinal;
                }
            }

            // If both win states or final states are imaginary, flip negative values
            // i * i = -1
            if ((winStateQ[0].Alpha.Imaginary != 0 || winStateQ[0].Beta.Imaginary != 0)
                && (winStateQ[1].Alpha.Imaginary != 0 || winStateQ[1].Beta.Imaginary != 0))
                {
                    negativeWin = !negativeWin;
                }
            
            if ((finalStates[0].Alpha.Imaginary != 0 || finalStates[0].Beta.Imaginary != 0)
                && (finalStates[1].Alpha.Imaginary != 0 || finalStates[1].Beta.Imaginary != 0))
                {
                    negativeFinal = !negativeFinal;
                }

            // If the real parts are equal and the signs and imaginary parts are equal, the qubits are equal
            if (!imaginaryWin && !imaginaryFinal)
            return (CompareReal(finalStates[0], winStateQ[0]) && CompareReal(finalStates[1], winStateQ[1]) &&
                   imaginaryWin == imaginaryFinal && negativeWin == negativeFinal);
        }
        return false;
    }

    private bool CompareReal(Qubit q1, Qubit q2, double tolerance = 1e-10)
    {
        // Compare positive values for alpha and beta
        return (Math.Abs(q1.Alpha.Real - q2.Alpha.Real) < tolerance && Math.Abs(q1.Beta.Real - q2.Beta.Real) < tolerance);
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
            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numColumns; col++)
                {
                    snapPointStates[row][col] = new Qubit();
                }
            }

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
            // barChartManager.UpdateBarChart(stateProbabilities["|00⟩"], stateProbabilities["|01⟩"], stateProbabilities["|10⟩"], stateProbabilities["|11⟩"]);
        }
        else if (numRows == 1)
        {
            // barChartManager.UpdateBarChart(stateProbabilities["|00⟩"], stateProbabilities["|01⟩"]);
        }

        // Output probabilities
        foreach (var state in stateProbabilities)
        {
            Debug.Log($"{state.Key}: {state.Value * 100}%");
        }
    }
}
