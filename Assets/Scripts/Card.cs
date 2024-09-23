using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int cardId;           // Unique ID for the card (set this in the inspector)
    public Image frontImage;     // Reference to the front image of the card
    public Image backImage;      // Reference to the back image of the card
    public bool isMatched = false; // Whether the card has been matched

    private bool isFlipped = false; // Track if the card is currently flipped

    private void Start()
    {
       
    }

    // Method to flip the card
    public void Flip(bool showFront)
    {
        isFlipped = showFront;
       // backImage.gameObject.SetActive(showFront);
        frontImage.enabled=!showFront;
    }

    // Method to set the card as matched
    public void SetMatched(bool matched)
    {
        isMatched = matched;
        if (matched)
        {
            // Optional: Add visual feedback for matched cards
            frontImage.color = Color.green; // Example feedback
        }
    }
}
