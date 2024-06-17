using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float rotationSpeed = 10f;

    void Update()
    {
        // Rotate the object around its z-axis
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
