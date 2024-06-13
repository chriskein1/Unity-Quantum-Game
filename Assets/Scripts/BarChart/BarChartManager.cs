using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarChartManager : MonoBehaviour
{
    public Animator[] barAnimators; // Array to hold references to the Animator components of the bars

    // Enum to define the different animation states
    public enum BarAnimationState
    {
        MoveTo25Percent,
        MoveTo50Percent,
        MoveTo75Percent,
        MoveTo100Percent
    }

    // Method to play the specified animation on a specific bar
    public void PlayAnimation(int barIndex, BarAnimationState state)
    {
        if (barIndex < 0 || barIndex >= barAnimators.Length)
        {
            Debug.LogError("Invalid bar index");
            return;
        }

        switch (state)
        {
            case BarAnimationState.MoveTo25Percent:
                barAnimators[barIndex].Play("MoveTo25Percent");
                break;
            case BarAnimationState.MoveTo50Percent:
                barAnimators[barIndex].Play("MoveTo50Percent");
                break;
            case BarAnimationState.MoveTo75Percent:
                barAnimators[barIndex].Play("MoveTo75Percent");
                break;
            case BarAnimationState.MoveTo100Percent:
                barAnimators[barIndex].Play("MoveTo100Percent");
                break;
            default:
                Debug.LogError("Unknown animation state");
                break;
        }
    }
}
