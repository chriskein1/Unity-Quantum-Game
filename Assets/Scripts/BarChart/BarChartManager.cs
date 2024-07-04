using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BarChartManager : MonoBehaviour
{
    public Slider[] BarSliders; // Array to hold references to the Slider components of the bars
    [SerializeField] private float animationDuration = 1;

    // Store references to active coroutines to stop them if necessary
    private Coroutine[] activeCoroutines;

    private void Awake()
    {
        activeCoroutines = new Coroutine[BarSliders.Length];
    }

    // Method to set the target value for a specific slider
    /// <summary>
    /// Sets the target value for a specific slider and smoothly transitions to it over the given duration.
    /// </summary>
    /// <param name="sliderIndex">The index of the slider in the BarSliders array to update.</param>
    /// <param name="targetValue">The value to which the slider should smoothly transition.</param>
    public void SetSliderValue(int sliderIndex, float targetValue)
    {
        if (sliderIndex >= 0 && sliderIndex < BarSliders.Length)
        {
            if (activeCoroutines[sliderIndex] != null)
            {
                StopCoroutine(activeCoroutines[sliderIndex]);
            }
            activeCoroutines[sliderIndex] = StartCoroutine(SmoothSliderValue(BarSliders[sliderIndex], targetValue, animationDuration));
        }
    }

    // Coroutine to smoothly transition the slider value
    private IEnumerator SmoothSliderValue(Slider slider, float targetValue, float duration)
    {
        float startValue = slider.value;
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            slider.value = Mathf.Lerp(startValue, targetValue, timeElapsed / duration);
            timeElapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        slider.value = targetValue;
    }

    public void ResetBars()
    {
        for (int i = 0; i < BarSliders.Length; i++)
        {
            SetSliderValue(i, 0f);
        }
    }

    /// <summary>
    /// Updates the bar chart with the given probabilities. Only updates as many bars as are available.
    /// </summary>
    /// <param name="probabilities">Array of probabilities to update the bar chart with.</param>
    public void UpdateBarChart(params float[] probabilities)
    {
        int count = Mathf.Min(probabilities.Length, BarSliders.Length);
        for (int i = 0; i < count; i++)
        {
            SetSliderValue(i, probabilities[i]);
        }
    }
}
