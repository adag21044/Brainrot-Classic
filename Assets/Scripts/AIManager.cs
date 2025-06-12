using System.Collections;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public DeckManager deckManager;
    public HandManager aiHand;

    public IEnumerator DrawCardForAI()
    {
        Card drawnCard = deckManager.GetNextCard();
        if (drawnCard != null)
        {
            yield return new WaitForSeconds(0.5f); 
            aiHand.AddCardToHand(drawnCard);
        }
    }
}
