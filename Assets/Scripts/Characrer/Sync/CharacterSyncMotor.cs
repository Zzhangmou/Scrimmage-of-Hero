using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns
{
    /// <summary>
    /// ��ɫͬ�����
    /// </summary>
    public class CharacterSyncMotor : MonoBehaviour
    {
        private Animator anim;
        private CharacterStatus characterStatus;
        public string statusName;
        public bool status;
        //ͬ��֡��
        public static float syncInterval = 0.1f;
        public float forecastTime = 0f;
        [Tooltip("��ת�ٶ�")]
        public float rotateSpeed = 20f;
        [Tooltip("�ƶ��ٶ�")]
        public float moveSpeed = 3f;

        public Vector3 targetPos;
        public Vector3 targetRot;


        private void Start()
        {
            anim = GetComponent<Animator>();
            characterStatus = GetComponent<CharacterStatus>();
        }
        private void Update()
        {
            SyncMovement();
        }

        public void SyncMovement()
        {
            //���ö���
            anim.SetBool(statusName, status);
            //��ת
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRot), rotateSpeed * Time.deltaTime);
            float t = (Time.time - forecastTime) / syncInterval;
            t = Mathf.Clamp(t, 0f, 1f);
            //λ��
            transform.position = Vector3.Lerp(transform.position, targetPos, t);
        }
    }
}