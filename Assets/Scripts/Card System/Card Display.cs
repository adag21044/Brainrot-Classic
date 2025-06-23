using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum CardLocation
{
    Deck,
    PlayerHand,
    AIHand,
    Table
}
  

public class CardDisplay : MonoBehaviour
{
    public Card cardData; // ScriptableObject that stores all static data about the card
    public Image cardImage; // The main sprite shown on the card (front or back)
    public TMP_Text nameText;   // Text component displaying the card name
    public TMP_Text healthText; // Text component displaying the health value
    public TMP_Text damageText; // Text component displaying the damage range
    public Image[] typeImages; // One icon for each element/type the card has (max length set in Inspector)
    [SerializeField] public bool isOpen; // Determines whether the card is currently face‑up
    [SerializeField] public CardLocation cardLocation; // The location of the card in the game (e.g., PlayerHand, AIHand, Table, Deck)
    [SerializeField] private Sprite[] typeIcons; // Type Icon
    public CardLocation currentLocation { get; private set; } // Current location of the card (e.g., PlayerHand, Table, etc.)

    

    public void SetLocation(CardLocation newLocation)
    {
        // Store location
        currentLocation = newLocation;
        cardLocation    = newLocation;

        // Face-up only if the card is in the player's hand or on the table
        isOpen = newLocation == CardLocation.PlayerHand ||
                newLocation == CardLocation.Table;

        UpdateCardDisplay();   // Refresh visuals
    }



    private Color[] cardColors =
    {
        Color.grey,    // Fire
        new Color(0.5f, 0.25f, 0.24f), // Earth
        Color.blue,   // Water
        Color.black,  // Dark
        Color.yellow, // Light
        Color.cyan    // Air
    };

    private Color[] typeColors =
    {
        Color.white,    // Fire
        Color.white, // Earth
        Color.blue,   // Water
        new Color(0.47f, 0f, 0.4f),  // Dark
        Color.yellow, // Light
        Color.cyan    // Air
    };

    private void Start()
    {
        UpdateCardDisplay(); // Initialise visuals once the object awakens
    }



    public void SetCard(Card card, bool open, CardLocation location)
    {
        cardData = card;
        isOpen = open;
        currentLocation = location;
        cardLocation    = location; 
        UpdateCardDisplay();
    }


    public bool IsInPlayerHand()
    {
        return currentLocation == CardLocation.PlayerHand;
    }

    public bool IsOnTable()
    {
        return currentLocation == CardLocation.Table;
    }

    public bool IsInAIHand()
    {
        return currentLocation == CardLocation.AIHand;
    }

    public bool IsInDeck()
    {
        return currentLocation == CardLocation.Deck;
    }



    public void UpdateCardDisplay()
    {
        if (isOpen && cardData.cardSprite != null)
        {
            // -------------------------- Front side -----------------------
            cardImage.sprite = cardData.cardSprite;
            //cardImage.color = cardColors[(int)cardData.cardType[0]];

            // Ensure all textual elements are visible
            nameText.gameObject.SetActive(true);
            healthText.gameObject.SetActive(true);
            damageText.gameObject.SetActive(true);

            // Populate text values
            nameText.text = cardData.cardName;
            healthText.text = cardData.health.ToString();
            damageText.text = $"{cardData.damageMin} - {cardData.damageMax}";

            // Display element/type icons
            for (int i = 0; i < typeImages.Length; i++)
            {
                if (i < cardData.cardType.Count)
                {
                    // Set correct sprite and enable image
                    typeImages[i].sprite = typeIcons[(int)cardData.cardType[i]];
                    typeImages[i].gameObject.SetActive(true);
                }
                else
                {
                    typeImages[i].gameObject.SetActive(false);
                }
            }
        }
        else if (!isOpen && cardData.cardBackSprite != null)
        {
            // Back side of the card
            cardImage.sprite = cardData.cardBackSprite;

            // Hide front‑only UI elements when the card is face‑down
            nameText.gameObject.SetActive(false);
            healthText.gameObject.SetActive(false);
            damageText.gameObject.SetActive(false);

            foreach (var img in typeImages)
            {
                img.gameObject.SetActive(false);
            }
        }
    }

}
