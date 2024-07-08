using System;
using System.Numerics;
using UnityEngine;
using QubitType;
using UnityEngine.Events;

public enum SingleQubitStateOptions
{
    State0,  // |0⟩
    State1,  // |1⟩
    Imaginary1, //i|1⟩
    Imaginary0, //i|0⟩
    NegativeState1,  // -|1⟩
    NegativeState0,
    NegativeImaginary1, //-i|1⟩
    NegativeImaginary0, //-i|1⟩
    SuperpositionPlus,  // |+⟩ = 1/√2 (|0⟩ + |1⟩)
    SuperpositionMinus, // |−⟩ = 1/√2 (|0⟩ - |1⟩)
    ImaginarySuperpositionPlus, // i/√2 (|0⟩ + |1⟩)
    ImaginarySuperpositionMinus, // i/√2 (|0⟩ - |1⟩)
    NoState
}

public class QubitOperations
{
    public UnityEvent OutputChanged; // Event for qubit controller

    public Qubit ConvertToQubit(SingleQubitStateOptions state)
    {
        switch (state)
        {
            case SingleQubitStateOptions.State0:
                return new Qubit(new Complex(1, 0), new Complex(0, 0));
            case SingleQubitStateOptions.State1:
                return new Qubit(new Complex(0, 0), new Complex(1, 0));
            case SingleQubitStateOptions.Imaginary0:
                return new Qubit(new Complex(1, 0), new Complex(0, 1));
            case SingleQubitStateOptions.Imaginary1:
                return new Qubit(new Complex(0, 0), new Complex(0, 1));
            case SingleQubitStateOptions.NegativeState1:
                return new Qubit(new Complex(0, 0), new Complex(-1, 0));
            case SingleQubitStateOptions.NegativeImaginary1:
                return new Qubit(new Complex(0, 0), new Complex(0, -1));
            case SingleQubitStateOptions.NegativeImaginary0:
                return new Qubit(new Complex(0, -1), new Complex(0, 0));
            case SingleQubitStateOptions.SuperpositionPlus:
                return new Qubit(new Complex(1 / Math.Sqrt(2), 0), new Complex(1 / Math.Sqrt(2), 0));
            case SingleQubitStateOptions.SuperpositionMinus:
                return new Qubit(new Complex(1 / Math.Sqrt(2), 0), new Complex(-1 / Math.Sqrt(2), 0));
            case SingleQubitStateOptions.ImaginarySuperpositionPlus:
                return new Qubit(new Complex(0, 1 / Math.Sqrt(2)), new Complex(0, 1 / Math.Sqrt(2)));
            case SingleQubitStateOptions.ImaginarySuperpositionMinus:
                return new Qubit(new Complex(0, -1 / Math.Sqrt(2)), new Complex(0, 1 / Math.Sqrt(2)));
            case SingleQubitStateOptions.NoState:
                return default; // No state
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

    public SingleQubitStateOptions ConvertToStateOption(Qubit qubit)
    {
        double epsilon = 0.0001; // Small threshold for floating-point comparison

        Debug.Log($"Alpha: ({qubit.Alpha.Real}, {qubit.Alpha.Imaginary}), Beta: ({qubit.Beta.Real}, {qubit.Beta.Imaginary})");

        if (Math.Abs(qubit.Alpha.Real - 1) < epsilon && Math.Abs(qubit.Alpha.Imaginary) < epsilon && Math.Abs(qubit.Beta.Real) < epsilon && Math.Abs(qubit.Beta.Imaginary) < epsilon)
        {
            return SingleQubitStateOptions.State0;
        }
        if (Math.Abs(qubit.Alpha.Real) < epsilon && Math.Abs(qubit.Alpha.Imaginary) < epsilon && Math.Abs(qubit.Beta.Real - 1) < epsilon && Math.Abs(qubit.Beta.Imaginary) < epsilon)
        {
            return SingleQubitStateOptions.State1;
        }
        if (Math.Abs(qubit.Alpha.Real - 1) < epsilon && Math.Abs(qubit.Alpha.Imaginary) < epsilon && Math.Abs(qubit.Beta.Real) < epsilon && Math.Abs(qubit.Beta.Imaginary - 1) < epsilon)
        {
            return SingleQubitStateOptions.Imaginary0;
        }
        if (Math.Abs(qubit.Alpha.Real) < epsilon && Math.Abs(qubit.Alpha.Imaginary) < epsilon && Math.Abs(qubit.Beta.Real) < epsilon && Math.Abs(qubit.Beta.Imaginary - 1) < epsilon)
        {
            return SingleQubitStateOptions.Imaginary1;
        }
        if (Math.Abs(qubit.Alpha.Real + 1) < epsilon && Math.Abs(qubit.Alpha.Imaginary) < epsilon && Math.Abs(qubit.Beta.Real) < epsilon && Math.Abs(qubit.Beta.Imaginary) < epsilon)
        {
            return SingleQubitStateOptions.NegativeState0;  // Added for negative |0>
        }
        if (Math.Abs(qubit.Alpha.Real) < epsilon && Math.Abs(qubit.Alpha.Imaginary) < epsilon && Math.Abs(qubit.Beta.Real + 1) < epsilon && Math.Abs(qubit.Beta.Imaginary) < epsilon)
        {
            return SingleQubitStateOptions.NegativeState1;
        }
        if (Math.Abs(qubit.Alpha.Real) < epsilon && Math.Abs(qubit.Alpha.Imaginary + 1) < epsilon && Math.Abs(qubit.Beta.Real) < epsilon && Math.Abs(qubit.Beta.Imaginary) < epsilon)
        {
            return SingleQubitStateOptions.NegativeImaginary0;
        }
        if (Math.Abs(qubit.Alpha.Real) < epsilon && Math.Abs(qubit.Alpha.Imaginary) < epsilon && Math.Abs(qubit.Beta.Real) < epsilon && Math.Abs(qubit.Beta.Imaginary + 1) < epsilon)
        {
            return SingleQubitStateOptions.NegativeImaginary1;
        }
        if (Math.Abs(qubit.Alpha.Imaginary - 1 / Math.Sqrt(2)) < epsilon && Math.Abs(qubit.Beta.Imaginary - 1 / Math.Sqrt(2)) < epsilon)
        {
            return SingleQubitStateOptions.ImaginarySuperpositionPlus;
        }
        if (Math.Abs(qubit.Alpha.Imaginary + 1 / Math.Sqrt(2)) < epsilon && Math.Abs(qubit.Beta.Imaginary + 1 / Math.Sqrt(2)) < epsilon)
        {
            return SingleQubitStateOptions.ImaginarySuperpositionMinus;
        }
        if (qubit.IsInSuperposition())
        {
            return qubit.Beta.Real < 0 ? SingleQubitStateOptions.SuperpositionMinus : SingleQubitStateOptions.SuperpositionPlus;
        }

        return SingleQubitStateOptions.NoState; // Default fallback
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