using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public List<Card> allCards = new List<Card>();

    private int currentCardIndex = 0;

    [Header("Players")]
    public HandManager playerHand;
    public HandManager aiHand;

    [Header("Cards in Table")]
    public Transform tableCardParent; // Yerdeki kartların konacağı GameObject
    public GameObject cardPrefab;     // Instantiate edilecek kart prefabı
    public List<GameObject> tableCards = new List<GameObject>();
    [SerializeField] private GameObject deckObject;

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

        DealTableCards(); // Open the card on the table
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

        // Remove the card from the deck
        allCards.RemoveAt(currentCardIndex);

        // Increment the index to point to the next card
        if (currentCardIndex >= allCards.Count)
            currentCardIndex = 0;

        
        if (allCards.Count == 0 && deckObject != null)
            SetDeactiveDeckObject(deckObject);

    }

    public Card GetNextCard()
    {
        if (allCards.Count == 0) return null;

        Card next = allCards[currentCardIndex];
        allCards.RemoveAt(currentCardIndex);

        if (currentCardIndex >= allCards.Count)
            currentCardIndex = 0;

        if (allCards.Count == 0 && deckObject != null)
            SetDeactiveDeckObject(deckObject);

        return next;
    }


    public void SetDeactiveDeckObject(GameObject deckObj)
    {
        if (deckObj != null)
        {
            deckObject = deckObj;
            deckObject.SetActive(false); 
        }
    }

    public void DealTableCards()
    {
        for (int i = 0; i < 3; i++)
        {
            Card card = GetNextCard();
            if (card == null) continue;

            GameObject newCard = Instantiate(cardPrefab, tableCardParent);
            var disp = newCard.GetComponent<CardDisplay>();

            disp.cardData = card;
            disp.SetLocation(CardLocation.Table);   // <-- tek satır eklendi

            tableCards.Add(newCard);
            newCard.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(i * 200, 0);
        }
    }
}
