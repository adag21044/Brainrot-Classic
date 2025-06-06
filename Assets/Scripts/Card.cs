using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "Card")]
public class Card : ScriptableObject
{
    public string cardName;
    public List<CardType> cardType;
    public int health;
    public int damageMin;
    public int damageMax;
    public List<DamageType> damageType;

    public enum CardType
    {
        Fire,
        Earth,
        Water,
        Dark,
        Light,
        Air
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
