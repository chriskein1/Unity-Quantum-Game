using System.Collections;
using UnityEngine;
using TMPro;

public class TextColorChanger : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;
    public Color[] colors;
    public float duration = 1f;

    private int currentColorIndex = 0;

    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        if (colors.Length < 4)
        {
            Debug.LogError("Please assign at least 4 colors in the inspector.");
            return;
        }
        StartCoroutine(FadeToNextColor());
    }

    IEnumerator FadeToNextColor()
    {
        while (true)
        {
            Color startColor = textMeshPro.color;
            Color endColor = colors[currentColorIndex];
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                textMeshPro.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            textMeshPro.color = endColor;
            currentColorIndex = (currentColorIndex + 1) % colors.Length;
            yield return new WaitForSeconds(duration);
        }
    }
}