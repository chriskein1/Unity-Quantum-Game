using QubitType;
using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;


public static class QuantumGates
{
   
    public static Qubit ApplyHadamard(Qubit qubit)
    {
        // The Hadamard matrix is 1/√2 * [[1, 1], [1, -1]]
        Complex newAlpha = (qubit.Alpha + qubit.Beta) / Math.Sqrt(2);
        Complex newBeta = (qubit.Alpha - qubit.Beta) / Math.Sqrt(2);

        return new Qubit(newAlpha, newBeta);
    }

    public static Qubit ApplyPauliX(Qubit qubit)
    {
        // The Pauli-X matrix is [[0, 1], [1, 0]]
        Complex newAlpha = qubit.Beta;
        Complex newBeta = qubit.Alpha;

        return new Qubit(newAlpha, newBeta);
    }

    public static Qubit ApplyPauliY(Qubit qubit)
    {
        // The Pauli-Y matrix is [[0, -i], [i, 0]]
        Complex newAlpha = -Complex.ImaginaryOne * qubit.Beta;
        Complex newBeta = Complex.ImaginaryOne * qubit.Alpha;

        return new Qubit(newAlpha, newBeta);
    }

    public static Qubit ApplyPauliZ(Qubit qubit)
    {
        // The Pauli-Z matrix is [[1, 0], [0, -1]]
        Complex newAlpha = qubit.Alpha;
        Complex newBeta = -qubit.Beta;

        return new Qubit(newAlpha, newBeta);
    }
}
