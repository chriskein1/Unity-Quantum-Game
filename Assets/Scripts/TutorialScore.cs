using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TutorialScore : MonoBehaviour
{
    public TextMeshProUGUI scoreText;  // Reference to the score text
    public Button nextLevelButton;  // Reference to the button that will change color
    public int maxScore = 4;  // Set the maximum score
    private int currentScore = 0;  // Track the current score
    private TextMeshProUGUI buttonText;
    private Image buttonImage;
    public Button ReadyToPlay;

    // Start is called before the first frame update
    void Start()
    {
        UpdateScoreText();
        buttonText = nextLevelButton.GetComponentInChildren<TextMeshProUGUI>();
        ReadyToPlay.gameObject.SetActive(false);
    }

    // Update the score text
    void UpdateScoreText()
    {
        scoreText.text = "Score: " + currentScore.ToString() + " out of " + maxScore;
    }

    // Method to increment score, which can be called by a separate button
    public void IncrementScore()
    {
        currentScore++;
        UpdateScoreText();

        if (currentScore >= maxScore)
        {
            // Change the button text color to black
            if (buttonText != null)
            {
                buttonText.color = Color.black;
            }

            // Change button background color to yellow
            ColorBlock cb = nextLevelButton.colors;
            cb.normalColor = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, cb.normalColor.a);
            cb.highlightedColor = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, cb.highlightedColor.a);
            cb.pressedColor = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, cb.pressedColor.a);
            cb.selectedColor = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, cb.selectedColor.a);
            nextLevelButton.colors = cb;

            // Change button image color to white
            buttonImage = nextLevelButton.GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.color = Color.white;
            }
            ReadyToPlay.gameObject.SetActive(true);

        }
    }
}
