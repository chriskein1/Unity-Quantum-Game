using UnityEngine;
using UnityEngine.EventSystems;

public class HolderButtonLogic : MonoBehaviour, IPointerDownHandler
{
    public GateHolderScript gateHolderScript; // Reference to the GateHolderScript
    public string gateType; // Type of the gate this button is responsible for

    public void OnPointerDown(PointerEventData eventData)
    {
        gateHolderScript.SpawnAndDragGate(gateType);
    }
}
