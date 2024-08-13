using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

/// <summary>
/// 按钮点击动效
/// </summary>
public class ButtonClickScale : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public void OnPointerUp(PointerEventData eventData)
    {
        transform.DOScale(1, 0.2f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.DOScale(1.1f, 0.2f);
    }
}