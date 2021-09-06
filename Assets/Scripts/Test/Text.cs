using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ns
{
    /// <summary>
    /// 
    /// </summary>
    public class Text : MonoBehaviour
    {
        public UnityEngine.UI.Text text;
        public string str;
        private void Update()
        {
            text.text = str;
        }
    }
}
