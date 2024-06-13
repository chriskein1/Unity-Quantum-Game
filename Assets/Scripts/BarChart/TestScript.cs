using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestScript : MonoBehaviour
{
    public BarChartManager barChartManager;

    // Methods to handle button clicks
    public void OnButton25Click()
    {
        TriggerAnimation(BarChartManager.BarAnimationState.MoveTo25Percent);
    }

    public void OnButton50Click()
    {
        TriggerAnimation(BarChartManager.BarAnimationState.MoveTo50Percent);
    }

    public void OnButton75Click()
    {
        TriggerAnimation(BarChartManager.BarAnimationState.MoveTo75Percent);
    }

    public void OnButton100Click()
    {
        TriggerAnimation(BarChartManager.BarAnimationState.MoveTo100Percent);
    }

    // Method to trigger animation on all bars
    private void TriggerAnimation(BarChartManager.BarAnimationState state)
    {
        for (int i = 0; i < barChartManager.barAnimators.Length; i++)
        {
            barChartManager.PlayAnimation(i, state);
        }
    }
}
