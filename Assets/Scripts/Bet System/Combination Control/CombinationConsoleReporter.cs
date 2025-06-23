using UnityEngine;

/// <summary>
/// Prints both Player+Table and AI+Table combinations to the Console.
/// Attach this to any active GameObject and link the two detectors.
/// </summary>
public class CombinationConsoleReporter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CombinationDetection playerDetector; // Player instance
    [SerializeField] private CombinationDetection aiDetector;     // AI instance
    Card.CardType _; 

    private void Update()
    {
        // Press P to print results in order: Player first, then AI
        if (Input.GetKeyDown(KeyCode.P))
            PrintBothHands();
    }

    private void PrintBothHands()
    {
        var playerComb = playerDetector.Evaluate(out _);
        Debug.Log($"PLAYER + Table ⇒ {playerComb}");

        var aiComb     = aiDetector.Evaluate(out _);
        Debug.Log($"AI     + Table ⇒ {aiComb}");
    }
}
