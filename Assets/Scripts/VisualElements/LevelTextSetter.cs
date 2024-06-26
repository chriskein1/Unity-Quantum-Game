using UnityEngine;
using TMPro; // Make sure to import TextMeshPro namespace
using UnityEngine.SceneManagement; // Import SceneManager for getting the build index

public class LevelTextSetter : MonoBehaviour
{
    private TextMeshProUGUI levelText; // Reference to the TextMeshPro component

    // Start is called before the first frame update
    void Start()
    {
        levelText = GetComponent<TextMeshProUGUI>(); // Get the TextMeshPro component attached to the GameObject

        if (levelText != null)
        {
            int buildIndex = SceneManager.GetActiveScene().buildIndex;
            levelText.text = "Level " + (buildIndex - 1);
        }
        else
        {
            Debug.LogError("TextMeshProUGUI component not found on this GameObject.");
        }
    }
}
