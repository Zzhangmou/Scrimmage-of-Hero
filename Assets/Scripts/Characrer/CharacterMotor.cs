using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    /// <summary>
    /// 角色移动马达
    /// </summary>
    public class CharacterMotor : MonoBehaviour
    {
        private CharacterController controller;
        [Tooltip("旋转速度")]
        public float rotateSpeed = 20f;
        [Tooltip("移动速度")]
        public float moveSpeed = 3f;


        private void Start()
        {
            controller = GetComponent<CharacterController>();
        }
        //注视目标方向旋转
        public void LookAtTarget(Vector3 dir)
        {
            if (dir == Vector3.zero) return;
            Quaternion lookDirection = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookDirection, rotateSpeed * Time.deltaTime);
        }
        //移动
        public void Movement(Vector3 dir)
        {
            LookAtTarget(dir);
            Vector3 forward = transform.forward;
            forward.y = -1;//重力
            //向前移动
            controller.Move(forward * Time.deltaTime * moveSpeed);
        }
    }
}