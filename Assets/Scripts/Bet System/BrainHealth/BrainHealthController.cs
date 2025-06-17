using UnityEngine;

public class BrainHealthController : MonoBehaviour
{
    [SerializeField]private BrainHealthModel brainHealthModel;
    [SerializeField]private BrainHealthView brainHealthView;
    [SerializeField]private PotController potController; // Reference to the PotController

    private void Start()
    {
        brainHealthModel = new BrainHealthModel();
    }


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