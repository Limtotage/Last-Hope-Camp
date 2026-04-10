using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "Card")]
public class Card : ScriptableObject
{
    public Enums.CardTypes cardType;
    public int woodCost;
    public int foodCost;
    public int goldCost;
    public int swordCost;
}
