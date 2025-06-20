using UnityEngine;
using TMPro;

public class AICardDebugger : MonoBehaviour
{
    [SerializeField] private HandManager handManager; // Reference to the HandManager to access the AI's hand
    [SerializeField] private TMP_Text debugText; // Optional: Text component to display debug information in the UI

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("AI's Hand Cards:");
            foreach (var card in handManager.cardsInHand)
            {
                var cardDisplay = card.GetComponent<CardDisplay>();
                Debug.Log($"Card: {cardDisplay.cardData.cardName}, Location: {cardDisplay.currentLocation}, Open: {cardDisplay.isOpen}\n");
                debugText.text += $"Card: {cardDisplay.cardData.cardName}, Location: {cardDisplay.currentLocation}, Open: {cardDisplay.isOpen}\n";
            }
        }
    }
}