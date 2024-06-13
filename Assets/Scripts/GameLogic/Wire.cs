using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawWire : MonoBehaviour
{

    [SerializeField] public LineRenderer lineRenderer;
    [SerializeField] public GameObject inputTile;
    [SerializeField] private GameObject outputTile;
    [SerializeField] private GameObject snapLocation;

    // Start is called before the first frame update
    void Start()
    {
        // All snap points + IO
        lineRenderer.positionCount = 2 + snapLocation.transform.childCount;
    }

    // Update is called once per frame
    void Update()
    {
        // Draw wire in order
        // First from input to first snap point
        lineRenderer.SetPosition(0, inputTile.transform.position);
        lineRenderer.SetPosition(1, snapLocation.transform.GetChild(0).position);

        // Then from snap point to snap point
        for (int i = 1; i < snapLocation.transform.childCount; i++)
        {
            Snap snap = snapLocation.transform.GetChild(i).GetComponent<Snap>();
            if (snap != null)
            {
                if (snap.GetState().state == 1)
                {
                    lineRenderer.startColor = Color.blue;
                    lineRenderer.endColor = Color.blue;
                }
                else
                {
                    lineRenderer.startColor = Color.black;
                    lineRenderer.endColor = Color.black;
                }
                lineRenderer.SetPosition(i + 1, snapLocation.transform.GetChild(i).position);
            }
        }

        // Finally from last snap point to output
        lineRenderer.SetPosition(snapLocation.transform.childCount + 1, outputTile.transform.position);
    }
}
