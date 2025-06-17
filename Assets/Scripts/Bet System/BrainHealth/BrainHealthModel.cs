using UnityEngine;

public class BrainHealthModel
{
    [SerializeField] private float healthPercentage = 100f; // Max brain health

    public float HealthPercentage
    {
        get { return healthPercentage; }
        set { healthPercentage = value; }
    }
}
