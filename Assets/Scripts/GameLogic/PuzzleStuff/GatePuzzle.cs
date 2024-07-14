using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using QubitType;
public class GatePuzzle : MonoBehaviour
{
    public List<CircuitManager> guessTheGateManagers;

    public List<CircuitManager> GetCircuitManagers()
    {
        return guessTheGateManagers;
    } 
}
