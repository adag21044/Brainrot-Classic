using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    public Card cardData;
    public Image cardImage;
    public TMP_Text nameText;
    public TMP_Text healthText;
    public TMP_Text damageText;
    public Image[] typeImages;
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
        new Color(0.8f, 0.52f, 0.24f), // Earth
        Color.blue,   // Water
        new Color(0.47f, 0f, 0.4f),  // Dark
        Color.yellow, // Light
        Color.cyan    // Air
    };

    private void Start()
    {
        UpdateCardDisplay();
    }

    public void UpdateCardDisplay()
    {
        //Update the main card image color based on the first card type
        cardImage.color = cardColors[(int)cardData.cardType[0]];


        nameText.text = cardData.cardName;
        healthText.text = cardData.health.ToString();
        damageText.text = $"{cardData.damageMin} - {cardData.damageMax}";

        // Update type images
        for (int i = 0; i < typeImages.Length; i++)
        {
            if (i < cardData.cardType.Count)
            {
                typeImages[i].gameObject.SetActive(true);
                typeImages[i].color = typeColors[(int)cardData.cardType[i]];
            }
            else
            {
                typeImages[i].gameObject.SetActive(false);
            }
        }
    }
}
