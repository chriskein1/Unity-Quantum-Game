using UnityEngine;
using UnityEngine.EventSystems;

public class HolderButtonLogic : MonoBehaviour, IPointerDownHandler
{
    public GateHolderScript gateHolderScript; // Reference to the GateHolderScript
    public string gateType; // Type of the gate this button is responsible for
    private TutorialOverlay tutorialOverlay;

    void Awake()
    {
        tutorialOverlay = FindAnyObjectByType<TutorialOverlay>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if(Time.timeScale > 0 && !tutorialOverlay.getStatus()) 
        gateHolderScript.SpawnAndDragGate(gateType);
    }
}
