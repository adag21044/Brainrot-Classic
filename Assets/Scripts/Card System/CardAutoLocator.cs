// Attach to card prefab – keeps CurrentLocation in sync with the parent
using UnityEngine;

public class CardAutoLocator : MonoBehaviour
{
    [SerializeField] private CardDisplay display;

    private void Awake() => display = GetComponent<CardDisplay>();

    private void OnTransformParentChanged()
    {
        Transform p = transform.parent;

        if      (p.CompareTag("PlayerHand")) display.SetLocation(CardLocation.PlayerHand);
        else if (p.CompareTag("AIHand"))     display.SetLocation(CardLocation.AIHand);
        else if (p.name == "TablePosition")  display.SetLocation(CardLocation.Table);
        else                                 display.SetLocation(CardLocation.Deck);
    }
}
