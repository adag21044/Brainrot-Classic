using UnityEngine;
using TMPro;

public class PotView : MonoBehaviour
{
    [SerializeField] private TMP_Text potAmountText; // UI Text to display pot amount

    public void UpdatePotDisplay(float potAmount)
    {
        // Update the UI text with the current pot amount
        potAmountText.text = $"{potAmount:F2}%";
    }
}
