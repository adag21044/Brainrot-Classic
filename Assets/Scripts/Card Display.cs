using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    public Card cardData; // ScriptableObject that stores all static data about the card
    public Image cardImage; // The main sprite shown on the card (front or back)
    public TMP_Text nameText;   // Text component displaying the card name
    public TMP_Text healthText; // Text component displaying the health value
    public TMP_Text damageText; // Text component displaying the damage range
    public Image[] typeImages; // One icon for each element/type the card has (max length set in Inspector)
    [SerializeField] private bool isOpen; // Determines whether the card is currently face‑up
    [SerializeField] private Sprite[] typeIcons; // Type Icon
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

    // Temporary debug shortcut: press SPACE to flip the card in play mode
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isOpen = !isOpen;
            UpdateCardDisplay();
        }
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
