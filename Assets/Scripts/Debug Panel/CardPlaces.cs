using UnityEngine;

public class CardPlaces : MonoBehaviour
{
    [Header("AI Cards")]
    [SerializeField] private AIManager aiManager;
    private bool isOpen = false;

    public void ToggleAICards()
    {
        if (!isOpen)
        {
            OpenAICards();
        }
        else
        {
            CloseAICards();
        }

    }

    private void OpenAICards()
    {
        foreach (var cardGO in aiManager.aiHand.cardsInHand)
        {
            var display = cardGO.GetComponent<CardDisplay>();
            if (display != null)
            {
                // Zorla kartı açık hale getir
                display.SetCard(display.cardData, true, CardLocation.Table); // sadece test amaçlı
            }
        }

        isOpen = true; // Kartlar açıldı
        Debug.Log("AI Cards Opened");
    }

    private void CloseAICards()
    {
        foreach (var cardGO in aiManager.aiHand.cardsInHand)
        {
            var display = cardGO.GetComponent<CardDisplay>();
            if (display != null)
            {
                // Zorla kartı kapalı hale getir
                display.SetCard(display.cardData, false, CardLocation.Table); // sadece test amaçlı
            }
        }

        isOpen = false; // Kartlar kapandı
        Debug.Log("AI Cards Closed");
    }
}
