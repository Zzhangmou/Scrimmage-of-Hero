using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using proto;

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

        #region ͬ������
        //�ϴη���ͬ����Ϣʱ��
        private float lastSendSyncTime = 0;
        //ͬ��֡��
        private float syncInterval = 0.1f;
        //״̬����
        private string statusName;
        //״ֵ̬
        private bool status;
        #endregion

        private void Start()
        {
            controller = GetComponent<CharacterController>();
        }
        private void Update()
        {
            //ÿ�Զ���֡�ϴ�λ����Ϣ
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
        /// ����ͬ��ʱͬ������״̬
        /// </summary>
        /// <param name="statusName"></param>
        /// <param name="status"></param>
        public void SetAnimStatus(string statusName, bool status)
        {
            this.statusName = statusName;
            this.status = status;
        }
        #region �ƶ����
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
        #endregion
    }
}