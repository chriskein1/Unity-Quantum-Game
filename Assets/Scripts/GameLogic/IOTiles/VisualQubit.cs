using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using QubitType;
public class VisualQubit : MonoBehaviour
{
    public Qubit qubit;
    public GameObject q0Circle;
    public GameObject q1Square;
    public GameObject negativeBar;
    public GameObject q0Super;
    public GameObject q0SuperNegative;
    public GameObject q1Super;
    public GameObject q1SuperNegative;
    private Dictionary<string, Sprite> qubitSprites;

    // Start is called before the first frame update
    void Start()
    {
        // Load sprites from Assets/Art/QubitSprites into list
        qubitSprites = new Dictionary<string, Sprite>();
        qubitSprites.Add("0Circle", Resources.Load<Sprite>("QubitSprites/0Circle"));
        qubitSprites.Add("1Circle", Resources.Load<Sprite>("QubitSprites/1Circle"));
        qubitSprites.Add("SuperCircle", Resources.Load<Sprite>("QubitSprites/SuperCircle"));
        qubitSprites.Add("0Square", Resources.Load<Sprite>("QubitSprites/0Square"));
        qubitSprites.Add("1Square", Resources.Load<Sprite>("QubitSprites/1Square"));
        qubitSprites.Add("SuperSquare", Resources.Load<Sprite>("QubitSprites/SuperSquare"));
    }

    public void SetQubit(Qubit q, int n)
    {
        qubit = q;
        if (n == 0)
        {
            q0Circle.SetActive(true);
            q1Square.SetActive(false);
            if (!qubit.IsInSuperposition())
            {
                q0Super.SetActive(false);
            }
           // If Alpha is 1, set the sprite to 0Circle
            if (Math.Abs(qubit.Alpha.Real) == 1 || Math.Abs(qubit.Alpha.Imaginary) == 1)
            {
                q0Circle.GetComponentInChildren<SpriteRenderer>().sprite = qubitSprites["0Circle"];
            }
            // If Beta is 1, set the sprite to 1Circle
            else if (Math.Abs(qubit.Beta.Real) == 1 || Math.Abs(qubit.Beta.Imaginary) == 1)
            {
                q0Circle.GetComponentInChildren<SpriteRenderer>().sprite = qubitSprites["1Circle"];
            }
            // If in superposition, set the sprite to SuperCircle
            else if (qubit.IsInSuperposition())
            {
                q0Circle.GetComponentInChildren<SpriteRenderer>().sprite = qubitSprites["0Circle"];
                q0Circle.SetActive(false);
                q0Super.SetActive(true);
                if (qubit.Beta.Real < 0 || qubit.Beta.Imaginary < 0)
                {
                    q0SuperNegative.SetActive(true);
                }
                else
                {
                    q0SuperNegative.SetActive(false);
                }
            }

        }
        else
        {
            q0Circle.SetActive(false);
            q1Square.SetActive(true);
            if (!qubit.IsInSuperposition())
            {
                q1Super.SetActive(false);
            }

            // If Alpha is 1, set the sprite to 0Square
            if (Math.Abs(qubit.Alpha.Real) == 1 || Math.Abs(qubit.Alpha.Imaginary) == 1)
            {
                q1Square.GetComponentInChildren<SpriteRenderer>().sprite = qubitSprites["0Square"];
            }
            // If Beta is 1, set the sprite to 1Square
            else if (Math.Abs(qubit.Beta.Real) == 1 || Math.Abs(qubit.Beta.Imaginary) == 1)
            {
                q1Square.GetComponentInChildren<SpriteRenderer>().sprite = qubitSprites["1Square"];
            }
            // If in superposition, set the sprite to SuperSquare
            else if (qubit.IsInSuperposition())
            {
                q1Square.GetComponentInChildren<SpriteRenderer>().sprite = qubitSprites["0Square"];
                q1Square.SetActive(false);
                q1Super.SetActive(true);
                if (qubit.Beta.Real < 0 || qubit.Beta.Imaginary < 0)
                {
                    q1SuperNegative.SetActive(true);
                }
                else
                {
                    q1SuperNegative.SetActive(false);
                }
            }
        }

        // Check if the qubit is < 0 for either alpha or beta
        if (!qubit.IsInSuperposition() && (qubit.Alpha.Real < 0 || qubit.Beta.Real < 0
            || qubit.Alpha.Imaginary < 0 || qubit.Beta.Imaginary < 0))
        {
            negativeBar.SetActive(true);
        }
        else
        {
            negativeBar.SetActive(false);
        }
    }
}
