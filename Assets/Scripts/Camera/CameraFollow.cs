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
        [SerializeField]
        private Transform targetPlayerTF;
        [SerializeField]
        private Vector3 offset;
        [SerializeField]
        private float cameraPosZ;

        private void LateUpdate()
        {
            if (targetPlayerTF == null) return;

            cameraPosZ = targetPlayerTF.position.z + offset.z;
            transform.position = new Vector3(transform.position.x, transform.position.y, cameraPosZ);
        }

        public void CameraInit(Transform targetTf, Vector3 withOffset)
        {
            targetPlayerTF = targetTf;
            offset = withOffset;

            transform.position = targetPlayerTF.position + offset;
            transform.LookAt(targetPlayerTF);

            Vector3 mapCenterVec = Common.GameManager.Instance.gameDatas["Map"].transform.position;
            transform.position = new Vector3(mapCenterVec.x, transform.position.y, transform.position.z);
        }
    }
}
