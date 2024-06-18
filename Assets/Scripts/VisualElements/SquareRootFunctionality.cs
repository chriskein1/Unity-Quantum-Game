using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SquareRootFunctionality : MonoBehaviour
{
    [SerializeField] private TextMeshPro numeratorText;
    public void SetState(bool imaginary)
    {
        if (imaginary)
            numeratorText.text = "i";
        
        else
            numeratorText.text = "1";
    }
}
