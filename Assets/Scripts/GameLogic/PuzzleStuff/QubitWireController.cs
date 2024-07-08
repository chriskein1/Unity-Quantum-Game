using QubitType;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QubitWireController : MonoBehaviour
{
    private List<GameObject> SnapPoints = new List<GameObject>();
    [SerializeField] private InputState inputTile;
    [SerializeField] private OutputState outputTile;

    private void Awake()
    {
        PopulateSnapPoints();
    }


    private void PopulateSnapPoints()
    {
        SnapPoints.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            SnapPoints.Add(transform.GetChild(i).gameObject);

        }
    }
    public List<GameObject> GetSnapPoints()
    {
        return SnapPoints;
    }

    public void SetInput(SingleQubitStateOptions q)
    {
        inputTile.UpdateText(q);
    }


    public void SetOutput(Qubit q)
    {
        outputTile.SetState(q);
    }


}
