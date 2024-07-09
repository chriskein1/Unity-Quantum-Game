using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class HolderButtonLogic : MonoBehaviour, IPointerDownHandler
{
    public GateHolderScript gateHolderScript; // Reference to the GateHolderScript
    public string gateType; // Type of the gate this button is responsible for

    private TutorialOverlay tutorialOverlay;
    private bool hasTutorialOverlay;
    private TextMeshProUGUI gateCountText;
    private Image buttonImage;

    void Awake()
    {
        tutorialOverlay = FindAnyObjectByType<TutorialOverlay>();
        hasTutorialOverlay = tutorialOverlay != null;

        gateCountText = GetComponentInChildren<TextMeshProUGUI>();
        buttonImage = GetComponent<Image>();
        UpdateButtonAppearance();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Time.timeScale > 0)
        {
            if (hasTutorialOverlay)
            {
                if (!tutorialOverlay.GetStatus())
                {
                    gateHolderScript.SpawnAndDragGate(gateType);
                }
            }
            else
            {
                gateHolderScript.SpawnAndDragGate(gateType);
            }
            UpdateButtonAppearance();
        }
    }

    public void UpdateButtonAppearance()
    {
        if (gateCountText == null || buttonImage == null)
        {
            return;
        }

        int gateCount;
        if (int.TryParse(gateCountText.text, out gateCount) && gateCount == 0)
        {
            buttonImage.color = Color.gray;
        }
        else
        {
            buttonImage.color = Color.white;
        }
    }
}
