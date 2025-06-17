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
    [SerializeField] private DeckVisibilityController deckVisibilityController;
    public bool IsEmpty => allCards.Count == 0;

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

        Debug.Log($"Total cards loaded from Resources: {allCards.Count}");

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
        if (allCards.Count == 0)
        {
            SetDeactiveDeckObject(deckObject);
            return;
        }

        if (currentCardIndex >= allCards.Count)
            currentCardIndex = 0;

        Card nextCard = allCards[currentCardIndex];
        handManager.AddCardToHand(nextCard);

        // Remove the card from the deck
        allCards.RemoveAt(currentCardIndex);

        if (allCards.Count == 0)
            SetDeactiveDeckObject(deckObject);

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
    
    public void DeactivateDeckIfEmpty()
    {
        if (allCards.Count == 0)
            SetDeactiveDeckObject(deckObject);
    }
}
