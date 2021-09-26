using NetWorkFK;
using proto;
using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// 技能同步管理器
    /// </summary>
    public class GeneratePrefabSyncManager : MonoSingleton<GeneratePrefabSyncManager>
    {
        public override void Init()
        {
            base.Init();
            NetManager.AddMsgListener("MsgGeneratePrefab", OnMsgGeneratePrefab);
        }

        private void OnMsgGeneratePrefab(IExtensible msgBase)
        {
            MsgGeneratePrefab msg = (MsgGeneratePrefab)msgBase;
            GameObject prefab = ResourcesManager.Load<GameObject>(msg.prefabName);
            Vector3 pos = new Vector3(msg.x, msg.y, msg.z);
            Quaternion rot = Quaternion.Euler(msg.eulerX, msg.eulerY, msg.eulerZ);
            GameObject go = GameObjectPool.Instance.CreateObject(msg.prefabName, prefab, pos, rot);
            GameObjectPool.Instance.CollectObject(go, msg.desTime);
        }
    }
}
