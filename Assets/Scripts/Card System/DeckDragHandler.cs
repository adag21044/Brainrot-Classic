using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DeckDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("References")]
    public DeckManager deckManager;
    public GameObject cardPrefab;           
    public HandManager handManager;

    [Header("Deck View Settings")]
    public int initialDeckSize = 17;           //How many face‑down cards are visible.
    public Vector2 cardOffset = new Vector2(4f, -4f); // Visual offset between cards.

    
    Canvas canvas;
    Card draggedCardData;
    GameObject preview;
    [SerializeField] private GameObject[] cardsToBeDeleted;
    [SerializeField] private int deleteIndex = 0;
    [SerializeField] private int maxDeleteCount = 26; // Max kart 
    public float snapDistance = 100f;

    [SerializeField] private DeckVisibilityController deckVisibilityController; // Reference to the DeckVisibilityController for hiding deck view

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData evt) {
        // 1) Kart verisini çek
        draggedCardData = deckManager.GetNextCard();
        if (draggedCardData == null) return;

        // 2) Preview instantiate: canvas’ın altında, ekran konumuna göre
        preview = Instantiate(cardPrefab, canvas.transform);
        var disp = preview.GetComponent<CardDisplay>();
        disp.cardData = draggedCardData;
        var cg = preview.AddComponent<CanvasGroup>();
        cg.blocksRaycasts = false; // raycast’leri engelle
        preview.transform.SetAsLastSibling();

        // 3) Hemen fare pozisyonuna taşı
        OnDrag(evt);
    }

    public void OnDrag(PointerEventData evt) {
        if (preview == null) return;
        // Eğer Screen Space−Overlay ise:
        preview.transform.position = evt.position;
        // Eğer başka render moda sahipse, RectTransformUtility ile dönüştür
    }

    public void OnEndDrag(PointerEventData evt)
    {
        if (preview == null) return;

        Vector3 worldPoint;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            handManager.handTransform.GetComponent<RectTransform>(),
            evt.position,
            evt.pressEventCamera,
            out worldPoint);

        float distance = Vector3.Distance(worldPoint, handManager.handTransform.position);

        if (distance <= snapDistance)
        {
            // El bölgesine yakın: ekle
            handManager.AddCardToHand(draggedCardData);
            Destroy(preview);
            preview = null;

            if (deckManager.allCards.Count > 0)
            {
                if (deleteIndex < cardsToBeDeleted.Length)
                {
                    Destroy(cardsToBeDeleted[deleteIndex]);
                }
                deleteIndex++;
                maxDeleteCount--;
            }
            else
            {
                Debug.Log("No more cards left. Deck visuals should already be hidden.");
            }

            deckManager.DeactivateDeckIfEmpty();
        }
        else
        {
            // Çok uzakta: yok et
            Destroy(preview);
            preview = null;
        }
    }
}
