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
    [SerializeField] private bool isOpen;
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

    // for test
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
            // Ön yüz
            cardImage.sprite = cardData.cardSprite;
            cardImage.color = cardColors[(int)cardData.cardType[0]];

            nameText.gameObject.SetActive(true);
            healthText.gameObject.SetActive(true);
            damageText.gameObject.SetActive(true);

            nameText.text = cardData.cardName;
            healthText.text = cardData.health.ToString();
            damageText.text = $"{cardData.damageMin} - {cardData.damageMax}";

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
        else if (!isOpen && cardData.cardBackSprite != null)
        {
            // Arka yüz
            cardImage.sprite = cardData.cardBackSprite;

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
