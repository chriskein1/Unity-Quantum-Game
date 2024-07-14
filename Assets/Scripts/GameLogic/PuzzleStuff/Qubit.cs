using UnityEngine;
using System.Numerics;
using System;

namespace QubitType
{
    [System.Serializable]
    // Qubit structure
    public struct Qubit
    {
        // Probability amplitude of 0
        public Complex Alpha;
        // Probability amplitude of 1
        public Complex Beta;

        
        // Constructor
        public Qubit(Complex alpha, Complex beta)
        {
            // Normalize the state if needed
            double magnitude = Math.Sqrt(alpha.Magnitude * alpha.Magnitude + beta.Magnitude * beta.Magnitude);
            if (Math.Abs(magnitude - 1.0) > 1e-10)
            {
                alpha /= magnitude;
                beta /= magnitude;
            }

            Alpha = alpha;
            Beta = beta;

            // Force it to be a valid state if both Alpha and Beta are zero
            if (Alpha.Real == 0 && Alpha.Imaginary == 0 && Beta.Real == 0 && Beta.Imaginary == 0)
            {
                Alpha = new Complex(1, 0);
            }

            // Ensure normalization: |α|^2 + |β|^2 = 1
            if (Math.Abs(Alpha.Magnitude * Alpha.Magnitude + Beta.Magnitude * Beta.Magnitude - 1.0) > 1e-10)
            {
                Debug.LogError($"Alpha: {Alpha}, Beta: {Beta}");
                throw new ArgumentException("The qubit state must be normalized: |α|^2 + |β|^2 = 1");
            }
        }
        public Qubit Measure()
        {
            float prob0 = (float)(Alpha.Real * Alpha.Real + Alpha.Imaginary * Alpha.Imaginary);
            float rand = UnityEngine.Random.Range(0.0f, 1.0f);
            Qubit resultQubit;

            if (rand < prob0)
            {
                // Collapse to state |0⟩
                resultQubit = new Qubit(new Complex(1, 0), new Complex(0, 0));
            }
            else
            {
                // Collapse to state |1⟩
                resultQubit = new Qubit(new Complex(0, 0), new Complex(1, 0));
            }

            // Update the current qubit state to match the measurement result
            this.Alpha = resultQubit.Alpha;
            this.Beta = resultQubit.Beta;

            return resultQubit;
        }
        // Method to check if the qubit is in superposition
        public bool IsInSuperposition()
        {
            double epsilon = 0.0001; // Small threshold for floating-point comparison
            return !(Math.Abs(Alpha.Magnitude - 1) < epsilon && Math.Abs(Beta.Magnitude) < epsilon) &&
                   !(Math.Abs(Beta.Magnitude - 1) < epsilon && Math.Abs(Alpha.Magnitude) < epsilon);
        }

        // Overloaded == operator
        public static bool operator ==(Qubit q1, Qubit q2)
        {
            return q1.Equals(q2);
        }

        // Overloaded != operator
        public static bool operator !=(Qubit q1, Qubit q2)
        {
            return !(q1 == q2);
        }

        // Override Equals operator
        public override bool Equals(object o)
        {
            if (!(o is Qubit))
                return false;
            return this == (Qubit)o;
        }

        // Override GetHashCode
        public override int GetHashCode()
        {
            return Alpha.GetHashCode() ^ Beta.GetHashCode();
        }

        // Override ToString method
        public override string ToString()
        {
            return $"Alpha: {Alpha}, Beta: {Beta}";
        }

        // Method to compare two qubits with a tolerance for floating-point precision
        public bool IsApproximatelyEqual(Qubit other, double epsilon = 1e-10)
        {
            return Complex.Abs(Alpha - other.Alpha) < epsilon && Complex.Abs(Beta - other.Beta) < epsilon;
        }
    }
}
