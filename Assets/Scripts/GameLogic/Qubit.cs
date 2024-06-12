using UnityEngine;

namespace QubitType
    {
    [System.Serializable]
    // Qubit structure
    public struct Qubit
    {
        public int state;
        public bool PositiveState;
        public bool SuperPosition;
        public bool HApplied;
    }
}