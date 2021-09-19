using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using proto;

namespace ns
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

        //��һ�η���ͬ����Ϣ��ʱ��  
        private float lastSendSyncTime = 0;
        //ͬ��֡��
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