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
        MoveTo100Percent,
        MoveTo0
    }

    // Method to reset all bars to the "MoveTo0" stat

    // Method to play the specified animation on a specific bar
    public void PlayAnimation(int barIndex, BarAnimationState state)
    {
        if (barIndex < 0 || barIndex >= barAnimators.Length)
        {
            Debug.LogError("Invalid bar index");
            return;
        }
        // Play the specified animation on the targeted bar
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
            case BarAnimationState.MoveTo0:
                barAnimators[barIndex].Play("MoveTo0");
                break;
            default:
                Debug.LogError("Unknown animation state");
                break;
        }

    }
    private void ResetAllBars()
    {
        foreach (Animator animator in barAnimators)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("MoveTo0"))
            {
                animator.Play("MoveTo0");
            }
        }
    }
}
