using UnityEngine;

public class Observer : MonoBehaviour
{
    public GameObject playerHandPosition;
    public GameObject aiHandPosition;
    public GameObject tablePosition;

    public GameObject[] playerCombined;
    public GameObject[] aiCombined;
    

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
            FindCombinations();
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

    private void CombineWithAI()
    {

    }

    private void FindCombinations()
    {

    }
}