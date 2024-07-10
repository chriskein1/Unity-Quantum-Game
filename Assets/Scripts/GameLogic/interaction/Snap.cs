using TMPro;
using UnityEngine;
using UnityEngine.Events;
using QubitType;

public class Snap : MonoBehaviour
{
    // Events for a gate being added or removed
    public UnityEvent GateChanged;
    [SerializeField] private bool disableSnapping = false;
    private bool occupied = false;
    private bool correctGate = false;
   // private bool active = true;
    private GameObject gate;
    private Drag dragComponent;
    private Qubit qubit;
    private CircuitManager circuitManager;

    private void Start()
    {
        // Find the CircuitManager in the scene
        circuitManager = FindObjectOfType<CircuitManager>();
        if (circuitManager == null)
        {
            Debug.LogError("CircuitManager not found in the scene!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!disableSnapping && occupied == false)
        {
            dragComponent = other.GetComponent<Drag>();

            if (dragComponent != null && !dragComponent.IsDragging() && dragComponent.CanSnap() && !dragComponent.IsSnapped())
            {
                SnapObject(other.gameObject);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!disableSnapping && occupied == false && dragComponent != null)
        {
            if (!dragComponent.IsDragging() && dragComponent.CanSnap() && !dragComponent.IsSnapped())
            {
                SnapObject(collision.gameObject);
            }
        }
    }

    //    public void SnapToPosition(GameObject obj)
    //{
    //    if (!disableSnapping && !occupied)
    //    {
    //        ClickOnSound();
    //        obj.transform.position = transform.position;
    //        gameObj = obj;
    //        occupied = true;
    //        GateChanged.Invoke();
    //    }
    //}
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (gate == collision.gameObject)
        {
                ClickOffSound();
            
            occupied = false;
            gate = null;
            correctGate = false;
            GateChanged.Invoke();
            dragComponent = null;
        }
    }

    public bool GetGateStatus()
    {
        return correctGate;
    }

    public GameObject GetGateObject()
    {
        return gate;
    }

    public void SetGateObject(GameObject Gate)
    {
        gate = Gate;
    }

    public void SetState(Qubit qubit)
    {
        this.qubit = qubit;
    }

    public Qubit GetState()
    {
        return qubit;
    }
    private void SnapObject(GameObject obj)
    {
        ClickOnSound();
        obj.transform.position = gameObject.transform.position;

        dragComponent.Snapping();
        gate = obj;
        occupied = true;
        if (gate.CompareTag("ctrl") || gate.CompareTag("swap"))
            Snap2BitGate(gate);

        GateChanged.Invoke();
    }


    public void Snap2BitGate(GameObject obj)
    {
        var (row, yDistance) = circuitManager.FindSnapPointRow(gameObject);
        Vector3 newPosition = obj.transform.position;


        if (row == 0)
        {
            newPosition.y -= yDistance;  // Add 1 to the y coordinate

        }
        else if (row == 1)
        {
            newPosition.y += yDistance;  // Add 1 to the y coordinate
            obj.transform.Rotate(0, 0, 180);
        }
        obj.transform.position = newPosition;
    }
    private void ClickOnSound()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.Play("SnapOn");
        }
        else
        {
            Debug.LogError("AudioManager instance not found!");
        }
    }

    private void ClickOffSound()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.Play("SnapOff");
        }
        else
        {
            Debug.LogError("AudioManager instance not found!");
        }
    }
    
}
