using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using QubitType;
public class TwoQubitTile : MonoBehaviour
{
    [Header("Starting Qubit State")]
    [SerializeField] Qubit[] qubits = new Qubit[2];
    [SerializeField] GameObject OutputTile;
    [SerializeField] GameObject SuperPositionTile;
    [SerializeField] GameObject TwoSuperPositionTile;
    [SerializeField] GameObject Numerator1;
    [SerializeField] GameObject States1;
    [SerializeField] GameObject Numerator2;
    [SerializeField] GameObject States2;
    private TextMeshPro outputText;
    private TextMeshPro numeratorText1;
    private TextMeshPro statesText1;
    private TextMeshPro numeratorText2;
    private TextMeshPro statesText2;

    string qubitStr = "";
    
    void Start()
    {
        if (OutputTile == null || SuperPositionTile == null 
            || Numerator1 == null || States1 == null 
            || Numerator2 == null || States2 == null)
        {
            return;
        }

        // Prevent -0
        if (qubits[0].state == 0 && !qubits[0].PositiveState && !qubits[0].ImaginaryState)
        {
            qubits[0].PositiveState = true;
        }
        if (qubits[1].state == 0 && !qubits[1].PositiveState && !qubits[1].ImaginaryState)
        {
            qubits[1].PositiveState = true;
        }
        
        outputText = OutputTile.GetComponentInChildren<TextMeshPro>();
        numeratorText1 = Numerator1.GetComponentInChildren<TextMeshPro>();
        statesText1 = States1.GetComponentInChildren<TextMeshPro>();
        numeratorText2 = Numerator2.GetComponentInChildren<TextMeshPro>();
        statesText2 = States2.GetComponentInChildren<TextMeshPro>();
        UpdateText();
    }
    // updates sprites text 
    void UpdateText()
    {
        qubitStr = "";
        string numeratorStr1 = "";
        string numeratorStr2 = "";

        // No H gate on either qubit
        if (!qubits[0].HApplied && !qubits[1].HApplied)
        {
            // Negative sign only if one qubit is negative
            if ((!qubits[0].PositiveState && qubits[1].PositiveState)
                || (qubits[0].PositiveState && !qubits[1].PositiveState))
            {
                qubitStr += "-";
            }
           
            // No negative sign if both qubits are negative

            // Imaginary part only if one qubit has an imaginary part
            if ((qubits[0].ImaginaryState && !qubits[1].ImaginaryState)
                || (!qubits[0].ImaginaryState && qubits[1].ImaginaryState))
            {
                qubitStr += "i";
            }
            // If both qubits have imaginary parts, i*i = -1
            else if (qubits[0].ImaginaryState && qubits[1].ImaginaryState)
            {
                qubitStr += "-";
            }

            // Combine the states of the two qubits and whatever sign and imaginary part they have
            qubitStr += "|" + qubits[0].state.ToString() + qubits[1].state.ToString() + ">";
            outputText.text = qubitStr;

            // Set the standard output tile active
            OutputTile.SetActive(true);
            SuperPositionTile.SetActive(false);
            TwoSuperPositionTile.SetActive(false);
        }
        // If both H gates are active
        else if (qubits[0].HApplied && qubits[1].HApplied)
        {
            // Determine if numerator is imaginary
            if ((qubits[0].ImaginaryState && !qubits[1].ImaginaryState)
                || (!qubits[0].ImaginaryState && qubits[1].ImaginaryState))
            {
                numeratorStr2 = "i";
            }
            else {
                numeratorStr2 = "1";
            }

            bool negativeSign = false;
            // Determine if numerator is negative
            if (qubits[0].PositiveState != qubits[1].PositiveState)
            {
                negativeSign = true;
            }
            // i * i = -1
            if (qubits[0].ImaginaryState && qubits[1].ImaginaryState)
            {
                negativeSign = !negativeSign;
            }

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
                qubitStr = "(|00>+|01>+|10>+|11>)";
            }
            // 2)
            else if ((qubits[0].state == 0 && qubits[1].state == 1 && !negativeSign)
                    || (qubits[0].state == 1 && qubits[1].state == 0 && negativeSign))
            {
                qubitStr = "(|00>-|01>+|10>-|11>)";
            }
            // 3)
            else if ((qubits[0].state == 1 && qubits[1].state == 0 && !negativeSign)
                    || (qubits[0].state == 0 && qubits[1].state == 1 && negativeSign))
            {
                qubitStr = "(|00>+|01>-|10>-|11>)";
            }
            // 4)
            else if ((qubits[0].state == 1 && qubits[1].state == 1 && !negativeSign)
                    || (qubits[0].state == 0 && qubits[1].state == 0 && negativeSign))
            {
                qubitStr = "(|00>-|01>-|10>+|11>)";
            }

            // Active the two superposition tile and deactivate others
            TwoSuperPositionTile.SetActive(true);
            OutputTile.SetActive(false);
            SuperPositionTile.SetActive(false);

            numeratorText2.text = numeratorStr2;
            statesText2.text = qubitStr;
        }
        // Only q1 is in superposition
        else if (qubits[0].HApplied && !qubits[1].HApplied)
        {
            // Determine if numerator is imaginary
            if (qubits[0].ImaginaryState && !qubits[1].ImaginaryState 
                || !qubits[0].ImaginaryState && qubits[1].ImaginaryState)
            {
                numeratorStr1 = "i";
            }
            else {
                numeratorStr1 = "1";
            }

            bool negativeSign = false;
            // Determine if sign between bits is negative
            // H gate is on q1
            // 1) H|1> both qubits positive
            // 2) H|0> only one qubit negative
            if (qubits[0].state == 1 && qubits[0].PositiveState && qubits[1].PositiveState
                || qubits[0].state == 0 && !qubits[0].PositiveState && qubits[1].PositiveState
                || qubits[0].state == 0 && qubits[0].PositiveState && !qubits[1].PositiveState
                )
            {
                negativeSign = true;
            }
            // i * i = -1
            if (qubits[0].ImaginaryState && qubits[1].ImaginaryState)
            {
                negativeSign = !negativeSign;
            }

            string sign = negativeSign ? "-" : "+";

            qubitStr = $"(|0{qubits[1].state}>{sign}|1{qubits[1].state}>)";

            // Active the superposition tile and deactivate others
            SuperPositionTile.SetActive(true);
            OutputTile.SetActive(false);
            TwoSuperPositionTile.SetActive(false);

            numeratorText1.text = numeratorStr1;
            statesText1.text = qubitStr;
        }

