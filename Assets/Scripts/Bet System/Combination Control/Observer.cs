using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    public GameObject playerHandPosition;
    public GameObject aiHandPosition;
    public GameObject tablePosition;

    public List<GameObject> playerCombined;
    public List<GameObject> aiCombined;

    private void Start()
    {
        playerCombined = new List<GameObject>();
        aiCombined = new List<GameObject>();
        
        // Initialize the positions if needed
        if (playerHandPosition == null)
            playerHandPosition = GameObject.Find("PlayerHandPosition");
        
        if (aiHandPosition == null)
            aiHandPosition = GameObject.Find("AIHandPosition");
        
        if (tablePosition == null)
            tablePosition = GameObject.Find("TablePosition");
    }
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            DebugPlayerHands();
        }
        else
        if (Input.GetKeyDown(KeyCode.Y))
        {
            DebugTableCards();
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            DebugAICards();
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            CombineWithAI();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            CombineWithPlayer();
        }
    }

    private void DebugPlayerHands()
    {
        int childCount = playerHandPosition.transform.childCount;
        Debug.Log($"Player Hand Position has {childCount} children.");

        for (int i = 0; i < childCount; i++)
        {
            Transform child = playerHandPosition.transform.GetChild(i);

            CardDisplay cardDisplay = child.GetComponent<CardDisplay>();
            if (cardDisplay != null && cardDisplay.cardData != null)
            {
                Debug.Log($"Player Hand Child {i} -> Name: {cardDisplay.cardData.cardName}");
            }
        }
    }

    private void DebugTableCards()
    {
        int childCount = tablePosition.transform.childCount;
        Debug.Log($"Table Cards has {childCount} children.");

        for (int i = 0; i < childCount; i++)
        {
            Transform child = tablePosition.transform.GetChild(i);

            CardDisplay cardDisplay = child.GetComponent<CardDisplay>();
            if (cardDisplay != null && cardDisplay.cardData != null)
            {
                Debug.Log($"Table Card {i} -> Name: {cardDisplay.cardData.cardName}");
            }
        }
    }

    private void DebugAICards()
    {
        int childCount = aiHandPosition.transform.childCount;
        Debug.Log($"AI has {childCount} children");

        for (int i = 0; i < childCount; i++)
        {
            Transform child = aiHandPosition.transform.GetChild(i);

            CardDisplay cardDisplay = child.GetComponent<CardDisplay>();
            if (cardDisplay != null && cardDisplay.cardData != null)
            {
                Debug.Log($"Table Card {i} -> Name: {cardDisplay.cardData.cardName}");
            }
        }
    }

    // Combine AI Cards + Table Cards
    private void CombineWithAI()
    {
        aiCombined.Clear();

        for (int i = 0; i < aiHandPosition.transform.childCount; i++)
        {
            Transform child = aiHandPosition.transform.GetChild(i);
            CardDisplay cardDisplay = child.GetComponent<CardDisplay>();

            if (cardDisplay != null && cardDisplay.cardData != null)
            {
                Debug.Log($"Combining AI Card {i} -> Name: {cardDisplay.cardData.cardName}");
                aiCombined.Add(child.gameObject);
            }
        }

        for (int i = 0; i < tablePosition.transform.childCount; i++)
        {
            Transform child = tablePosition.transform.GetChild(i);
            CardDisplay cardDisplay = child.GetComponent<CardDisplay>();

            if (cardDisplay != null && cardDisplay.cardData != null)
            {
                Debug.Log($"Combining Table Card {i} -> Name: {cardDisplay.cardData.cardName}");
                aiCombined.Add(child.gameObject);
            }
        }

        // Debug Combined AI Cards
        foreach (GameObject card in aiCombined)
        {
            CardDisplay cardDisplay = card.GetComponent<CardDisplay>();
            if (cardDisplay != null && cardDisplay.cardData != null)
            {
                Debug.Log($"Combined AI Card -> Name: {cardDisplay.cardData.cardName}");
            }
        }
    }

    // Combine Player Cards + Table Cards
    private void CombineWithPlayer()
    {
        playerCombined.Clear();

        // 1. Player hand kartlarını ekle
        for(int i = 0; i < playerHandPosition.transform.childCount; i++)
        {
            Transform child = playerHandPosition.transform.GetChild(i);
            CardDisplay cardDisplay = child.GetComponent<CardDisplay>();

            if (cardDisplay != null && cardDisplay.cardData != null)
            {
                playerCombined.Add(child.gameObject);
            }
        }

        // 2. Table kartlarını da ekle
        for (int i = 0; i < tablePosition.transform.childCount; i++)
        {
            Transform child = tablePosition.transform.GetChild(i);
            CardDisplay cardDisplay = child.GetComponent<CardDisplay>();

            if (cardDisplay != null && cardDisplay.cardData != null)
            {
                playerCombined.Add(child.gameObject);
            }
        }

        // 3. Debug çıktısı
        foreach (GameObject card in playerCombined)
        {
            CardDisplay cardDisplay = card.GetComponent<CardDisplay>();
            if (cardDisplay != null && cardDisplay.cardData != null)
            {
                Debug.Log($"Combined Player Card -> Name: {cardDisplay.cardData.cardName}");
            }
        }
    }

}