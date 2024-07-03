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

        public bool SuperPosition;

        // Constructor
        public Qubit(Complex alpha, Complex beta)
        {
            Alpha = alpha;
            Beta = beta;

            if (Alpha.Real == 0 && Alpha.Imaginary == 0
                && Beta.Real == 0 && Beta.Imaginary == 0)
            {
                // Force it to be a valid state
                Alpha = new Complex(1, 0);
            }

            // Check if qubit is in superposition
            if (Math.Abs(Alpha.Real) != 1 || Alpha.Real != 0 || Math.Abs(Alpha.Imaginary) != 1 || Alpha.Imaginary != 0
                || Math.Abs(Beta.Real) != 1 || Beta.Real != 0 || Math.Abs(Beta.Imaginary) != 1 || Beta.Imaginary != 0)
            {
                SuperPosition = true;
            }
            else
            {
                SuperPosition = false;
            }

            // Ensure normalization: |α|^2 + |β|^2 = 1
            if (Math.Abs(alpha.Magnitude * alpha.Magnitude + beta.Magnitude * beta.Magnitude - 1.0) > 1e-10)
            {
                Debug.LogError($"Alpha: {alpha}, Beta: {beta}");
                throw new ArgumentException("The qubit state must be normalized: |α|^2 + |β|^2 = 1");
            }
        }

        // Overloaded == operator
        public static bool operator ==(Qubit q1, Qubit q2)
        {
            return q1.Alpha == q2.Alpha && q1.Beta == q2.Beta;
        }

        // Overloaded != operator
        public static bool operator !=(Qubit q1, Qubit q2)
        {
            return !(q1 == q2);
        }

        // Override Equals operator
        public override bool Equals(object o) {
        if(!(o is Qubit))
            return false;
        return this ==(Qubit)o;
        }

        // Override GetHashCode
        public override int GetHashCode() {
            return Alpha.GetHashCode() ^ Beta.GetHashCode() ^ SuperPosition.GetHashCode();
        }

        // Override ToString method
        public override string ToString()
        {
            return $"Alpha: {Alpha}, Beta: {Beta}";
        }
    }
}