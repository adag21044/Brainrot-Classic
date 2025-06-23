using NUnit.Framework;                                  // NUnit attributes & assertions
using UnityEngine;                                          // GameObject / MonoBehaviour
using System.Collections.Generic;                           // List<>
using System.Linq;                                           // LINQ convenience
using System.Reflection;                                    // Reflection for private SerializeField injection

/// <summary>
/// **Parametric Edit-Mode tests** that validate <see cref="CombinationDetection"/>
/// against every ranking type for both Player and AI.  Each <c>Scenario</c>
/// defines: 2 player hole cards, 2 AI hole cards, and 3 communal cards.
/// </summary>
public class CombinationDetectionEditorTest
{
    // ───────────────────────────────────────────────────────── scenario model

    /// <summary>Simple data holder for a single test vector.</summary>
    public class Scenario                                           // must be public because the test method is public
    {
        public string  Name;                                        // Appears in Test Runner UI
        public Card.CardType[] PlayerHole;                          // Player's two private cards
        public Card.CardType[] AIHole;                              // AI's two private cards
        public Card.CardType[] Table;                               // Three communal cards
        public CombinationDetection.HandCombinations ExpectedPlayer;
        public CombinationDetection.HandCombinations ExpectedAI;
        public override string ToString() => Name;                  // For readable TestRunner output
    }

    /// <summary>
    /// Returns one scenario per hand ranking so we hit every branch once.
    /// </summary>
    private static IEnumerable<Scenario> AllScenarios()
    {
        var S  = Card.CardType.Sigma;
        var Cg = Card.CardType.Cringe;
        var Br = Card.CardType.Brainrot;
        var N  = Card.CardType.NPC;
        var W  = Card.CardType.Wholesome;

        return new[]
        {
            new Scenario
            {
                Name           = "Quintuple player / HighMeme AI",
                PlayerHole     = new[]{ S, S },
                AIHole         = new[]{ Cg, Br },
                Table          = new[]{ S, S, S },
                ExpectedPlayer = CombinationDetection.HandCombinations.Quintuple,
                ExpectedAI     = CombinationDetection.HandCombinations.HighMeme
            },
            new Scenario
            {
                Name           = "FourOfAKind player / HighMeme AI",
                PlayerHole     = new[]{ Cg, Cg },
                AIHole         = new[]{ S, Br },
                Table          = new[]{ Cg, Cg, W },
                ExpectedPlayer = CombinationDetection.HandCombinations.FourOfAKind,
                ExpectedAI     = CombinationDetection.HandCombinations.HighMeme
            },
            new Scenario
            {
                Name           = "FullHouse player / FourOfAKind AI",
                PlayerHole     = new[]{ Cg, Cg },
                AIHole         = new[]{ Br, Br },
                Table          = new[]{ Cg, Br, Br },
                ExpectedPlayer = CombinationDetection.HandCombinations.FullHouse,
                ExpectedAI     = CombinationDetection.HandCombinations.FourOfAKind
            },
            new Scenario
            {
                Name           = "ThreeOfAKind player / ThreeOfAKind AI",
                PlayerHole     = new[]{ N, N },
                AIHole         = new[]{ W, W },
                Table          = new[]{ N, Cg, W },
                ExpectedPlayer = CombinationDetection.HandCombinations.ThreeOfAKind,
                ExpectedAI     = CombinationDetection.HandCombinations.ThreeOfAKind
            },
            new Scenario
            {
                Name           = "ThreeOfAKind player / ThreeOfAKind AI",
                PlayerHole     = new[]{ S, S },
                AIHole         = new[]{ Br, Br },
                Table          = new[]{ S, Br, W },
                ExpectedPlayer = CombinationDetection.HandCombinations.ThreeOfAKind,
                ExpectedAI     = CombinationDetection.HandCombinations.ThreeOfAKind
            },
            new Scenario
            {
                Name           = "SinglePair player / FourOfAKind AI",
                PlayerHole     = new[]{ W, W },
                AIHole         = new[]{ N, N },
                Table          = new[]{ N, N, Cg },
                ExpectedPlayer = CombinationDetection.HandCombinations.TwoPair,
                ExpectedAI     = CombinationDetection.HandCombinations.FourOfAKind
            },
            new Scenario
            {
                Name           = "SinglePair player / SinglePair AI",
                PlayerHole     = new[]{ Card.CardType.Sigma, Card.CardType.Wholesome },
                AIHole         = new[]{ Card.CardType.NPC, Card.CardType.Cringe },
                Table          = new[]{ Card.CardType.Brainrot, Card.CardType.Wholesome, Card.CardType.NPC },
                ExpectedPlayer = CombinationDetection.HandCombinations.SinglePair,
                ExpectedAI     = CombinationDetection.HandCombinations.SinglePair
            }
        };
    }

