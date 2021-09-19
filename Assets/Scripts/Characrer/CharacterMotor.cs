using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using proto;

namespace ns
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

        //上一次发送同步信息的时间  
        private float lastSendSyncTime = 0;
        //同步帧率
        public static float syncInterval = 0.1f;
        public string statusName;
        public bool status;

        private void Start()
        {
            controller = GetComponent<CharacterController>();
        }
        private void Update()
        {
            if (Time.time - lastSendSyncTime < syncInterval) return;
            lastSendSyncTime = Time.time;
            MsgSyncPos msg = new MsgSyncPos()
            {
                x = transform.position.x,
                y = transform.position.y,
                z = transform.position.z,
                eulerY = transform.rotation.eulerAngles.y,
                statusName = this.statusName,
                status = this.status
            };
            NetWorkFK.NetManager.Send(msg);
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