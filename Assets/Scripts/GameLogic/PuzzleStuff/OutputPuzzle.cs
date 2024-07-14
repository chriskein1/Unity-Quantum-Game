using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputPuzzle : MonoBehaviour
{
    public List<Snap> outputSnaps = new List<Snap>();
    public List<SingleQubitStateOptions> correctOutput = new List<SingleQubitStateOptions>();

    public List<Snap> GetOutputSnaps()
    {
        return outputSnaps;
    }

    public List<SingleQubitStateOptions> GetCorrectOutput()
    {
        return correctOutput;
    }
}
