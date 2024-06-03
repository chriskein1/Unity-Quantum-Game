using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// ToolTipTrigger is responsible for showing and hiding the tooltip when the mouse hovers over the associated GameObject.
/// It delays the tooltip display by 0.5 seconds and ensures the tooltip doesn't appear if the mouse button is held down.
/// </summary>
public class ToolTipTrigger : MonoBehaviour
{
    public string content;
    [Multiline()]
    public string header;
    private bool isHovering = false; // Track if the mouse is still hovering over the object
    private bool isMouseDown = false; // Track if the mouse button is held down

    public void OnMouseEnter()
    {
        isHovering = true;
        Invoke("ShowToolTip", 0.5f); // Delay tooltip display by 0.5 seconds
    }

    public void OnMouseExit()
    {
        isHovering = false;
        CancelInvoke("ShowToolTip"); // Cancel the delayed call if the mouse exits before the tooltip is shown
        ToolTipSystem.Hide();
    }

    private void ShowToolTip()
    {
        if (isHovering && !isMouseDown) // Only show tooltip if the mouse is still hovering and not held down
        {
            ToolTipSystem.Show(content, header);
        }
    }

    public void OnMouseDown()
    {
        isMouseDown = true;
        ToolTipSystem.Hide();
    }

    public void OnMouseUp()
    {
        isMouseDown = false;
        if (isHovering)
        {
            Invoke("ShowToolTip", 0.5f); // Reinvoke the tooltip display with a delay when the mouse button is released
        }
    }
}
