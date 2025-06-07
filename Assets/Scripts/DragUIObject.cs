using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Draggable UI card that follows the mouse precisely
/// and can be dropped onto other UI elements.
/// </summary>
public class DraggableCard : MonoBehaviour,IPointerDownHandler,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    private RectTransform rectTransform;      // Cached RectTransform
    private RectTransform canvasRect;         // Parent canvas's RectTransform
    private Vector2 pointerOffsetCanvas;      // Offset in canvas space
    private Vector2 targetPosition;           // For optional smoothing

    [Header("Optional smoothing")]
    public bool smoothMove = false;
    public float moveSpeed  = 20f;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasRect    = GetComponentInParent<Canvas>().transform as RectTransform;
        targetPosition = rectTransform.localPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Mouse position in canvas local space
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPointerPos);

        // Difference between card position and mouse, both in canvas space
        pointerOffsetCanvas = rectTransform.localPosition - (Vector3)localPointerPos;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetAsLastSibling();          // Bring card to front
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localPointerPos))
        {
            Vector2 newPos = localPointerPos + pointerOffsetCanvas;

            if (smoothMove)
                targetPosition = newPos;
            else
                rectTransform.localPosition = newPos;
        }
    }

    public void OnEndDrag(PointerEventData eventData) { /* noop */ }

    private void Update()
    {
        if (smoothMove)
        {
            rectTransform.localPosition = Vector3.Lerp(
                rectTransform.localPosition,
                targetPosition,
                Time.deltaTime * moveSpeed);
        }
    } 
}
