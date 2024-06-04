using TMPro;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// This class handles snapping objects to a specified location
/// </summary>
public class Snap : MonoBehaviour
{

    // Events for a gate being added or removed
    public UnityEvent GateChanged;
    private bool occupied=false;
    private bool correctGate=false;
    private GameObject gameObj;
    
    


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(occupied==false)
        {
            
            Drag dragComponent = other.GetComponent<Drag>();
            if (dragComponent != null)
            {
                dragComponent.Snapping();
                //Snap the object to the center of the trigger plus any adjustment
                other.transform.position = transform.position;
                gameObj = other.gameObject;
                occupied = true;
                GateChanged.Invoke();
                
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if(gameObj==collision.gameObject) 
        {
            occupied = false;
            gameObj = null;
            correctGate = false;
            GateChanged.Invoke();
        }
        
    }
  
    public bool GetGateStatus()
    {
        return correctGate;

    }

    public GameObject GetGateObject()
    {
        return gameObj;
    }
}
