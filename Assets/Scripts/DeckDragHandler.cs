using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DeckDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("References")]
    public DeckManager deckManager;
    public GameObject cardPrefab;           // HandManager.cardPrefab’ı kullanabilirsin
    public HandManager handManager;

    [Header("Deck View Settings")]
    public int initialDeckSize = 17;           // kaç kart göstereceğiz
    public Vector2 cardOffset = new Vector2(4f, -4f); // her kartın bir öncekine göre offset'i

    
    Canvas canvas;
    Card draggedCardData;
    GameObject preview;
    [SerializeField] private GameObject[] cardsToBeDeleted;
    [SerializeField] private int deleteIndex = 0;
    [SerializeField] private int maxDeleteCount = 26; // Max kart 
    [SerializeField] private GameObject DeckObject;

    public float snapDistance = 100f;

    void Awake() {
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

            if (maxDeleteCount > 0 && deleteIndex < cardsToBeDeleted.Length)
            {
                Destroy(cardsToBeDeleted[deleteIndex]);
                deleteIndex++;
            }
            if (maxDeleteCount == 0)
            {
                Destroy(DeckObject);
            }
        }
        else
        {
            // Çok uzakta: yok et
            Destroy(preview);
            preview = null;
        }
    }
}
