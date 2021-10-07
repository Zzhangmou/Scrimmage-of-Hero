using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scrimmage
{
    /// <summary>
    /// Ïà»ú¸úËæ
    /// </summary>
    public class CameraFollow : MonoBehaviour
    {
        public Transform targetPlayerTF;

        public Vector3 targetPos;
        public Vector3 offset;

        private void LateUpdate()
        {
            if (targetPlayerTF == null) return;

            targetPos = targetPlayerTF.position + offset;
            transform.position = targetPos;
            transform.LookAt(targetPlayerTF);
        }
    }
}
