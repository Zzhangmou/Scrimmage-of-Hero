using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using proto;

namespace Scrimmage.Skill
{
    [RequireComponent(typeof(CharacterSkillManager))]
    [RequireComponent(typeof(CharacterShowSkillAreaManager))]
    /// <summary>
    /// 封装技能系统,提供简单的技能释放功能
    /// </summary>
    public class CharacterSkillSystem : MonoBehaviour
    {
        private CharacterSkillManager skillManager;
        private CharacterShowSkillAreaManager areaManager;

        private Vector3 deltaVac;
        private void Awake()
        {
            skillManager = GetComponent<CharacterSkillManager>();
            areaManager = GetComponent<CharacterShowSkillAreaManager>();
        }
        /// <summary>
        /// 使用技能
        /// </summary>
        /// <param name="skillId">技能id</param>
        public void AttackUseSkill(int skillId)
        {
            //准备技能
            SkillData skill = skillManager.PrepareSkill(skillId);
            print(skill);
            if (skill == null) return;
            //面向技能方向
            transform.LookAt(transform.position + deltaVac);
            skill.prefabPos = SwitchPosByAttackType(skill);
            //播放动画
            //生成技能
            skillManager.GenerateSkill(skill);
            //发送协议
            SendMsgByType(skill);
        }
        private Vector3 SwitchPosByAttackType(SkillData data)
        {
            switch (data.generateType)
            {
                case SkillGenerateType.Inplace:
                    return this.transform.position;
                case SkillGenerateType.Select:
                    return transform.position + deltaVac * data.attackScope / 2;
                default:
                    return data.attackPos.position;
            }
        }
        #region 协议相关
        /// <summary>
        /// 发送协议
        /// </summary>
        /// <param name="data"></param>
        private void SendMsgByType(SkillData data)
        {
            if (data.generateType == SkillGenerateType.FileAndDIs)
            {
                SendMsgGeneratePrefabWDis(data);
            }
            else
                SendMsgGeneratePrefab(data);
        }
        private void SendMsgGeneratePrefab(SkillData data)
        {
            MsgGeneratePrefab msg = new MsgGeneratePrefab();
            msg.prefabName = data.prefabName;
            msg.x = data.prefabPos.x;
            msg.y = data.prefabPos.y;
            msg.z = data.prefabPos.z;
            msg.eulerX = transform.rotation.eulerAngles.x;
            msg.eulerY = transform.rotation.eulerAngles.y;
            msg.eulerZ = transform.rotation.eulerAngles.z;
            msg.durTime = data.durationTime;
            msg.isFllowTarget = data.generateType == SkillGenerateType.FileAndFllow ? true : false;
            NetWorkFK.NetManager.Send(msg);
        }

        private void SendMsgGeneratePrefabWDis(SkillData data)
        {
            MsgGeneratePrefabWDis msg = new MsgGeneratePrefabWDis();
            msg.prefabName = data.prefabName;
            msg.x = data.prefabPos.x;
            msg.y = data.prefabPos.y;
            msg.z = data.prefabPos.z;
            msg.eulerX = transform.rotation.eulerAngles.x;
            msg.eulerY = transform.rotation.eulerAngles.y;
            msg.eulerZ = transform.rotation.eulerAngles.z;
            msg.moveSpeed = data.bulletSpeed;
            msg.targetX = data.attackPos.TransformPoint(0, 0, data.attackDistance).x;
            msg.targetY = data.attackPos.TransformPoint(0, 0, data.attackDistance).y;
            msg.targetZ = data.attackPos.TransformPoint(0, 0, data.attackDistance).z;
            msg.durTime = data.durationTime;
            NetWorkFK.NetManager.Send(msg);
        }
        #endregion
        /// <summary>
        /// 获得技能数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SkillData GetSkillData(int id)
        {
            return skillManager.skills.Find(s => s.skillId == id);
        }
        /// <summary>
        /// 设置技能数据
        /// </summary>
        /// <param name="data"></param>
        public void SetSkillData(SkillData[] data)
        {
            skillManager.skills = data;
        }

        #region 提供调用技能区域管理器的方法
        /// <summary>
        /// 创建技能展示区域
        /// </summary>
        public void CreateSkillShowArea(SkillData data)
        {
            areaManager.CreateSkillShowArea(data);
        }
        /// <summary>
        /// 更新技能展示区域
        /// </summary>
        /// <param name="deltaVac">摇杆位置</param>
        public void UpdateElement(Vector3 JoyStickPos, bool reverse)
        {
            areaManager.UpdateElement(JoyStickPos);
            if (reverse)
                JoyStickPos = -JoyStickPos;
            deltaVac = JoyStickPos;
        }
        /// <summary>
        /// 隐藏技能区域显示
        /// </summary>
        public void HideElement()
        {
            areaManager.HideSkillShowArea();
        }
        #endregion
    }
}
