using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public List<Card> allCards = new List<Card>();

    private int currentCardIndex = 0;

    [Header("Players")]
    public HandManager playerHand;
    public HandManager aiHand;

    private void Start()
    {
        //Load all cards from the Resources folder
        Card[] loadedCards = Resources.LoadAll<Card>("Cards");

        //Add the loaded cards to the allCards list
        allCards.AddRange(loadedCards);

        //Shuffle the deck
        ShuffleCards(allCards);

        for (int i = 0; i < 2; i++)
        {
            DrawCard(playerHand);
            DrawCard(aiHand);
        }
    }

    private void ShuffleCards(List<Card> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            int randomIndex = Random.Range(i, cards.Count);
            Card temp = cards[i];
            cards[i] = cards[randomIndex];
            cards[randomIndex] = temp;
        }
    }

    public void DrawCard(HandManager handManager)
    {
        if (allCards.Count == 0) return;

        Card nextCard = allCards[currentCardIndex];
        handManager.AddCardToHand(nextCard);
        currentCardIndex = (currentCardIndex + 1) % allCards.Count; // Loop back to the start of the deck
    }

    public Card GetNextCard()
    {
        if (allCards.Count == 0) return null; // Check if there are any cards left

        var next = allCards[currentCardIndex]; // Get the next card
        currentCardIndex = (currentCardIndex + 1) % allCards.Count; // Move to the next card, looping back to the start
        return next;
    }
}
