using DG.Tweening;
using TMPro;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance;
    public GameObject SoldierPrefab;
    public GameObject Grid;
    public Transform SoldierSpawnPoint;
    public TMP_Text HideShowText;
    private CardUIAnim currentSelectedCard;
    public Enums.CardTypes currentCardType;
    public GameObject[] Cards;
    private bool isCardHiden = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void CreateSoldier()
    {
        currentCardType = Enums.CardTypes.Soldier;
        GridPlacementSystem.Instance.SwitchGhost(currentCardType);
        if (CardResourceSpend.Instance.IsResourceSufficient())
        {
            CardResourceSpend.Instance.SpendResources();
            Instantiate(SoldierPrefab, SoldierSpawnPoint.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("Not enough resources to create soldier.");
        }
    }
    private void Reset()
    {
        GridPlacementSystem.Instance.DestroyCurrentGhost();
        currentCardType = Enums.CardTypes.None;
    }
    public void ResetCardSelection()
    {
        currentSelectedCard?.Deselect();
        currentSelectedCard = null;
        Reset();
    }
    public void SelectedUICard(CardUIAnim card)
    {
        if (currentSelectedCard == card)
        {
            card.Deselect();
            currentSelectedCard = null;
            Reset();
            return;
        }

        if (currentSelectedCard != null)
            currentSelectedCard.Deselect();

        currentSelectedCard = card;
        card.Select();
    }
    public void SelectWorkerCard()
    {
        currentCardType = Enums.CardTypes.Worker;
        GridPlacementSystem.Instance.SwitchGhost(currentCardType);
    }
    public void SelectMineCard()
    {
        currentCardType = Enums.CardTypes.Mine;
        GridPlacementSystem.Instance.SwitchGhost(currentCardType);
    }
    public void SelectBushCard()
    {
        currentCardType = Enums.CardTypes.Bush;
        GridPlacementSystem.Instance.SwitchGhost(currentCardType);
    }
    public void SelectTreeCard()
    {
        currentCardType = Enums.CardTypes.Tree;
        GridPlacementSystem.Instance.SwitchGhost(currentCardType);

    }
    public void SelectAnvilCard()
    {
        currentCardType = Enums.CardTypes.Anvil;
        GridPlacementSystem.Instance.SwitchGhost(currentCardType);

    }
    public void HideorShowCard()
    {
        if (isCardHiden)
        {
            foreach (GameObject card in Cards)
            {
                card.SetActive(true);
            }
            isCardHiden = false;
            Grid.SetActive(true);
            HideShowText.text = "v";
        }
        else
        {
            GridPlacementSystem.Instance.DestroyCurrentGhost();
            foreach (GameObject card in Cards)
            {
                card.SetActive(false);
            }
            currentSelectedCard?.Deselect();
            currentSelectedCard = null;
            isCardHiden = true;
            Grid.SetActive(false);
            HideShowText.text = "^";
        }
    }
}
