using UnityEngine.UI;

/// <summary>
/// 点击不规则的UI
/// </summary>
public class CustomImage : Image
{
    protected override void Awake()
    {
        base.Awake();
        //alpha 阈值指定像素必须具有的最小 alpha 值，事件才能被视为对图像的“命中”。
        alphaHitTestMinimumThreshold = 0.1f;
    }
}
