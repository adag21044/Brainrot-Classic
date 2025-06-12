using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Manages all pointer‑based interactions for a single card:
/// * Hover  – enlarges the card and adds a glow.
/// * Drag   – follows the mouse cursor inside the hand area.
/// * Play   – snaps to a fixed play position once moved above a Y threshold.
/// 
/// States
/// -------
/// 0 = Idle   – default resting state.
/// 1 = Hover  – pointer is over the card (no mouse button).
/// 2 = Drag   – left button held, card is being dragged.
/// 3 = Play   – card has crossed <see cref="cardPlay"/> Y and is ready to be played.
/// 
/// Attach this script to the card prefab.  Requires a Canvas parent so that
/// RectTransformUtility can translate screen‑to‑local coordinates correctly.
/// </summary>

public class CardMovement : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rectTransform; // UI transform of the card itself
    private Canvas canvas;  // Reference to the parent canvas (for coordinate conversion)
    private Vector2 originalLocalPointerPosition; // Reference to the parent canvas (for coordinate conversion)
    private Vector3 originalPanelLocalPosition;  // Card position when drag starts

    // ---------------------------------------------------------------------
    //  Restore Data – used when returning the card to its idle state
    // ---------------------------------------------------------------------
    private Vector3 originalScale;
    private int currentState = 0;
    private Quaternion originalRotation;
    private Vector3 originalPosition;

    [SerializeField] private float selectScale = 1.1f; // Scale multiplier while hovering
    [SerializeField] private Vector2 cardPlay;
    [SerializeField] private Vector3 playPoisition;
    [SerializeField] private GameObject glowEffect;
    [SerializeField] private GameObject playArrow;

    private void Awake()
    {
        // Cache expensive component lookups for performance
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        // Preserve original transform values for later reset
        originalScale = rectTransform.localScale;
        originalPosition = rectTransform.localPosition;
        originalRotation = rectTransform.localRotation;
    }

    private void Update()
    {
        // Dispatch per‑state behaviour each frame
        switch (currentState)
        {
            case 1:
                HandleHoverState();
                break;
            case 2:
                HandleDrageState();
                // If the mouse button is released during drag, return to idle
                if (!Input.GetMouseButton(0))
                {
                    TransitionToState0();
                }
                break;
            case 3:
                HandlePlayState();
                // Re‑check mouse button release to allow cancelling play
                if (!Input.GetMouseButton(0))
                {
                    TransitionToState0();
                }
                break;
        }
    }

    private void HandlePlayState()
    {
        // Snap to the dedicated play position and reset any rotation
        rectTransform.localPosition = playPoisition; // Set position for play state
        rectTransform.localRotation = Quaternion.identity; // Reset rotation to zero

        // If the card drops below the threshold, revert to drag state
        if (Input.mousePosition.y < cardPlay.y)
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
            // Convert the current screen position of the pointer to the canvas' local space
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.GetComponent<RectTransform>(),
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localPointerPosition)
                )
            {
                // Compute how far we have moved relative to the initial click point
                Vector3 offsetToOriginal = localPointerPosition - originalLocalPointerPosition;
                rectTransform.localPosition = originalPanelLocalPosition + offsetToOriginal;

                // If dragged above the threshold, switch to play state
                if (rectTransform.localPosition.y > cardPlay.y)
                {
                    currentState = 3; // Transition to play state
                    playArrow.SetActive(true); // Enable play arrow
                    rectTransform.localPosition = playPoisition; // Set position for play state
                }
            }
        }
    }

    /// <summary>
    /// Called once when the user presses the left mouse button on the card.
    /// Initiates drag state if we were hovering.
    /// </summary>
    public void OnPointerDown(PointerEventData eventData)
    {
        if (currentState == 1) // Only if previously hovering
        {
            currentState = 2; // Transition to drag state

            // Record starting positions for smooth drag maths
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.GetComponent<RectTransform>(),
                eventData.position,
                eventData.pressEventCamera,
                out originalLocalPointerPosition
            );
            originalPanelLocalPosition = rectTransform.localPosition; // Store original position for dragging
        }
    }

    /// <summary>
    /// Called when the pointer first hovers over the card.
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentState == 0)
        {
            originalPosition = rectTransform.localPosition; // Store original position
            originalRotation = rectTransform.localRotation; // Store original rotation
            originalScale = rectTransform.localScale; // Store original scale

            currentState = 1; // Switch to hover state
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentState == 1)
        {
            TransitionToState0(); // Revert to idle
        }
    }
}
