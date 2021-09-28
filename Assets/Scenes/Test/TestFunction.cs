using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

namespace ns
{
    /// <summary>
    /// ≤‚ ‘Ω≈±æ
    /// </summary>
    public class TestFunction : MonoBehaviour
    {
        public Transform GenerateTf;
        private void Start()
        {
            GameObject go = ResourcesManager.Load<GameObject>("Spaceman");
            CharacterInitConfigFactory.CreateCharacter(go, GenerateTf, 18, true);
        }
    }
}