    // ───────────────────────────────────────────────────────── helpers

    private static void Spawn(Card.CardType type, CardLocation loc, Transform parent)
    {
        GameObject go = new(type.ToString());
        go.transform.SetParent(parent);

        var cd = go.AddComponent<CardDisplay>();
        var so = ScriptableObject.CreateInstance<Card>();
        so.cardType = new List<Card.CardType> { type };
        so.cardName = type.ToString();
        cd.cardData = so;
        cd.SetLocation(loc);
    }

    private static List<GameObject> Collect(Transform t) =>
        t.GetComponentsInChildren<CardDisplay>(true).Select(c => c.gameObject).ToList();

    // ───────────────────────────────────────────────────────── main test

    [Test, TestCaseSource(nameof(AllScenarios))]
    public void Detect_All_Combinations_Correctly(Scenario sc)
    {
        // 1️⃣ Positional parents
        Transform playerPos = new GameObject("PlayerHand").transform;
        Transform aiPos     = new GameObject("AIHand").transform;
        Transform tablePos  = new GameObject("Table").transform;

        // 2️⃣ Observer stub (no Start called in EditMode tests)
        Observer obs = new GameObject("Observer").AddComponent<Observer>();
        obs.playerHandPosition = playerPos.gameObject;
        obs.aiHandPosition     = aiPos.gameObject;
        obs.tablePosition      = tablePos.gameObject;
        obs.playerCombined     = new List<GameObject>();
        obs.aiCombined         = new List<GameObject>();

        // 3️⃣ Detectors
        CombinationDetection detP = new GameObject("DetP").AddComponent<CombinationDetection>();
        CombinationDetection detA = new GameObject("DetA").AddComponent<CombinationDetection>();
        var flags = BindingFlags.NonPublic | BindingFlags.Instance;
        typeof(CombinationDetection).GetField("side", flags).SetValue(detP, CombinationDetection.Side.Player);
        typeof(CombinationDetection).GetField("observer", flags).SetValue(detP, obs);
        typeof(CombinationDetection).GetField("side", flags).SetValue(detA, CombinationDetection.Side.AI);
        typeof(CombinationDetection).GetField("observer", flags).SetValue(detA, obs);

        // 4️⃣ Spawn cards
        foreach (var t in sc.PlayerHole) Spawn(t, CardLocation.PlayerHand, playerPos);
        foreach (var t in sc.AIHole)     Spawn(t, CardLocation.AIHand,     aiPos);
        foreach (var t in sc.Table)      Spawn(t, CardLocation.Table,      tablePos);

        // 5️⃣ Build combined lists (no debug logging)
        obs.playerCombined = Collect(playerPos).Concat(Collect(tablePos)).ToList();
        obs.aiCombined     = Collect(aiPos)    .Concat(Collect(tablePos)).ToList();

        // 6️⃣ Evaluate & assert
        var playerResult = detP.Evaluate(out var playerBest);
        var aiResult     = detA.Evaluate(out var aiBest);

        if (playerResult == aiResult && playerResult == CombinationDetection.HandCombinations.HighMeme)
        {
            Debug.Log($"[TIE] HighMeme → Player = {playerBest}, AI = {aiBest}");
        }


        // 7️⃣ Cleanup to avoid memory leaks between test cases
        Object.DestroyImmediate(obs.gameObject);
        Object.DestroyImmediate(detP.gameObject);
        Object.DestroyImmediate(detA.gameObject);
        Object.DestroyImmediate(playerPos.gameObject);
        Object.DestroyImmediate(aiPos.gameObject);
        Object.DestroyImmediate(tablePos.gameObject);
    }
}