        // Only q2 is in superposition
        else if (!qubits[0].HApplied && qubits[1].HApplied)
        {
            // Determine if numerator is imaginary
            if (qubits[0].ImaginaryState && !qubits[1].ImaginaryState 
                || !qubits[0].ImaginaryState && qubits[1].ImaginaryState)
            {
                numeratorStr1 = "i";
            }
            else {
                numeratorStr1 = "1";
            }

            bool negativeSign = false;
            // Determine if sign between bits is negative
            // H gate is on q2
            // 1) H|1> both qubits positive
            // 2) H|0> only one qubit negative
            if (qubits[1].state == 1 && qubits[0].PositiveState && qubits[1].PositiveState
                || qubits[1].state == 0 && !qubits[0].PositiveState && qubits[1].PositiveState
                || qubits[1].state == 0 && qubits[0].PositiveState && !qubits[1].PositiveState
                )
            {
                negativeSign = true;
            }
            // i * i = -1
            if (qubits[0].ImaginaryState && qubits[1].ImaginaryState)
            {
                negativeSign = !negativeSign;
            }

            string sign = negativeSign ? "-" : "+";

            qubitStr = $"(|{qubits[0].state}0>{sign}|{qubits[0].state}1>)";

            // Active the superposition tile and deactivate others
            SuperPositionTile.SetActive(true);
            OutputTile.SetActive(false);
            TwoSuperPositionTile.SetActive(false);

            numeratorText1.text = numeratorStr1;
            statesText1.text = qubitStr;
        }
    }    

    // Function to set the state and its sign
    public void SetState(Qubit q1, Qubit q2)
    {
        qubits[0] = q1;
        qubits[1] = q2;
        UpdateText();
    }

    public string GetStr()
    {
        return qubitStr;
    }
}