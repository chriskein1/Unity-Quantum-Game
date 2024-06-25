using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChangeImageAlphaOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image targetImage; // Assign this in the Inspector
    private Color originalColor;

    private void Start()
    {
        if (targetImage != null)
        {
            // Save the original color of the target Image
            originalColor = targetImage.color;
        }
        else
        {
            Debug.LogError("No Image assigned!");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (targetImage != null)
        {
            ChangeAlpha(150f / 255f); // Alpha value of 150 out of 255
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (targetImage != null)
        {
            ResetAlpha();
        }
    }

    private void ChangeAlpha(float alpha)
    {
        if (targetImage != null)
        {
            Color newColor = targetImage.color;
            newColor.a = alpha;
            targetImage.color = newColor;
        }
    }

    private void ResetAlpha()
    {
        if (targetImage != null)
        {
            targetImage.color = originalColor;
        }
    }
}
