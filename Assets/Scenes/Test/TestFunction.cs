using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

namespace ns
{
    /// <summary>
    /// ���Խű�
    /// </summary>
    public class TestFunction : MonoBehaviour
    {
        public Transform GenerateTf;
        private void Start()
        {
            GameObject go = ResourcesManager.Load<GameObject>("Spaceman");
            //CharacterInitConfigFactory.CreateCharacter(go, GenerateTf, 18, true);
        }

        private void OnGUI()
        {
            if (GUILayout.Button("���ȫ��"))
            {
                GameObjectPool.Instance.ClearAll();
            }
        }
    }
}
