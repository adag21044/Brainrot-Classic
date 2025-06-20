using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombinationTester : MonoBehaviour
{
    [SerializeField] private int testCount = 100000; // Number of hands to simulate

    // Counters for each combination type
    private int quintupleCount = 0;
    private int fourOfAKindCount = 0;
    private int fullHouseCount = 0;
    private int threeOfAKindCount = 0;
    private int twoPairCount = 0;
    private int singlePairCount = 0;
    private int highMemeCount = 0;

    // The full deck: 5 types × 5 cards each = 25 total cards
    private List<Card.CardType> fullDeck = new List<Card.CardType>();

    private void Start()
    {
        RunSimulation(); // Begin simulation on start
    }

    // Runs the Monte Carlo simulation
    private void RunSimulation()
    {
        // Build the full deck with 5 copies of each card type
        foreach (Card.CardType type in System.Enum.GetValues(typeof(Card.CardType)))
        {
            for (int i = 0; i < 5; i++)
                fullDeck.Add(type);
        }

        // Repeat for the given number of test runs
        for (int i = 0; i < testCount; i++)
        {
            List<Card.CardType> hand = DrawFiveRandom(); // Draw a random hand
            CountCombinations(hand); // Evaluate the hand
        }

        // Output results to the console
        Debug.Log($"Out of {testCount} hands:");
        Print("Quintuple", quintupleCount);
        Print("Four of a Kind", fourOfAKindCount);
        Print("Full House", fullHouseCount);
        Print("Three of a Kind", threeOfAKindCount);
        Print("Two Pair", twoPairCount);
        Print("Single Pair", singlePairCount);
        Print("High Meme", highMemeCount);
    }

    // Prints a labeled percentage result
    private void Print(string label, int count)
    {
        float percent = (float)count / testCount * 100f;
        Debug.Log($"{label}: {count} times → %{percent:F4}");
    }

    // Randomly draws 5 cards from the deck
    private List<Card.CardType> DrawFiveRandom()
    {
        List<Card.CardType> copy = new List<Card.CardType>(fullDeck); // Create a fresh copy of the deck
        List<Card.CardType> hand = new List<Card.CardType>(); // Holds the drawn hand

        for (int i = 0; i < 5; i++)
        {
            int index = Random.Range(0, copy.Count); // Pick a random index
            hand.Add(copy[index]); // Add selected card to the hand
            copy.RemoveAt(index); // Remove it from the deck copy
        }

        return hand;
    }

    // Checks and counts what kind of combination the hand contains
    private void CountCombinations(List<Card.CardType> hand)
    {
        // Group cards by type and count how many of each type
        var groups = hand.GroupBy(t => t).Select(g => g.Count()).OrderByDescending(c => c).ToList();

        // Check from highest to lowest combination
        if (groups.SequenceEqual(new List<int> { 5 }))
        {
            quintupleCount++;
        }
        else if (groups.SequenceEqual(new List<int> { 4, 1 }))
        {
            fourOfAKindCount++;
        }
        else if (groups.SequenceEqual(new List<int> { 3, 2 }))
        {
            fullHouseCount++;
        }
        else if (groups.SequenceEqual(new List<int> { 3, 1, 1 }))
        {
            threeOfAKindCount++;
        }
        else if (groups.SequenceEqual(new List<int> { 2, 2, 1 }))
        {
            twoPairCount++;
        }
        else if (groups.SequenceEqual(new List<int> { 2, 1, 1, 1 }))
        {
            singlePairCount++;
        }
        else
        {
            highMemeCount++; // If nothing else matches, count as High Meme
        }
    }
}
