// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using TMPro;

// using QubitType;
// public class Goal2Bit : MonoBehaviour
// {
//     [SerializeField] TwoQubitController gamePuzzleController;
//     private TMP_Text text;

//     string qubitStr = "";

//     // Start is called before the first frame update
//     void Start()
//     {
//         // Get the 2 qubit win state
//         Qubit[] winState = gamePuzzleController.GetWinState();

//         text = GetComponentInChildren<TextMeshProUGUI>();
//         UpdateText(winState);
//     }
//     private void UpdateText(Qubit[] winState)
//     {
//         if (winState[0].state == 2 || winState[1].state == 2)
//         {
//             text.text = "Goal: NaN";
//             return;
//         }

//         bool negativeSign = false;
//         if (!winState[0].PositiveState && winState[1].PositiveState
//             || winState[0].PositiveState && !winState[1].PositiveState)
//         {
//             negativeSign = true;
//         }
//         // i * i = -1
//         if (winState[0].ImaginaryState && winState[1].ImaginaryState)
//         {
//             negativeSign = !negativeSign;
//         }

//         string sign = negativeSign ? "-" : "";
//         string qubitStr = "";

//         if (winState[0].HApplied || winState[1].HApplied)
//         {
//             // filler
//         }
//         // Both qubits have an imaginary part
//         else if (winState[0].ImaginaryState && winState[1].ImaginaryState)
//         {
//             qubitStr = $"{sign}|{winState[0].state}{winState[1].state}>";
//         }
//         // Only one qubit has an imaginary part
//         else if (winState[0].ImaginaryState || winState[1].ImaginaryState)
//         {
//             qubitStr = $"{sign}i|{winState[0].state}{winState[1].state}>";
//         }
//         else
//         {
//             qubitStr = $"{sign}|{winState[0].state}{winState[1].state}>";
//         }

//         text.text = $"Goal: {qubitStr}";
//     }

//     public string GetStr()
//     {
//         return qubitStr;
//     }
// }