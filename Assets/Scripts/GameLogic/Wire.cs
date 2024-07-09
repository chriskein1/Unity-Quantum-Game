using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QubitType;

public class DrawWire : MonoBehaviour
{
    [SerializeField] public LineRenderer lineRendererPrefab;
    [SerializeField] public GameObject inputTile;
    [SerializeField] private GameObject outputTile;
    [SerializeField] private GameObject snapLocation;

    List<LineRenderer> lineRenderers = new List<LineRenderer>();
    private QubitWireController wireController;
    private Qubit qubit = new Qubit();

    // Start is called before the first frame update
    void Start()
    {
        // Find the specific wire controller in the parent
        wireController = transform.parent.GetComponentInChildren<QubitWireController>();

        // Line renderer for each snap point
        for (int i = 0; i < snapLocation.transform.childCount + 1; i++)
        {
            LineRenderer line = Instantiate(lineRendererPrefab, transform);
            line.positionCount = 2;
            lineRenderers.Add(line);
        }
    }
    public void SetInput(Qubit q)
    {
        qubit = q;
    }
    // Update is called once per frame
    void Update()
    {

        //// Draw wire in order
        //// First from input to first snap point
        DrawLine(inputTile.transform.position, snapLocation.transform.GetChild(0).position, qubit, 0);

        //// Then from snap point to snap point
        for (int i = 0; i < snapLocation.transform.childCount - 1; i++)
        {
            qubit = snapLocation.transform.GetChild(i).GetComponent<Snap>().GetState();
            DrawLine(snapLocation.transform.GetChild(i).position, snapLocation.transform.GetChild(i + 1).position, qubit, i + 1);
        }

        //// Finally from last snap point to output
        qubit = snapLocation.transform.GetChild(snapLocation.transform.childCount - 1).GetComponent<Snap>().GetState();
        DrawLine(snapLocation.transform.GetChild(snapLocation.transform.childCount - 1).position, outputTile.transform.position, qubit, snapLocation.transform.childCount);
    }

    void DrawLine(Vector3 start, Vector3 end, Qubit qubit, int index)
    {
        LineRenderer line = lineRenderers[index];
        // Uncomment and adjust the following code to set line colors based on qubit state
        // if (qubit.HApplied)
        // {
        //     line.startColor = Color.green;
        //     line.endColor = Color.green;
        // }
        // else if (qubit.ImaginaryState)
        // {
        //    line.startColor = Color.yellow;
        //    line.endColor = Color.yellow;
        // }
        // else if (!qubit.PositiveState)
        // {
        //     line.startColor = Color.red;
        //     line.endColor = Color.red;
        // }
        // else if (qubit.state == 1)
        // {
        //     line.startColor = Color.blue;
        //     line.endColor = Color.blue;
        // }
        // else
        // {
        //     line.startColor = Color.black;
        //     line.endColor = Color.black;
        // }

        line.SetPosition(0, start);
        line.SetPosition(1, end);
    }
}
