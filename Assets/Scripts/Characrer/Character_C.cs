using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

namespace ns
{
    /// <summary>
    /// ½ÇÉ«¿ØÖÆÀà
    /// </summary>
    public class Character_C : MonoBehaviour
    {
        public CharacterInfo characterInfo;
        private CharacterController controller;
        public float rotateSpeed = 45;

        private void Start()
        {
            controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                Movement(new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")));
            }
        }

        private void Movement(Vector3 dir)
        {
            Vector3 forward = transform.forward;
            LookAtTarget(dir);
            controller.Move(forward * characterInfo.speed * Time.deltaTime);
        }
        private void LookAtTarget(Vector3 dir)
        {
            if (dir == Vector3.zero) return;
            Quaternion target = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, target, rotateSpeed * Time.deltaTime);
        }
    }
}
