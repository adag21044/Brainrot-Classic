using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombinationDetection : MonoBehaviour
{
    [SerializeField] private Observer observer;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
            DetectCombination();
    }

    private void DetectCombination()
    {
        if (IsQuintuple())              Debug.Log("Quintuple detected");
        else if (IsFourOfAKind())       Debug.Log("Four of a Kind detected");
        else if (IsFullHouse())         Debug.Log("Full House detected");
        else if (IsThreeOfAKind())      Debug.Log("Three of a Kind detected");
        else if (IsTwoPair()) Debug.Log("Two Pair detected");
        else if (IsSinglePair()) Debug.Log("Single Pair detected");
        else Debug.Log("High Meme detected");
    }

    // Helper: get all CardDisplay components from combined list
    private IEnumerable<CardDisplay> GetCombinedDisplays()
    {
        return observer.playerCombined
            .Select(go => go.GetComponent<CardDisplay>())
            .Where(cd => cd != null);
    }

    private bool IsQuintuple()
    {
        // 5 of a kind can't happen with 3 table + 2 hand, but example:
        return GetCombinedDisplays()
            .GroupBy(cd => cd.cardData.cardType[0])
            .Any(g => g.Count() == 5 && g.Any(cd => cd.currentLocation == CardLocation.PlayerHand));
    }

    private bool IsFourOfAKind()
    {
        // 4 same type, at least one from player
        return GetCombinedDisplays()
            .GroupBy(cd => cd.cardData.cardType[0])
            .Any(g => g.Count() == 4 && g.Any(cd => cd.currentLocation == CardLocation.PlayerHand));
    }

    private bool IsFullHouse()
    {
        var groups = GetCombinedDisplays()
            .GroupBy(cd => cd.cardData.cardType[0]);

        // three-of-a-kind group
        bool hasThree = groups
            .Any(g => g.Count() == 3 && g.Any(cd => cd.currentLocation == CardLocation.PlayerHand));
        // pair group
        bool hasTwo = groups
            .Any(g => g.Count() == 2 && g.Any(cd => cd.currentLocation == CardLocation.PlayerHand));

        return hasThree && hasTwo;
    }

    // Checks if there are exactly 3 cards of the same type,
    // and at least one of them comes from the player's hand.
    private bool IsThreeOfAKind()
    {
        // Get all CardDisplay components from the combined cards (player + table)
        var groupedTypes = GetCombinedDisplays()
            .GroupBy(cd => cd.cardData.cardType[0]); // Group by first card type

        foreach (var group in groupedTypes)
        {
            // Check if exactly 3 cards of same type exist
            if (group.Count() == 3)
            {
                // Ensure at least one of them is from the player's hand
                if (group.Any(cd => cd.currentLocation == CardLocation.PlayerHand))
                {
                    return true;
                }
            }
        }

        return false;
    }


    private bool IsTwoPair()
    {
        // count how many valid pairs (count==2 AND at least one from player)
        int validPairs = GetCombinedDisplays()
            .GroupBy(cd => cd.cardData.cardType[0])
            .Count(g => g.Count() == 2 && g.Any(cd => cd.currentLocation == CardLocation.PlayerHand));

        return validPairs == 2;
    }

    private bool IsSinglePair()
    {
        // exactly one valid pair
        int validPairs = GetCombinedDisplays()
            .GroupBy(cd => cd.cardData.cardType[0])
            .Count(g => g.Count() == 2 && g.Any(cd => cd.currentLocation == CardLocation.PlayerHand));

        return validPairs == 1;
    }

    private bool IsHighMeme()
    {
        // if no other combination, pick the highest-ranked meme from player's contributions
        Dictionary<Card.CardType, int> ranking = new Dictionary<Card.CardType, int>
        {
            { Card.CardType.Sigma,     1 },
            { Card.CardType.Wholesome,  2 },
            { Card.CardType.NPC,        3 },
            { Card.CardType.Cringe,     4 },
            { Card.CardType.Brainrot,   5 },
        };

        // only consider player's cards for HighMeme ranking
        Card.CardType best = GetCombinedDisplays()
            .Where(cd => cd.currentLocation == CardLocation.PlayerHand)
            .Select(cd => cd.cardData.cardType[0])
            .OrderBy(t => ranking[t])
            .FirstOrDefault();

        Debug.Log("High meme type: " + best);
        return true;
    }
}
