using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public List<Card> allCards = new List<Card>();

    private int currentCardIndex = 0;

    private void Start()
    {
        //Load all cards from the Resources folder
        Card[] loadedCards = Resources.LoadAll<Card>("Cards");

        //Add the loaded cards to the allCards list
        allCards.AddRange(loadedCards);

        HandManager hand = FindObjectOfType<HandManager>();

        for (int i = 0; i < 6; i++)
        {
            DrawCard(hand);
        }
    }

    public void DrawCard(HandManager handManager)
    {
        if (allCards.Count == 0) return;

        Card nextCard = allCards[currentCardIndex];
        handManager.AddCardToHand(nextCard);
        currentCardIndex = (currentCardIndex + 1) % allCards.Count; // Loop back to the start of the deck
    } 
}
