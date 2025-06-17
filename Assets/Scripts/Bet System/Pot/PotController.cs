using UnityEngine;

public class PotController : MonoBehaviour
{
    [SerializeField] private PotModel potModel;
    [SerializeField] private PotView potView;

    public void Decrease(PotModel potModel)
    {
        if (potModel.PotAmount >= 5f)
        {
            potModel.PotAmount -= 5f; // Decrease the pot amount by 5%
            potView.UpdatePotDisplay(potModel.PotAmount);
        }
        
    }

    public void Increase(PotModel potModel)
    {
        if(potModel.PotAmount >= 100f)
        {
            return; // Do not increase if pot amount is already at or above 100%
        }
        
        potModel.PotAmount += 5f; // Increase the pot amount by 5%
        potView.UpdatePotDisplay(potModel.PotAmount);
    }
}
