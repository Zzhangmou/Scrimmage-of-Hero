using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    /// <summary>
    /// 角色同步马达
    /// </summary>
    public class CharacterSyncMotor : MonoBehaviour
    {
        private Animator anim;
        //同步帧率
        public static float syncInterval = 0.1f;
        public float forecastTime = 0f;
        [Tooltip("旋转速度")]
        public float rotateSpeed = 20f;
        [Tooltip("移动速度")]
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
            //旋转
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRot), t);
            //位置
            transform.position = Vector3.Lerp(transform.position, targetPos, t);
        }
    }
}