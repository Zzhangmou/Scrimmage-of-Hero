using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace ns
{
    /// <summary>
    /// 
    /// </summary>
    public class NewBehaviourScript : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<RectTransform>().DOAnchorPosX(200, 1);
            print(1111);
        }
    }
}
