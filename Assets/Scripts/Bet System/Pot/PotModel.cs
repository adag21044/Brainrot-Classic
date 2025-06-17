using UnityEngine;

public class PotModel : MonoBehaviour
{
    [SerializeField] private float potAmount = 10f; // Amount of percantage in the pot

    public float PotAmount
    {
        get { return potAmount; }
        set { potAmount = value; }
    }
}
