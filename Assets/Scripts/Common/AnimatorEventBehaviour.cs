using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 为动画片段添加事件,指向OnAttack
 */
namespace Common
{
    /// <summary>
    /// 动画事件行为类
    /// </summary>
    public class AnimatorEventBehaviour : MonoBehaviour
    {
        public event Action AttackHandler;

        private Animator anim;
        private void Start()
        {
            anim = GetComponent<Animator>();
        }
        /// <summary>
        /// 由Unity引擎调用
        /// </summary>
        private void OnAttack()
        {
            if (AttackHandler != null)
            {
                //引发事件
                AttackHandler();
            }
        }
        /// <summary>
        /// 由Unity引擎调用
        /// </summary>
        /// <param name="animParam"></param>
        private void OnCancelAnim(string animParam)
        {
            anim.SetBool(animParam, false);
        }
        /// <summary>
        /// 由Unity调用
        /// </summary>
        /// <param name="animParam"></param>
        private void OnCancelAnimWithDelay(string animParam)
        {
            int delay = 2;
            StartCoroutine(CancelAnim(animParam, delay));
        }

        private IEnumerator CancelAnim(string animParam, int delay)
        {
            yield return new WaitForSeconds(delay);
            anim.SetBool(animParam, false);
        }
    }
}
