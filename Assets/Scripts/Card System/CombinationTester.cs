using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Draws 100 000 random 5-card hands from a 25-card deck (5 of each meme type)
/// and prints the empirical probability of every combination.
/// Attach to an empty GameObject and press Play.
/// </summary>
public class CombinationTester : MonoBehaviour
{
    [SerializeField] private int testCount = 100_000;

    // Result counters
    private readonly Dictionary<Combination, int> counter = new();

    // Immutable 25-card deck (5 copies of each type)
    private readonly List<Card.CardType> fullDeck = new();

    private void Awake()
    {
        // init counter dict
        foreach (Combination c in System.Enum.GetValues(typeof(Combination)))
            counter[c] = 0;

        // init deck
        foreach (Card.CardType t in System.Enum.GetValues(typeof(Card.CardType)))
            for (int i = 0; i < 5; i++)
                fullDeck.Add(t);
    }

    private void Start() => RunSimulation();

    // ───────────────────────────────────────────────────────── simulation
    private void RunSimulation()
    {
        System.Random rng = new();           // Faster & repeatable than UnityEngine.Random

        for (int i = 0; i < testCount; i++)
        {
            // quick shuffle-and-take
            List<Card.CardType> hand = fullDeck
                .OrderBy(_ => rng.Next())
                .Take(5)
                .ToList();

            counter[Evaluate(hand)]++;
        }

        // print
        Debug.Log($"Simulated {testCount:N0} random 5-card hands:");
        foreach (var kvp in counter.OrderBy(k => k.Key))
        {
            float pct = kvp.Value * 100f / testCount;
            Debug.Log($"{kvp.Key,-13}: {kvp.Value,8:N0}  ({pct:F4} %)");
        }
    }

    // ───────────────────────────────────────────────────────── evaluation
    private static Combination Evaluate(List<Card.CardType> hand)
    {
        var groups = hand
            .GroupBy(t => t)
            .Select(g => g.Count())
            .OrderByDescending(c => c)
            .ToList();                       // e.g. [3,2] for full-house

        return groups switch
        {
            var g when g.SequenceEqual(new[] { 5 })           => Combination.Quintuple,
            var g when g.SequenceEqual(new[] { 4, 1 })        => Combination.FourOfAKind,
            var g when g.SequenceEqual(new[] { 3, 2 })        => Combination.FullHouse,
            var g when g.SequenceEqual(new[] { 3, 1, 1 })     => Combination.ThreeOfAKind,
            var g when g.SequenceEqual(new[] { 2, 2, 1 })     => Combination.TwoPair,
            var g when g.SequenceEqual(new[] { 2, 1, 1, 1 })  => Combination.SinglePair,
            _                                                 => Combination.HighMeme
        };
    }

    private enum Combination
    {
        HighMeme,
        SinglePair,
        TwoPair,
        ThreeOfAKind,
        FullHouse,
        FourOfAKind,
        Quintuple
    }
}
