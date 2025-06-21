using UnityEngine;
using TMPro;


public class DeckCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI remainingCardCountText;

    public void UpdateCardCountUI(int remainingCards)
    {
        remainingCardCountText.text = remainingCards.ToString();
    }
}