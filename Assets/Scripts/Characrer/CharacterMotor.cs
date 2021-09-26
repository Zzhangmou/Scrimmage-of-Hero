using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    /// <summary>
    /// ��ɫ�ƶ����
    /// </summary>
    public class CharacterMotor : MonoBehaviour
    {
        private CharacterController controller;
        [Tooltip("��ת�ٶ�")]
        public float rotateSpeed = 20f;
        [Tooltip("�ƶ��ٶ�")]
        public float moveSpeed = 3f;


        private void Start()
        {
            controller = GetComponent<CharacterController>();
        }
        //ע��Ŀ�귽����ת
        public void LookAtTarget(Vector3 dir)
        {
            if (dir == Vector3.zero) return;
            Quaternion lookDirection = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookDirection, rotateSpeed * Time.deltaTime);
        }
        //�ƶ�
        public void Movement(Vector3 dir)
        {
            LookAtTarget(dir);
            Vector3 forward = transform.forward;
            forward.y = -1;//����
            //��ǰ�ƶ�
            controller.Move(forward * Time.deltaTime * moveSpeed);
        }
    }
}