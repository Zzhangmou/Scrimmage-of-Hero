using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XLua;

[LuaCallCSharp]
public class RecyclingList : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform content;
    public GridLayoutGroup layoutGroup;

    protected virtual void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
        content = scrollRect.content;
        layoutGroup = content.GetComponent<GridLayoutGroup>();

        layoutGroup.enabled = false;
    }
}