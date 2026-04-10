using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
public class CardUIAnim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rect;

    [Header("Positions")]
    public float hoverY = 50f;   // yarım çıkma
    public float selectedY = 100f; // tam çıkma
    public float duration = 0.25f;
    public Enums.CardTypes cardType;

    private Vector2 startPos;
    private bool isSelected = false;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        startPos = rect.anchoredPosition;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isSelected) return;

        rect.DOAnchorPosY(startPos.y + hoverY, duration)
            .SetEase(Ease.OutQuad);
        transform.DOScale(1.2f, duration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isSelected) return;

        rect.DOAnchorPosY(startPos.y, duration)
            .SetEase(Ease.OutQuad);
        transform.DOScale(1f, duration);
    }
    public void Clicked()
    {
        CardManager.Instance.SelectedUICard(this);
    }

    public void Select()
    {
        isSelected = true;

        rect.DOAnchorPosY(startPos.y + selectedY, duration)
            .SetEase(Ease.OutBack);

        transform.DOScale(1.2f, duration);
    }

    public void Deselect()
    {
        isSelected = false;

        rect.DOAnchorPosY(startPos.y, duration)
            .SetEase(Ease.OutQuad);

        transform.DOScale(1f, duration);
    }
}
