using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeState : MonoBehaviour
{
    public void SetZeroState()
    {
        transform.eulerAngles = new Vector3(0f, 0f, 0f);
    }

    public void SetOneState()
    {
        transform.eulerAngles = new Vector3(180f, 0f, 0f);
    }

    public void SetPositiveIState()
    {
        transform.eulerAngles = new Vector3(0f, 0f, -90f);
    }
    public void SetNegativeIState()
    {
        transform.eulerAngles = new Vector3(0f, 0f, 90f);
    }
    public void SetPositiveState()
    {
        transform.eulerAngles = new Vector3(90f, 0f, 0f);
    }
    public void SetNegativeState()
    {
        transform.eulerAngles = new Vector3(-90f, 0f, 0f);
    }
}
