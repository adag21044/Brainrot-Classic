using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMovement : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rectTransform;      
    private Canvas canvas;
    private Vector2 originalLocalPointerPosition;
    private Vector3 originalPanelLocalPosition;
    private Vector3 originalScale;
    private int currentState = 0;
    private Quaternion originalRotation;
    private Vector3 originalPosition;

    [SerializeField] private float selectScale = 1.1f;
    [SerializeField] private Vector2 cardPlay;
    [SerializeField] private Vector3 playPoisition;
    [SerializeField] private GameObject glowEffect;
    [SerializeField] private GameObject playArrow;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        originalScale = rectTransform.localScale;
        originalPosition = rectTransform.localPosition;
        originalRotation = rectTransform.localRotation;
    }

    private void Update()
    {
        switch (currentState)
        {
            case 1:
                HandleHoverState();
                break;
            case 2:
                HandleDrageState();
                if (!Input.GetMouseButton(0))
                {
                    TransitionToState0();
                }
                break;
            case 3:
                HandlePlayState();
                if (!Input.GetMouseButton(0))
                {
                    TransitionToState0();
                }
                break;
        }
    }

    private void HandlePlayState()
    {
        rectTransform.localPosition = playPoisition; // Set position for play state
        rectTransform.localRotation = Quaternion.identity; // Reset rotation to zero

        if(Input.mousePosition.y < cardPlay.y)
        {
            currentState = 2; // Transition back to drag state
            playArrow.SetActive(false); // Disable play arrow
        }
    }

    private void HandleDrageState()
    {
        //Set the card rotation to zero
        rectTransform.localRotation = Quaternion.identity; // Reset rotation to zero
        
    }

    private void HandleHoverState()
    {
        glowEffect.SetActive(true); // Enable glow effect
        rectTransform.localScale = originalScale * selectScale; // Enlarge card
    }

    private void TransitionToState0()
    {
        currentState = 0;
        rectTransform.localScale = originalScale; // Reset scale
        rectTransform.localRotation = originalRotation; // Reset rotation
        rectTransform.localPosition = originalPosition; // Reset position
        glowEffect.SetActive(false); // Disable glow effect
        playArrow.SetActive(false); // Disable play arrow
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentState == 2)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.GetComponent<RectTransform>(),
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localPointerPosition)
                )
            {
                Vector3 offsetToOriginal = localPointerPosition - originalLocalPointerPosition;
                rectTransform.localPosition = originalPanelLocalPosition + offsetToOriginal;

                if (rectTransform.localPosition.y > cardPlay.y)
                {
                    currentState = 3; // Transition to play state
                    playArrow.SetActive(true); // Enable play arrow
                    rectTransform.localPosition = playPoisition; // Set position for play state
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (currentState == 1)
        {
            currentState = 2; // Transition to drag state
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.GetComponent<RectTransform>(),
                eventData.position,
                eventData.pressEventCamera,
                out originalLocalPointerPosition
            );
            originalPanelLocalPosition = rectTransform.localPosition; // Store original position for dragging
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentState == 0)
        {
            originalPosition = rectTransform.localPosition; // Store original position
            originalRotation = rectTransform.localRotation; // Store original rotation
            originalScale = rectTransform.localScale; // Store original scale

            currentState = 1;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentState == 1)
        {
            TransitionToState0();
        }
    }
}
