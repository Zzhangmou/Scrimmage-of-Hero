using UnityEngine;

public class ResManager : MonoBehaviour
{

    //加载预设
    public static GameObject LoadPrefab(string path)
    {
        return Resources.Load<GameObject>(path);
    }
    public static Object Load(string path)
    {
        return Resources.Load(path);
    }
}
