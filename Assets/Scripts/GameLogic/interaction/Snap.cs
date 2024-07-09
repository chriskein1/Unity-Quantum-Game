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
    private GameObject gameObj;

    private Qubit qubit;


 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!disableSnapping && occupied == false)
        {
            Drag dragComponent = other.GetComponent<Drag>();
            if (dragComponent != null)
            {
                ClickOnSound();
                dragComponent.Snapping();
                // Snap the object to the center of the trigger plus any adjustment
                other.transform.position = transform.position;
                gameObj = other.gameObject;
                occupied = true;
                GateChanged.Invoke();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (gameObj == collision.gameObject)
        {
                ClickOffSound();
            
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

    public void SetGateObject(GameObject gate)
    {
        gameObj = gate;
    }

    public void SetState(Qubit qubit)
    {
        this.qubit = qubit;
    }

    public Qubit GetState()
    {
        return qubit;
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

   
    //public void ActivateGate()
    //{
    //    active = true;
    //}

    //public void DeactivateGate()
    //{
    //    active = false;
    //}

    //public bool IsActive()
    //{
    //    return active;
    //}
}
