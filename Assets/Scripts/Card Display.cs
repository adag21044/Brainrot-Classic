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

    private void Start()
    {
        UpdateCardDisplay();
    }

    public void UpdateCardDisplay()
    {
        nameText.text = cardData.cardName;
        healthText.text = cardData.health.ToString();
    }
}
