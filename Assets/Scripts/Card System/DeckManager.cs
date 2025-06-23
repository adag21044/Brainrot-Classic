using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    [SerializeField] private DeckVisibilityController deckVisibilityController;
    public bool IsEmpty => allCards.Count == 0;

    [Header("Events")]
    public IntEvent onDeckChanged; // Custom UnityEvent
    
    public DeckCounter deckCounter; // Reference to the DeckCounter to update UI


    private void Start()
    {
        onDeckChanged?.Invoke(allCards.Count);

        //Load all cards from the Resources folder
        Card[] loadedCards = Resources.LoadAll<Card>("Cards");

        //Add the loaded cards to the allCards list
        allCards.AddRange(loadedCards);

        //Shuffle the deck
        ShuffleCards(allCards);

        // Start() – initial deal (two cards each)
        for (int i = 0; i < 2; i++)
        {
            DrawCard(playerHand, false); // Player
            DrawCard(aiHand,     true ); // AI
        }

        DealTableCards(); // Open the card on the table

        Debug.Log($"Total cards loaded from Resources: {allCards.Count}");


    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            AddCardToTable(GetNextCard());
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

    // Draw one card into the specified hand
    public void DrawCard(HandManager handManager, bool isAI)
    {
        if (allCards.Count == 0)
        {
            SetDeactiveDeckObject(deckObject);
            return;
        }

        if (currentCardIndex >= allCards.Count)
            currentCardIndex = 0;

        Card nextCard = allCards[currentCardIndex];
        handManager.AddCardToHand(nextCard, isAI);   // <-- pass ownership flag

        // Remove the drawn card
        allCards.RemoveAt(currentCardIndex);

        if (allCards.Count == 0)
            SetDeactiveDeckObject(deckObject);

        onDeckChanged?.Invoke(allCards.Count);
        DeactivateDeckIfEmpty();
    }


    public Card GetNextCard()
    {
        if (IsEmpty) return null;
        else
        if (allCards.Count == 0)
        {
            SetDeactiveDeckObject(deckObject);
            return null;
        }

        // 🔐 index sınırını kontrol et
        if (currentCardIndex >= allCards.Count)
        {
            currentCardIndex = 0;
        }

        // 🃏 karta güvenle eriş
        Card next = allCards[currentCardIndex];

        // ❌ kartı çıkar
        allCards.RemoveAt(currentCardIndex);

        // 🧹 bitti mi kontrol
        if (allCards.Count == 0)
        {
            SetDeactiveDeckObject(deckObject);
        }

        return next;
    }




    public void SetDeactiveDeckObject(GameObject deckObj)
    {
        if (deckObj != null)
        {
            deckObject = deckObj;
            deckObject.SetActive(false);

            if (deckVisibilityController != null)
            {
                deckVisibilityController.HideDeckView(); // Hide the deck view
            }
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


    public void AddCardToTable(Card card)
    {
        if (card == null) return;
        else if (tableCards.Count == 5) return; // Maximum 5 cards on the table

        GameObject newCard = Instantiate(cardPrefab, tableCardParent);
        var disp = newCard.GetComponent<CardDisplay>();

        disp.cardData = card;
        disp.SetLocation(CardLocation.Table);
        newCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(tableCards.Count * 200, 0);

        tableCards.Add(newCard);
        onDeckChanged?.Invoke(allCards.Count);
        deckCounter?.UpdateCardCountUI(allCards.Count); // Update the UI with the remaining
    }
    
    public void DeactivateDeckIfEmpty()
    {
        if (allCards.Count == 0)
            SetDeactiveDeckObject(deckObject);
    }
}
