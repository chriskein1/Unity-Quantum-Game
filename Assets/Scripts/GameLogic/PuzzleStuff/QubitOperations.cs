using System;
using System.Numerics;
using UnityEngine;
using QubitType;
using UnityEngine.Events;
public enum WinStateOptions
{
    State0,  // |0⟩
    State1,  // |1⟩
    SuperpositionPlus,  // |+⟩ = 1/√2 (|0⟩ + |1⟩)
    SuperpositionMinus, // |−⟩ = 1/√2 (|0⟩ - |1⟩)
    ComplexSuperposition1, // 1/√3 |0⟩ + √(2/3) |1⟩
    ComplexSuperposition2, // 1/2 |0⟩ + √3/2 |1⟩
    NoWinState // No win state
}

public enum StartingStateOptions
{
    State0,  // |0⟩
    State1,  // |1⟩
}




public class QubitOperations
{

    public UnityEvent OutputChanged; // Event for qubit controller
    public Qubit ConvertToQubit(StartingStateOptions state)
    {
        switch (state)
        {
            case StartingStateOptions.State0:
                return new Qubit(new Complex(1, 0), new Complex(0, 0));
            case StartingStateOptions.State1:
                return new Qubit(new Complex(0, 0), new Complex(1, 0));
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public Qubit ConvertToQubit(WinStateOptions state)
    {
        switch (state)
        {
            case WinStateOptions.State0:
                return new Qubit(new Complex(1, 0), new Complex(0, 0));
            case WinStateOptions.State1:
                return new Qubit(new Complex(0, 0), new Complex(1, 0));
            case WinStateOptions.SuperpositionPlus:
                return new Qubit(new Complex(1 / Mathf.Sqrt(2), 0), new Complex(1 / Mathf.Sqrt(2), 0));
            case WinStateOptions.SuperpositionMinus:
                return new Qubit(new Complex(1 / Mathf.Sqrt(2), 0), new Complex(-1 / Mathf.Sqrt(2), 0));
            case WinStateOptions.ComplexSuperposition1:
                return new Qubit(new Complex(1 / Mathf.Sqrt(3), 0), new Complex(Mathf.Sqrt(2 / 3f), 0));
            case WinStateOptions.ComplexSuperposition2:
                return new Qubit(new Complex(1 / 2f, 0), new Complex(Mathf.Sqrt(3) / 2f, 0));
            case WinStateOptions.NoWinState:
                return default; // No win state
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void ApplyGateOperation(GameObject gateObject, ref Qubit state)
    {
        if (gateObject == null)
        {
           
            return;
        }

        switch (gateObject.tag)
        {
            case "XGate":
                
                state = QuantumGates.ApplyPauliX(state);
                break;

            case "YGate":
                
                state = QuantumGates.ApplyPauliY(state);
                break;

            case "ZGate":
                
                state = QuantumGates.ApplyPauliZ(state);
                break;

            case "HGate":
                
                state = QuantumGates.ApplyHadamard(state);
                break;
        }
    }
}
