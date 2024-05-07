using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ToolTipTrigger : MonoBehaviour
{
    public string content;
    [Multiline()]
    public string header;
    private bool isHovering = false; //track if the mouse is still hovering over the object

    public void OnMouseEnter()
    {
        isHovering = true;
        Invoke("ShowToolTip", 0.5f); //Delay tooltip display by 0.5 seconds
    }

    public void OnMouseExit()
    {
        isHovering = false;
        CancelInvoke("ShowToolTip"); //Cancel the delayed call if the mouse exits before the tooltip is shown
        ToolTipSystem.Hide();
    }

    private void ShowToolTip()
    {
        if (isHovering) //Only show tooltip if the mouse is still hovering
        {
            ToolTipSystem.Show(content, header);
        }
    }

    public void OnMouseDown()
    {
        ToolTipSystem.Hide();
    }
}