using QubitType;
using System.Numerics;
using TMPro;
using UnityEngine;

public class InputState : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;
 
    public void UpdateText(SingleQubitStateOptions state)
    {
        switch (state)
        {
            case SingleQubitStateOptions.State0:
                text.text = "|0>";
                break;
            case SingleQubitStateOptions.State1:
                text.text = "|1>";
                break;
            case SingleQubitStateOptions.Imaginary0:
                text.text = "i|0>";
                break;
            case SingleQubitStateOptions.Imaginary1:
                text.text = "i|1>";
                break;
            case SingleQubitStateOptions.NegativeState1:
                text.text = "-|1>";
                break;
            case SingleQubitStateOptions.NegativeImaginary1:
                text.text = "-i|1>";
                break;
            case SingleQubitStateOptions.NegativeImaginary0:
                text.text = ("-i|0>");
                break;
            default:
                text.text = "-|0>";
                break;
        }
    }

}
