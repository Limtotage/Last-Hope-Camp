using UnityEngine;

public class CardResourceSpend : MonoBehaviour
{
    public static CardResourceSpend Instance;
    public Card cardmine;
    public Card cardbush;
    public Card cardtree;
    public Card cardanvil;
    public Card cardworker;
    public Card cardsoldier;
    private Card selectedCard;
    void Awake()
    {
        if(Instance == null)
        {
            Instance=this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SelectCard()
    {
        switch (GridPlacementSystem.Instance.GetCurrentCardType())
        {
            case Enums.CardTypes.Mine:
                selectedCard = cardmine;
                break;
            case Enums.CardTypes.Bush:
                selectedCard = cardbush;
                break;
            case Enums.CardTypes.Tree:
                selectedCard = cardtree;
                break;
            case Enums.CardTypes.Anvil:
                selectedCard = cardanvil;
                break;
            case Enums.CardTypes.Worker:
                selectedCard = cardworker;
                break;
            case Enums.CardTypes.Soldier:
                selectedCard = cardsoldier;
                break;
        }
    }
    public bool IsResourceSufficient()
    {
        SelectCard();
        if(selectedCard == null)
        {
            Debug.Log("No card selected.");
            return false;
        }
        if(ResourceManager.Instance.GoldAmount < selectedCard.goldCost ||
           ResourceManager.Instance.FoodAmount < selectedCard.foodCost ||
           ResourceManager.Instance.WoodAmount < selectedCard.woodCost ||
           ResourceManager.Instance.SwordAmount < selectedCard.swordCost)
        {
            return false;
        }
        return true;
    }
    public void SpendResources()
    {
        ResourceManager.Instance.SpendGold(selectedCard.goldCost);
        ResourceManager.Instance.SpendFood(selectedCard.foodCost);
        ResourceManager.Instance.SpendWood(selectedCard.woodCost);
        ResourceManager.Instance.SpendSword(selectedCard.swordCost);
    }
}
