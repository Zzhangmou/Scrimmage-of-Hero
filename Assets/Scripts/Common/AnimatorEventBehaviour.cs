using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Ϊ����Ƭ������¼�,ָ��OnAttack
 */
namespace Common
{
    /// <summary>
    /// �����¼���Ϊ��
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
        /// ��Unity�������
        /// </summary>
        private void OnAttack()
        {
            if (AttackHandler != null)
            {
                //�����¼�
                AttackHandler();
            }
        }
        /// <summary>
        /// ��Unity�������
        /// </summary>
        /// <param name="animParam"></param>
        private void OnCancelAnim(string animParam)
        {
            anim.SetBool(animParam, false);
        }
        /// <summary>
        /// ��Unity����
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
