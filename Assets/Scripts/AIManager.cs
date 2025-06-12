using System.Collections;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public DeckManager deckManager;

    public HandManager ai1Hand;
    public HandManager ai2Hand;
    public HandManager ai3Hand;

    public float drawInterval = 1f;

    private void Start()
    {
        StartCoroutine(DrawInitialCardsForAIs());
    }

    public IEnumerator DrawCardForAISequentially(HandManager aiHand)
    {
        Card drawnCard = deckManager.GetNextCard();
        if (drawnCard != null)
        {
            yield return StartCoroutine(MoveCardToHand(aiHand, drawnCard));
        }
    }


    private IEnumerator DrawInitialCardsForAIs()
    {
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(DrawCardForAISequentially(ai1Hand));

        yield return new WaitForSeconds(drawInterval);
        yield return StartCoroutine(DrawCardForAISequentially(ai2Hand));

        yield return new WaitForSeconds(drawInterval);
        yield return StartCoroutine(DrawCardForAISequentially(ai3Hand));
    }

    public void DrawCardForAI(HandManager aiHand)
    {
        Card drawnCard = deckManager.GetNextCard();
        if (drawnCard != null)
        {
            StartCoroutine(MoveCardToHand(aiHand, drawnCard));
        }
    }

    private IEnumerator MoveCardToHand(HandManager hand, Card cardData)
  {
      // Kartı sahnede ortada oluştur
      GameObject cardObj = Instantiate(hand.cardPrefab, transform.position, Quaternion.identity, transform);
      cardObj.GetComponent<CardDisplay>().cardData = cardData;
  
      Vector3 start = cardObj.transform.position;
      Vector3 end = hand.handTransform.position;
      float duration = 0.5f;
      float elapsed = 0f;
  
      while (elapsed < duration)
      {
          cardObj.transform.position = Vector3.Lerp(start, end, elapsed / duration);
          elapsed += Time.deltaTime;
          yield return null;
      }
  
      // Pozisyonu tam oturt
      cardObj.transform.SetParent(hand.handTransform);
      cardObj.transform.localPosition = Vector3.zero;
      hand.cardsInHand.Add(cardObj);
      hand.UpdateHandVisuals();
  }
}
