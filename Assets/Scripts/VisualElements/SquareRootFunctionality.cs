using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SquareRootFunctionality : MonoBehaviour
{
    [Header("Text Objects")]
    [SerializeField] private TextMeshProUGUI numeratorText;
    [SerializeField] private TextMeshProUGUI denominatorText;
    [SerializeField] private GameObject squareRootSymbol;
    [SerializeField] private int numerator;
    [SerializeField] private int denominator;
    [SerializeField] private bool ShowSquareRoot;


    private void Awake()
    {
        numeratorText.text= numerator.ToString();
        denominatorText.text= denominator.ToString();

        squareRootSymbol.SetActive(ShowSquareRoot);

    }




}
