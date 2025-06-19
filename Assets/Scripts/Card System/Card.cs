using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// -----------------------------------------------------------------------------
// Card ScriptableObject
// -----------------------------------------------------------------------------
// A data container that represents a single card definition in the project.
// Instances are created via the Unity editor (right‑click ▶ Create ▶ Card).
// This allows game designers to tweak card stats without touching code.
// -----------------------------------------------------------------------------
[CreateAssetMenu(fileName = "NewCard", menuName = "Card")]
public class Card : ScriptableObject
{
    public string cardName; // Display name shown on the card UI

    // A card can belong to multiple thematic categories ("Sigma", "Cringe", etc.)
    public List<CardType> cardType;
    public int health;
    public int damageMin;
    public int damageMax;
    public List<DamageType> damageType;
    public int cardID;

    // -------------------------------------------------------------------------
    //  Visual Assets
    // -------------------------------------------------------------------------
    public Sprite cardSprite;
    public Sprite cardBackSprite;
    public Sprite symbolSprite;
    

    public enum CardType
    {
        Sigma,
        Cringe,
        Brainrot,
        NPC,
        Wholesome
    }

    public enum DamageType
    {
        Fire,
        Earth,
        Water,
        Dark,
        Light,
        Air
    }

}
