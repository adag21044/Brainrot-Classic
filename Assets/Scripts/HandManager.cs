using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public DeckManager deckManager; // Reference to the DeckManager to access the deck
    public GameObject cardPrefab; // Assign the card prefab in the inspector
    public Transform handTransform;
    public float fanSpread = 7.5f;
    public float cardSpacing = 100f; // Spacing between cards
    public float verticalSpacing = 100f;
    public List<GameObject> cardsInHand = new List<GameObject>(); // List to keep track of cards in hand

    void Start()
    {
        
    }

    void Update()
    {
        // Update the hand visuals every frame
        //UpdateHandVisuals();
    }

    public void AddCardToHand(Card cardData)
    {
        // Instantiate the card
        GameObject newCard = Instantiate(cardPrefab, handTransform.position, Quaternion.identity, handTransform);
        cardsInHand.Add(newCard);

        // Set the cardData of the instantiated card
        newCard.GetComponent<CardDisplay>().cardData = cardData;

        UpdateHandVisuals();
    }



    private void UpdateHandVisuals()
    {
        int cardCount = cardsInHand.Count;

        if (cardCount == 1)
        {
            cardsInHand[0].transform.localRotation = Quaternion.Euler(0f, 0f, 0f); // Center the single card
            cardsInHand[0].transform.localPosition = new Vector3(0f, 0f, 0f);
            return; // No need to update positions for a single card
        }

        for (int i = 0; i < cardCount; i++)
        {
            float rotationAngle = (fanSpread * (i - (cardCount - 1) / 2f));
            cardsInHand[i].transform.localRotation = Quaternion.Euler(0f, 0f, rotationAngle);

            float horizantalOffset = (cardSpacing * (i - (cardCount - 1) / 2f));

            float normalizedPosition = (2f * i / (cardCount - 1)) - 1f; // Normalize position to range [-1, 1]
            float verticalOffset = verticalSpacing * (1 - normalizedPosition * normalizedPosition); // Calculate vertical offset based on normalized position

            // Set the position of the card
            cardsInHand[i].transform.localPosition = new Vector3(horizantalOffset, verticalOffset, 0f);
        }
    }
}
