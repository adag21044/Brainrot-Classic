using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewCard", menuName = "Card")]
public class Card : ScriptableObject
{
    public string cardName;
    public List<CardType> cardType;
    public int health;
    public int damageMin;
    public int damageMax;
    public List<DamageType> damageType;
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
