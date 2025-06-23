using System.Collections.Generic;   // List, Dictionary, ...
using System.Linq;                  // LINQ extensions
using UnityEngine;                  // Unity core API

/// <summary>
/// Evaluates poker-like hand strength for either the human Player or the AI.
/// One instance is attached for each side and configured in the Inspector.
/// </summary>
public class CombinationDetection : MonoBehaviour
{
    // ───────────────────────────────────────────────────────────── public data
    public enum Side { Player, AI }

    [SerializeField] private Side side;      // Which participant
    [SerializeField] private Observer observer;  // Provides combined lists

    // ───────────────────────────────────────────────────── HandCombinations
    public enum HandCombinations
    {
        HighMeme,
        SinglePair,
        TwoPair,
        ThreeOfAKind,
        FullHouse,
        FourOfAKind,
        Quintuple
    }

    // ────────────────────────────────────────────────── private helpers

    private CardLocation OwnerLocation =>
        side == Side.Player ? CardLocation.PlayerHand
                            : CardLocation.AIHand;

    /// <summary>Returns a fresh 7-card set (2 hole + table).</summary>
    private IEnumerable<CardDisplay> GetCombinedCards()
    {
        if (side == Side.Player)
            observer.CombineWithPlayer();
        else
            observer.CombineWithAI();

        var raw = side == Side.Player ? observer.playerCombined
                                      : observer.aiCombined;

        return raw.Select(go => go.GetComponent<CardDisplay>())
                  .Where(cd => cd != null);
    }

    private bool IsOwner(CardDisplay cd) => cd.currentLocation == OwnerLocation;

    // ──────────────────────────────────────────────── public evaluation API

    /// <summary>
    /// Fully evaluates the current hand and returns its best combination.
    /// </summary>
    public HandCombinations Evaluate(out Card.CardType bestMeme)
    {
        if (IsQuintuple())     { bestMeme = default; return HandCombinations.Quintuple; }
        if (IsFourOfAKind())   { bestMeme = default; return HandCombinations.FourOfAKind; }
        if (IsFullHouse())     { bestMeme = default; return HandCombinations.FullHouse; }
        if (IsThreeOfAKind())  { bestMeme = default; return HandCombinations.ThreeOfAKind; }
        if (IsTwoPair())       { bestMeme = default; return HandCombinations.TwoPair; }
        if (IsSinglePair())    { bestMeme = default; return HandCombinations.SinglePair; }

        bestMeme = GetBestMemeType(GetCombinedCards());
        return HandCombinations.HighMeme;
    }

    public HandCombinations Evaluate()
    {
        return Evaluate(out _); // sadece kombinasyonu döndür, meme türünü at
    }



    /// <summary>
    /// Shortcut that evaluates and logs immediately (useful for quick tests).
    /// </summary>
    public void DetectAndLog() =>
        Debug.Log($"{side}: {Evaluate()}");

    // ───────────────────────────────────────────────────── Unity update

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))   // press D on each detector to test
            DetectAndLog();
    }

    // ───────────────────────────────────────────── combination predicates

    private bool IsQuintuple() =>
        GetCombinedCards()
           .GroupBy(cd => cd.cardData.cardType[0])
           .Any(g => g.Count() == 5 && g.Any(IsOwner));

    private bool IsFourOfAKind() =>
        GetCombinedCards()
           .GroupBy(cd => cd.cardData.cardType[0])
           .Any(g => g.Count() == 4 && g.Any(IsOwner));

    private bool IsFullHouse()
    {
        var groups = GetCombinedCards().GroupBy(cd => cd.cardData.cardType[0]).ToList();

        var trio = groups.FirstOrDefault(g => g.Count() >= 3);
        if (trio != null)
        {
            var pair = groups.FirstOrDefault(g =>
                g.Key != trio.Key && g.Count() >= 2);

            if (pair != null)
            {
                // At least one of the 5 cards (in total) must come from hand
                return trio.Any(IsOwner) || pair.Any(IsOwner);
            }
        }
        return false;

    }


    private bool IsThreeOfAKind()
    {
        var groups = GetCombinedCards().GroupBy(cd => cd.cardData.cardType[0]);
        return groups.Any(g => g.Count() >= 3 && g.Count() < 5 && g.Any(IsOwner));
    }


    private bool IsTwoPair()
    {
        var groups = GetCombinedCards().GroupBy(cd => cd.cardData.cardType[0]).ToList();

        // Find all groups that form a valid pair (count >= 2)
        var pairGroups = groups.Where(g => g.Count() >= 2).ToList();

        // Two different pairs found?
        if (pairGroups.Count >= 2)
        {
            // Check if at least one of those pairs has a card from hand
            return pairGroups.Any(g => g.Any(IsOwner));
        }

        return false;
    }



    private bool IsSinglePair()
    {
        var groups = GetCombinedCards().GroupBy(cd => cd.cardData.cardType[0]);
        var ownerPairs = groups.Where(g => g.Count() == 2 && g.Any(IsOwner));
        return ownerPairs.Count() == 1;
    }
    
    private Card.CardType GetBestMemeType(IEnumerable<CardDisplay> cards)
    {
        var ranking = new Dictionary<Card.CardType, int>
        {
            { Card.CardType.Sigma,     5 },
            { Card.CardType.Wholesome, 4 },
            { Card.CardType.NPC,       3 },
            { Card.CardType.Cringe,    2 },
            { Card.CardType.Brainrot,  1 }
        };

        return cards
            .Where(cd => cd.currentLocation == OwnerLocation) // only hand cards
            .Select(cd => cd.cardData.cardType[0])
            .OrderByDescending(t => ranking[t])
            .DefaultIfEmpty(Card.CardType.Brainrot)
            .First();
    }

}
