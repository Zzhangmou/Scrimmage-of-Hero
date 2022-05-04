using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scrimmage
{
    /// <summary>
    ///ÎïÌåÐý×ª
    /// </summary>
    public class ObjTouchRotate : EventTrigger
    {
        public Transform showHeroTF;
        private Vector2 fightpos;
        private void Start()
        {
            showHeroTF = GameObject.Find("HeroShowTargetPosition").transform;
        }
        private void Update()
        {
            if (Input.touchCount <= 0) return;
            if (Input.touchCount == 1)
            {
                fightpos = Input.touches[0].deltaPosition;
                if (Input.touches[0].phase == TouchPhase.Moved)
                {
                    showHeroTF.Rotate(Vector3.up * -fightpos.x * Time.deltaTime);
                }
            }
        }
        public override void OnDrag(PointerEventData eventData)
        {
            float x = eventData.delta.x;
            showHeroTF.Rotate(Vector3.up * -x * Time.deltaTime * 10);
        }
    }
}
