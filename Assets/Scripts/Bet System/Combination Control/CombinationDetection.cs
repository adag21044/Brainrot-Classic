using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombinationDetection : MonoBehaviour
{
    [SerializeField] private Observer observer;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            DetectCombination();
        }
    }

    private void DetectCombination()
    {
        if (isQuintuple())
            Debug.Log("Quintuple detected");
        else if (isFourOfAKind())
            Debug.Log("Four of a Kind detected");
        else if (isFullHouse())
            Debug.Log("Full House detected");
        else if (isTwoPair())
            Debug.Log("Two Pair detected");
        else if (isSinglePair())
            Debug.Log("Single Pair detected");
        else
            Debug.Log("High Meme detected");
    }

    // Converts GameObject list to card type list
    private List<Card.CardType> GetPlayerCardTypes()
    {
        var types = observer.playerCombined
            .Select(go => go.GetComponent<CardDisplay>().cardData.cardType[0])
            .ToList();

        Debug.Log("Current card types: " + string.Join(", ", types));
        return types;
    }


    private bool isQuintuple()
    {
        List<Card.CardType> types = GetPlayerCardTypes();
        return types.GroupBy(t => t).Any(g => g.Count() == 5);
    }

    private bool isFourOfAKind()
    {
        List<Card.CardType> types = GetPlayerCardTypes();
        return types.GroupBy(t => t).Any(g => g.Count() == 4);
    }

    private bool isFullHouse()
    {
        List<Card.CardType> types = GetPlayerCardTypes();
        var grouped = types.GroupBy(t => t).Select(g => g.Count()).ToList();
        return grouped.Contains(3) && grouped.Contains(2);
    }

    private bool isTwoPair()
    {
        List<Card.CardType> types = GetPlayerCardTypes();
        int pairCount = types.GroupBy(t => t).Count(g => g.Count() == 2);
        return pairCount == 2;
    }

    private bool isSinglePair()
    {
        List<Card.CardType> types = GetPlayerCardTypes();
        int pairCount = types.GroupBy(t => t).Count(g => g.Count() == 2);
        return pairCount == 1;
    }


    private bool isHighMeme()
    {
        // Meme type ranks (lower is better)
        Dictionary<Card.CardType, int> memeRanking = new Dictionary<Card.CardType, int>()
        {
            { Card.CardType.Sigma, 1 },
            { Card.CardType.Wholesome, 2 },
            { Card.CardType.NPC, 3 },
            { Card.CardType.Cringe, 4 },
            { Card.CardType.Brainrot, 5 },
        };

        List<Card.CardType> types = GetPlayerCardTypes();
        Card.CardType best = types.OrderBy(t => memeRanking[t]).First();
        Debug.Log("High meme type: " + best.ToString());
        return true; // Always true, lowest ranked is printed
    }
}
