using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using proto;

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

        #region 同步数据
        //上次发送同步信息时间
        private float lastSendSyncTime = 0;
        //同步帧率
        private float syncInterval = 0.1f;
        //状态名称
        private string statusName;
        //状态值
        private bool status;
        #endregion

        private void Start()
        {
            controller = GetComponent<CharacterController>();
        }
        private void Update()
        {
            //每自定义帧上传位置信息
            SyncPosUpdate();
        }

        private void SyncPosUpdate()
        {
            if (Time.time - lastSendSyncTime < syncInterval) return;
            lastSendSyncTime = Time.time;
            MsgSyncPos msgSyncPos = new MsgSyncPos()
            {
                statusName = statusName,
                status = status,
                x = transform.position.x,
                y = transform.position.y,
                z = transform.position.z,
                eulerY = transform.rotation.eulerAngles.y
            };
            NetWorkFK.NetManager.Send(msgSyncPos);
        }

        /// <summary>
        /// 用于同步时同步动作状态
        /// </summary>
        /// <param name="statusName"></param>
        /// <param name="status"></param>
        public void SetAnimStatus(string statusName, bool status)
        {
            this.statusName = statusName;
            this.status = status;
        }
        #region 移动相关
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
        #endregion
    }
}