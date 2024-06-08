using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ToolTipSystem : MonoBehaviour
{
    private static ToolTipSystem current;
    public ToolTip tooltip;

    public void Awake()
    {
        current = this;
        current.tooltip.gameObject.SetActive(false);
    }

    public static void Show(string content, string header, Color headerColor, Color contentColor)
    {
        current.tooltip.SetText(content, header, headerColor, contentColor);
        current.tooltip.gameObject.SetActive(true);
    }
    public static void Hide()
    {
        current.tooltip.gameObject.SetActive(false);
    }
}
