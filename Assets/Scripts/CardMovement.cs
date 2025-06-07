using UnityEngine;
using UnityEngine.EventSystems;

public class CardMovement : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler,
    IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rectTransform;
    private RectTransform canvasRect;
    private Canvas canvas;

    // Fare ile kart arasındaki offset
    private Vector2 pointerOffset;

    // Orijinaller
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 originalScale;

    // KartPlay eşiği (Inspector'dan ayarla)
    [SerializeField] private float playThresholdY;
    [SerializeField] private Vector3 playPosition;
    [SerializeField] private GameObject glowEffect;
    [SerializeField] private GameObject playArrow;

    // 0 = idle, 1 = hover, 2 = dragging, 3 = played
    private int currentState = 0;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasRect = canvas.GetComponent<RectTransform>();

        originalPosition = rectTransform.localPosition;
        originalRotation = rectTransform.localRotation;
        originalScale = rectTransform.localScale;
    }

    private void Update()
    {
        switch (currentState)
        {
            case 1: // hover
                glowEffect.SetActive(true);
                rectTransform.localScale = originalScale * 1.1f;
                break;

            case 3: // played
                rectTransform.localPosition = playPosition;
                rectTransform.localRotation = Quaternion.identity;
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentState == 0)
            currentState = 1;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentState == 1)
            ResetCard();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentState == 1)
        {
            currentState = 2;
            glowEffect.SetActive(false);

            // Ekran noktasını localPoint'e çeviriyoruz
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localPoint);

            // localPosition'ı Vector2'ye indirgemek için:
            Vector2 panelLocalPos = new Vector2(
                rectTransform.localPosition.x,
                rectTransform.localPosition.y);

            // Artık Vector2 - Vector2 çıkarabiliyoruz:
            pointerOffset = localPoint - panelLocalPos;

            transform.SetAsLastSibling();
        }
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (currentState == 2)
        {
            // Yeni local pozisyon = fare konumu – offset
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localPoint))
            {
                rectTransform.localPosition = localPoint - pointerOffset;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (currentState == 2)
        {
            // Sürükleme bitti, eşiğin üstündeyse Play state, değilse reset
            if (rectTransform.localPosition.y > playThresholdY)
            {
                currentState = 3;
                playArrow.SetActive(true);
            }
            else
            {
                ResetCard();
            }
        }
    }

    private void ResetCard()
    {
        currentState = 0;
        rectTransform.localPosition = originalPosition;
        rectTransform.localRotation = originalRotation;
        rectTransform.localScale = originalScale;
        glowEffect.SetActive(false);
        playArrow.SetActive(false);
    }
}
