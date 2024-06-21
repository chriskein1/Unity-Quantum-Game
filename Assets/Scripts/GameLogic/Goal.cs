using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using QubitType;
public class Goal : MonoBehaviour
{
    [SerializeField] GamePuzzleController gamePuzzleController;
    private TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        // Get the win state
        Qubit winState = gamePuzzleController.GetWinState();

        text = GetComponentInChildren<TextMeshProUGUI>();
        UpdateText(winState);
    }
    private void UpdateText(Qubit winState)
    {
        string sign = winState.PositiveState ? "" : "-";
        string qubitStr = "";


        if (winState.state==2)
        {
            text.text = "Goal: NaN";
            return;
        }
        if (winState.HApplied)
        {
            if (winState.PositiveState)
            {
                qubitStr = winState.ImaginaryState ? "i/√2(|0> + |1>)" : "1/√2(|0> + |1>)";
            }
            else
            {
                qubitStr = winState.ImaginaryState ? "i/√2(|0> - |1>)" : "1/√2(|0> - |1>)";
            }
        }
        else if (winState.ImaginaryState)
        {
            qubitStr = $"{sign}i|{winState.state}>";
        }
        else
        {
            qubitStr = $"{sign}|{winState.state}>";
        }

        text.text += qubitStr;
    }
}