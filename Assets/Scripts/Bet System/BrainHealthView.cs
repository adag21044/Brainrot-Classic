using TMPro;
using UnityEngine;

public class BrainHealthView : MonoBehaviour
{
    [SerializeField] private TMP_Text healthPercentageText; // UI Text to display health percentage

    public void UpdateHealthDisplay(float healthPercentage)
    {
        // Update the UI text with the current health percentage
        healthPercentageText.text = $"Brain Health: {healthPercentage:F2}%";
    }
}