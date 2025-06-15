using UnityEngine;

public class BrainHealthController : MonoBehaviour
{
    BrainHealthModel brainHealthModel;
    BrainHealthView brainHealthView;

    public void Bet()
    {
        // Simulate a bet that reduces brain health
        float betAmount = 10f; // Example bet amount
        ReduceBrainHealth(betAmount);
    }

    private void ReduceBrainHealth(float amount)
    {
        // Reduce the brain health by the bet amount
        brainHealthModel.HealthPercentage -= amount;

        // Ensure health does not go below 0
        if (brainHealthModel.HealthPercentage < 0)
        {
            brainHealthModel.HealthPercentage = 0;
        }

        // Update the view to reflect the new health percentage
        brainHealthView.UpdateHealthDisplay(brainHealthModel.HealthPercentage);
    }
}