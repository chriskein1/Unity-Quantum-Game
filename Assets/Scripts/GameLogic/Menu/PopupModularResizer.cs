using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Loading;
using UnityEngine;
using UnityEngine.UI;

public class PopupModularResizer : MonoBehaviour
{
    // Fields for displaying header and content text
    [Header("Text:")]
    public TextMeshProUGUI headerField;

    public TextMeshProUGUI contentField;

    // Character wrap limit for enabling/disabling the layout element
    public int characterWrapLimit;


    [Header("Layout Element and RectTransform:")]
    // RectTransform of the tooltip
    public LayoutElement layoutElement;
    // Layout element to control the wrapping of text
    public RectTransform rectTransform;

    /// <summary>
    /// Initializes the RectTransform component.
    /// </summary>
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    /// <summary>
    /// Sets the text content of the tooltip.
    /// </summary>
    public void SetText(string content, string header, Color headerColor, Color contentColor)
    {
        // Toggle the header field's visibility based on whether a header is provided
        if (string.IsNullOrEmpty(header))
        {
            headerField.gameObject.SetActive(false);
        }
        else
        {
            headerField.gameObject.SetActive(true);
            headerField.color = headerColor;
            headerField.text = header;
        }

        // Set the content text
        contentField.color = contentColor;
        contentField.text = content;

        // Calculate lengths and enable the layout element if text exceeds wrap limit
        int headerLength = headerField.text.Length;
        int contentLength = contentField.text.Length;

        layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit);
    }
    
}
