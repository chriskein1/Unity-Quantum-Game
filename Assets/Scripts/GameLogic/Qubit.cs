using UnityEngine;

namespace QubitType
    {
    [System.Serializable]
    // Qubit structure
    public struct Qubit
    {
        public int state;
        public bool PositiveState;
        public bool ImaginaryState;
        public bool HApplied;

        // Overloaded == operator
        public static bool operator ==(Qubit q1, Qubit q2)
        {
            return q1.state == q2.state && q1.PositiveState == q2.PositiveState && q1.ImaginaryState == q2.ImaginaryState && q1.HApplied == q2.HApplied;
        }

        // Overloaded != operator
        public static bool operator !=(Qubit q1, Qubit q2)
        {
            return q1.state != q2.state || q1.PositiveState != q2.PositiveState || q1.ImaginaryState != q2.ImaginaryState || q1.HApplied != q2.HApplied;
        }

        // Override Equals operator
        public override bool Equals(object o) {
        if(!(o is Qubit))
            return false;
        return this ==(Qubit)o;
        }

        // Override GetHashCode
        public override int GetHashCode() {
            return state.GetHashCode() ^ PositiveState.GetHashCode() ^ ImaginaryState.GetHashCode() ^ HApplied.GetHashCode();
        }
    }
}