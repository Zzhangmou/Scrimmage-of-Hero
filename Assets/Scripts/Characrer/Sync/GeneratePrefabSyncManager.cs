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
            NetManager.AddMsgListener("MsgGeneratePrefabWDis", OnMsgGeneratePrefabWDis);
        }

        private void OnMsgGeneratePrefabWDis(IExtensible msgBase)
        {
            MsgGeneratePrefabWDis msg = (MsgGeneratePrefabWDis)msgBase;
            GameObject prefab = ResourcesManager.Load<GameObject>(msg.prefabName);
            Vector3 pos = new Vector3(msg.x, msg.y, msg.z);
            Quaternion rot = Quaternion.Euler(msg.eulerX, msg.eulerY, msg.eulerZ);
            GameObject go = GameObjectPool.Instance.CreateObject(msg.prefabName, prefab, pos, rot);
            Vector3 targetPos = new Vector3(msg.targetX, msg.targetY, msg.targetZ);
            Scrimmage.Skill.BulletSkillDeployer bullet = go.GetComponent<Scrimmage.Skill.BulletSkillDeployer>();
            bullet.targetTf = targetPos;
            bullet.speed = msg.moveSpeed;
            bullet.camp = msg.camp;
            //GameObjectPool.Instance.CollectObject(go, msg.durTime);
        }

        private void OnMsgGeneratePrefab(IExtensible msgBase)
        {
            MsgGeneratePrefab msg = (MsgGeneratePrefab)msgBase;
            GameObject prefab = ResourcesManager.Load<GameObject>(msg.prefabName);
            Vector3 pos = new Vector3(msg.x, msg.y, msg.z);
            Quaternion rot = Quaternion.Euler(msg.eulerX, msg.eulerY, msg.eulerZ);
            GameObject go = GameObjectPool.Instance.CreateObject(msg.prefabName, prefab, pos, rot);

            GameObjectPool.Instance.CollectObject(go, msg.durTime);
            if (msg.isFllowTarget)
            {
                GameObject fatherGo = GetComponent<GameManager>().GetHero(msg.targetId);
                fatherGo.transform.rotation = rot;
                //问题 旋转问题 设置父物体时 角色对象还未旋转到释放角色的位置 导致技能效果与人物反方向
                go.transform.SetParent(fatherGo.transform);
            }
        }
    }
}