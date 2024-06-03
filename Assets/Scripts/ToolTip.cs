using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
/// <summary>
/// This class manages a tooltip, including setting its text content and positioning it relative to the mouse cursor.
/// </summary>
public class ToolTip : MonoBehaviour
{
    // Fields for displaying header and content text
    public TextMeshProUGUI headerField;
    public TextMeshProUGUI contentField;

    // Layout element to control the wrapping of text
    public LayoutElement layoutElement;

    // Character wrap limit for enabling/disabling the layout element
    public int characterWrapLimit;

    // RectTransform of the tooltip
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
    public void SetText(string content, string header = "")
    {
        // Toggle the header field's visibility based on whether a header is provided
        if (string.IsNullOrEmpty(header))
        {
            headerField.gameObject.SetActive(false);
        }
        else
        {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }

        // Set the content text
        contentField.text = content;

        // Calculate lengths and enable the layout element if text exceeds wrap limit
        int headerLength = headerField.text.Length;
        int contentLength = contentField.text.Length;

        layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit);
    }

    /// <summary>
    /// Updates the tooltip's position and layout in the editor and during runtime.
    /// </summary>
    private void Update()
    {
        if (Application.isEditor)
        {
            // Update layout element in the editor
            int headerLength = headerField.text.Length;
            int contentLength = contentField.text.Length;

            layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit);
        }

        // Get mouse position in screen coordinates
        Vector2 position = Input.mousePosition;
        float x = position.x / Screen.width;
        float y = position.y / Screen.height;

        // Determine pivot based on mouse position relative to screen edges
        if (x <= y && x <= 1 - y) // left
            rectTransform.pivot = new Vector2(-0.15f, y);
        else if (x >= y && x <= 1 - y) // bottom
            rectTransform.pivot = new Vector2(x, -0.1f);
        else if (x >= y && x >= 1 - y) // right
            rectTransform.pivot = new Vector2(1.1f, y);
        else if (x <= y && x >= 1 - y) // top
            rectTransform.pivot = new Vector2(x, 1.3f);

        // Set tooltip position to mouse position
        transform.position = position;
    }
}
