using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawWire : MonoBehaviour
{

    [SerializeField] public LineRenderer lineRendererPrefab;
    [SerializeField] public GameObject inputTile;
    [SerializeField] public GameObject outputTile;

    // Start is called before the first frame update
    void Start()
    {
        // Draw Line from input to output
        DrawLine(inputTile.transform.position, outputTile.transform.position);
    }

    void DrawLine(Vector3 start, Vector3 end)
    {
        LineRenderer line = Instantiate(lineRendererPrefab);
        line.startColor = Color.black;
        line.endColor = Color.black;
        line.SetPosition(0, start);
        line.SetPosition(1, end);
    }
}
