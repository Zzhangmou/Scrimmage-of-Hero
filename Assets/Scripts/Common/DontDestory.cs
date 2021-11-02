using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Common
{
    /// <summary>
    /// ��������糡����������
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
