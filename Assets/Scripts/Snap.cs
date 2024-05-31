using TMPro;
using UnityEngine;

public class Snap : MonoBehaviour
{

    private bool occupied=false;
    private bool correctGate=false;
    private GameObject gameObj;
    [Header("Correct Gate Object")]
    [SerializeField] private GameObject correctObj;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(occupied==false)
        {
            
            Drag dragComponent = other.GetComponent<Drag>();
            if (dragComponent != null)
            {
                dragComponent.StopDragging();
                //Snap the object to the center of the trigger plus any adjustment
                other.transform.position = transform.position;
                gameObj = other.gameObject;
                occupied = true;
                if (gameObj.tag == correctObj.tag)
                {
                    print("Correct gate has been placed");
                    correctGate= true;
                }
            }
        }
    }

    // Function to return the gate on the snap point (game object tag)
    public string GetGate()
    {
        if (gameObj != null)
        {
            return gameObj.tag;
        }
        return "";
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(gameObj==collision.gameObject) 
        {
           
            occupied = false;
            gameObj = null;
            correctGate = false;
        }
        
    }
    //public void SnapToCenter(Transform objectTransform)
    //{
    //    if (!occupied){
    //        print("This is inside of the snaptocenter");
    //        objectTransform.position = transform.position;
    //        occupied=true;
    //    }
    //}
    public bool GetGateStatus()
    {
        return correctGate;

    }
}
