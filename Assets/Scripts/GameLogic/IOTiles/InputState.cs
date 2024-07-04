using QubitType;
using TMPro;
using UnityEngine;
using System;
public class InputState : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;
    private QubitWireController controller;
    private StartingStateOptions currentState;

    // Start is called before the first frame update
    void Start()
    {
        controller = FindObjectOfType<QubitWireController>();
        if (controller != null)
        {
            // Update the text with the initial input state
            currentState = controller.GetInputState();
            if (currentState == 0)
            {
                text.text = "|0>";

            }
            else
            {
                text.text = "|1>";
            }
        }
    }

}
