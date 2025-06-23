using System.Collections.Generic;    // Provides generic collections (List, Dictionary, etc.)
using System.Linq;                   // Provides LINQ extension methods
using UnityEngine;                   // Gives access to Unity engine core classes

/// <summary>
/// Detects poker-like combinations for either the Player or the AI.
/// Attach one instance for the player hand and one for the AI hand,
/// then set the <see cref="side"/> field in the Inspector.
/// </summary>
public class CombinationDetection : MonoBehaviour // Must derive from MonoBehaviour to attach to a GameObject
{
    // --------------------------------------------------------------------
    // Public / Inspector fields
    // --------------------------------------------------------------------

    public enum Side                      // Determines which participant this detector evaluates
    {
        Player,                           // Human player
        AI                                // Artificial-intelligence opponent
    }

    [SerializeField] private Side side;   // Which participant to evaluate (set in Inspector)
    [SerializeField] private Observer observer; // Reference that exposes the combined card lists

    // --------------------------------------------------------------------
    // LINQ helpers
    // --------------------------------------------------------------------

    private IEnumerable<CardDisplay> CombinedCards              // All relevant CardDisplay components
        => (side == Side.Player                                  // Choose the correct combined list
                ? observer.playerCombined                        // Player + table cards
                : observer.aiCombined)                           // AI + table cards
           .Select(go => go.GetComponent<CardDisplay>())         // Map GameObject → CardDisplay
           .Where(cd => cd != null);                             // Ignore null components (safety)

    private CardLocation OwnerLocation                           // Hand location that counts as “owner”
        => side == Side.Player ? CardLocation.PlayerHand         // PlayerHand if detector is Player
                               : CardLocation.AIHand;            // AIHand otherwise

    // --------------------------------------------------------------------
    // Unity life-cycle
    // --------------------------------------------------------------------

    private void Update()                                        // Called once per frame by Unity
    {
        if (Input.GetKeyDown(KeyCode.D))                         // Press D to run a quick evaluation
        {
            DetectCombination();                                 // Evaluate and log the best hand
        }
    }

    // --------------------------------------------------------------------
    // Combination dispatcher
    // --------------------------------------------------------------------

    private void DetectCombination()                             // Checks hands in ranking order
    {
        if      (IsQuintuple())     Debug.Log($"{side}: Quintuple");        // Five identical types
        else if (IsFourOfAKind())   Debug.Log($"{side}: Four of a Kind");   // Four identical types
        else if (IsFullHouse())     Debug.Log($"{side}: Full House");       // Three + two identical
        else if (IsThreeOfAKind())  Debug.Log($"{side}: Three of a Kind");  // Three identical
        else if (IsTwoPair())       Debug.Log($"{side}: Two Pair");         // Two separate pairs
        else if (IsSinglePair())    Debug.Log($"{side}: Single Pair");      // One pair
        else                        LogHighMeme();                          // Fallback: High Meme
    }

    // --------------------------------------------------------------------
    // Combination check helpers
    // Each returns true only when the grouping contains ≥1 owner card
    // --------------------------------------------------------------------

    private bool IsQuintuple()                                            // 5 identical types
        => CombinedCards
           .GroupBy(cd => cd.cardData.cardType[0])                        // Group by primary type
           .Any(g => g.Count() == 5 &&                                    // Exactly five in group
                     g.Any(cd => cd.currentLocation == OwnerLocation));   // At least one owner card

    private bool IsFourOfAKind()                                          // 4 identical types
        => CombinedCards
           .GroupBy(cd => cd.cardData.cardType[0])
           .Any(g => g.Count() == 4 &&
                     g.Any(cd => cd.currentLocation == OwnerLocation));

    private bool IsFullHouse()                                            // 3 + 2 identical types
    {
        var groups = CombinedCards.GroupBy(cd => cd.cardData.cardType[0]); // Cache grouping once
        bool hasThree = groups.Any(g => g.Count() == 3 &&                  // A trio that owner helps form
                                       g.Any(cd => cd.currentLocation == OwnerLocation));
        bool hasTwo   = groups.Any(g => g.Count() == 2 &&                  // A pair that owner helps form
                                       g.Any(cd => cd.currentLocation == OwnerLocation));
        return hasThree && hasTwo;                                         // Full house if both true
    }

    private bool IsThreeOfAKind()                                         // Exactly 3 identical types
        => CombinedCards
           .GroupBy(cd => cd.cardData.cardType[0])
           .Any(g => g.Count() == 3 &&
                     g.Any(cd => cd.currentLocation == OwnerLocation));

    private bool IsTwoPair()                                              // Two distinct pairs
    {
        int pairCount = CombinedCards
            .GroupBy(cd => cd.cardData.cardType[0])
            .Count(g => g.Count() == 2 &&                                 // Exactly two in group
                        g.Any(cd => cd.currentLocation == OwnerLocation)); // Owner involvement
        return pairCount == 2;                                            // True only for two pairs
    }

    private bool IsSinglePair()                                           // A single pair
    {
        int pairCount = CombinedCards
            .GroupBy(cd => cd.cardData.cardType[0])
            .Count(g => g.Count() == 2 &&
                        g.Any(cd => cd.currentLocation == OwnerLocation));
        return pairCount == 1;                                            // True only for one pair
    }

    // --------------------------------------------------------------------
    // High Meme fallback
    // --------------------------------------------------------------------

    private void LogHighMeme()                                            // Always prints best meme type
    {
        // Lower value ⇒ stronger meme
        Dictionary<Card.CardType, int> memeRank = new Dictionary<Card.CardType, int>
        {
            { Card.CardType.Sigma,      1 },
            { Card.CardType.Wholesome,  2 },
            { Card.CardType.NPC,        3 },
            { Card.CardType.Cringe,     4 },
            { Card.CardType.Brainrot,   5 }
        };

        Card.CardType bestMeme = CombinedCards
            .Where(cd => cd.currentLocation == OwnerLocation)             // Only owner cards
            .Select(cd => cd.cardData.cardType[0])                        // Project to type enum
            .DefaultIfEmpty(Card.CardType.Brainrot)                       // Safety for empty hand
            .OrderBy(t => memeRank[t])                                    // Strongest first
            .First();                                                     // Take strongest meme

        Debug.Log($"{side}: High Meme ({bestMeme})");                     // Output final result
    }
}
