using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Common
{
    /// <summary>
    /// 保护物体跨场景不被销毁
    /// </summary>
    public class DontDestory : MonoBehaviour
    {
        private void Awake()
        {
            if (GameObject.Find(this.gameObject.name).gameObject != this.gameObject)
                Destroy(this.gameObject);
        }
        private void Start()
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
