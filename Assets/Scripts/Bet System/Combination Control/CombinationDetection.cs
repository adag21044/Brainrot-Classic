using System.Collections.Generic;                                    // Provides generic collection classes
using System.Linq;                                                   // Enables LINQ extension methods
using UnityEngine;                                                   // Core Unity engine namespace

public class CombinationDetection : MonoBehaviour                    // MonoBehaviour so Unity can attach it to a GameObject
{
    //------------------------------------------------------------------------
    // Public / Inspector fields
    //------------------------------------------------------------------------
    public enum Side                                                  // Defines which hand this detector evaluates
    {
        Player,                                                       // Evaluate the human player's cards
        AI                                                            // Evaluate the AI opponent's cards
    }

    [SerializeField] private Side side;                               // Which side to inspect (set from Inspector)
    [SerializeField] private Observer observer;                       // Reference to Observer that holds combined card lists

    //------------------------------------------------------------------------
    // Unity life-cycle
    //------------------------------------------------------------------------
    private void Update()                                             // Called once per frame by Unity
    {
        if (Input.GetKeyDown(KeyCode.D))                              // If the D key is pressed
        {
            DetectCombination();                                      // Run combination detection
        }
    }

    //------------------------------------------------------------------------
    // Main combination dispatcher
    //------------------------------------------------------------------------
    private void DetectCombination()                                  // Evaluates all hand rankings in priority order
    {
        if      (IsQuintuple())     Debug.Log($"{side}: Quintuple");  // Check five-of-a-kind
        else if (IsFourOfAKind())   Debug.Log($"{side}: Four-of-a-Kind"); // Check four-of-a-kind
        else if (IsFullHouse())     Debug.Log($"{side}: Full House"); // Check full house
        else if (IsThreeOfAKind())  Debug.Log($"{side}: Three-of-a-Kind"); // Check three-of-a-kind
        else if (IsTwoPair())       Debug.Log($"{side}: Two Pair");   // Check two pairs
        else if (IsSinglePair())    Debug.Log($"{side}: Single Pair");// Check single pair
        else if (IsHighMeme())      Debug.Log($"{side}: High Meme");   // Always true, logs best meme type
        
    }

    //------------------------------------------------------------------------
    // Helpers — data access
    //------------------------------------------------------------------------
    private IEnumerable<CardDisplay> Combined =>
        (side == Side.Player ? observer.playerCombined : observer.aiCombined)
        .Select(go => go.GetComponent<CardDisplay>())
        .Where(cd => cd != null);

    private CardLocation RequiredHandLocation =>                      // Required location must belong to current side’s hand
        side == Side.Player ? CardLocation.PlayerHand                 // Player’s hand location
                            : CardLocation.AIHand;                    // AI’s hand location

    private void Log(HandCombinations combo) =>
        Debug.Log($"{side} ⇒ {combo}");

    //------------------------------------------------------------------------
    // Combination checks
    //------------------------------------------------------------------------
     private bool IsQuintuple() =>
        Combined.GroupBy(cd => cd.cardData.cardType[0])
                .Any(g => g.Count() == 5 &&
                          g.Any(cd => cd.currentLocation == RequiredHandLocation));

    private bool IsFourOfAKind() =>
        Combined.GroupBy(cd => cd.cardData.cardType[0])
                .Any(g => g.Count() == 4 &&
                          g.Any(cd => cd.currentLocation == RequiredHandLocation));

    private bool IsFullHouse()
    {
        var groups = Combined.GroupBy(cd => cd.cardData.cardType[0]);
        bool hasThree = groups.Any(g => g.Count() == 3 &&
                                        g.Any(cd => cd.currentLocation == RequiredHandLocation));
        bool hasTwo   = groups.Any(g => g.Count() == 2 &&
                                        g.Any(cd => cd.currentLocation == RequiredHandLocation));
        return hasThree && hasTwo;
    }

    private bool IsThreeOfAKind() =>
        Combined.GroupBy(cd => cd.cardData.cardType[0])
                .Any(g => g.Count() == 3 &&
                          g.Any(cd => cd.currentLocation == RequiredHandLocation));

    private bool IsTwoPair()
    {
        int pairs = Combined.GroupBy(cd => cd.cardData.cardType[0])
                            .Count(g => g.Count() == 2 &&
                                        g.Any(cd => cd.currentLocation == RequiredHandLocation));
        return pairs == 2;
    }

    private bool IsSinglePair()
    {
        int pairs = Combined.GroupBy(cd => cd.cardData.cardType[0])
                            .Count(g => g.Count() == 2 &&
                                        g.Any(cd => cd.currentLocation == RequiredHandLocation));
        return pairs == 1;
    }

    //------------------------------------------------------------------------
    // High-Meme check (fallback ranking)
    //------------------------------------------------------------------------
    private bool IsHighMeme()
    {
        // Manual ranking dictionary: lower value = stronger meme
        var ranking = new Dictionary<Card.CardType, int>
        {
            { Card.CardType.Sigma,      1 },
            { Card.CardType.Wholesome,  2 },
            { Card.CardType.NPC,        3 },
            { Card.CardType.Cringe,     4 },
            { Card.CardType.Brainrot,   5 }
        };

        // Filter only cards that belong to the current hand (required location)
        IEnumerable<Card.CardType> handTypes = Combined
            .Where(cd => cd.currentLocation == RequiredHandLocation) // keep only own cards
            .Select(cd => cd.cardData.cardType[0]);                  // project to type enum

        // In rare case the hand is empty (should not happen), default to worst type
        Card.CardType bestType = handTypes
            .DefaultIfEmpty(Card.CardType.Brainrot)  // avoid empty sequence error
            .OrderBy(t => ranking[t])                // sort ascending by strength
            .First();                                // pick strongest (lowest rank value)

        // Output to console for debugging
        Debug.Log($"{side} ⇒ High Meme ({bestType})");

        return true; // Always returns true so DetectCombination() stops here
    }

    //-------------  High Meme fallback  -------------------------------------
    private void LogHighMeme()
    {
        var ranking = new Dictionary<Card.CardType, int> {
            { Card.CardType.Sigma, 1 },
            { Card.CardType.Wholesome, 2 },
            { Card.CardType.NPC, 3 },
            { Card.CardType.Cringe, 4 },
            { Card.CardType.Brainrot, 5 }
        };

        // Pick best meme type contributed by the required hand
        var best = Combined
            .Where(cd => cd.currentLocation == RequiredHandLocation)
            .Select(cd => cd.cardData.cardType[0])
            .DefaultIfEmpty(Card.CardType.Brainrot)  // Safety: avoid empty sequence
            .OrderBy(t => ranking[t])
            .First();

        Debug.Log($"{side} ⇒ HighMeme ({best})");
    }
}
