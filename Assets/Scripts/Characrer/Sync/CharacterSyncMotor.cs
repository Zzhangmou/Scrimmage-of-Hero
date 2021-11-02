using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    /// <summary>
    /// ��ɫͬ�����
    /// </summary>
    public class CharacterSyncMotor : MonoBehaviour
    {
        private Animator anim;
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
        }
        private void Update()
        {
            SyncMovement();
        }

        public void SyncMovement()
        {

            float t = (Time.time - forecastTime) / syncInterval;
            t = Mathf.Clamp(t, 0f, 1f);
            //��ת
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRot), t);
            //λ��
            transform.position = Vector3.Lerp(transform.position, targetPos, t);
        }
    }
}