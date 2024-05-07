using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePuzzleController : MonoBehaviour
{
    [SerializeField] private List<GameObject> SnapPoints = new List<GameObject>();
    private bool complete=false;
    private void Update()
    {
        foreach( GameObject p in SnapPoints)
        {
            complete = true;
            Snap snapComp = p.GetComponent<Snap>();
            if (snapComp.GetGateStatus() == false)
            {
                complete=false;
                break;
            }
        }
        if (complete == true)
        {
            print("Puzzle Complete!!!!");
        }
  
    }

}

