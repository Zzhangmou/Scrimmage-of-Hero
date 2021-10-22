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
    /// ��װ����ϵͳ,�ṩ�򵥵ļ����ͷŹ���
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
        /// ʹ�ü���
        /// </summary>
        /// <param name="skillId">����id</param>
        public void AttackUseSkill(int skillId)
        {
            //׼������
            SkillData skill = skillManager.PrepareSkill(skillId);
            print(skill);
            if (skill == null) return;
            //�����ܷ���
            transform.LookAt(transform.position + deltaVac);
            skill.prefabPos = SwitchPosByAttackType(skill);
            //���Ŷ���
            //���ɼ���
            skillManager.GenerateSkill(skill);
            //����Э��
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
        #region Э�����
        /// <summary>
        /// ����Э��
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
        /// ��ü�������
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SkillData GetSkillData(int id)
        {
            return skillManager.skills.Find(s => s.skillId == id);
        }
        /// <summary>
        /// ���ü�������
        /// </summary>
        /// <param name="data"></param>
        public void SetSkillData(SkillData[] data)
        {
            skillManager.skills = data;
        }

        #region �ṩ���ü�������������ķ���
        /// <summary>
        /// ��������չʾ����
        /// </summary>
        public void CreateSkillShowArea(SkillData data)
        {
            areaManager.CreateSkillShowArea(data);
        }
        /// <summary>
        /// ���¼���չʾ����
        /// </summary>
        /// <param name="deltaVac">ҡ��λ��</param>
        public void UpdateElement(Vector3 JoyStickPos, bool reverse)
        {
            areaManager.UpdateElement(JoyStickPos);
            if (reverse)
                JoyStickPos = -JoyStickPos;
            deltaVac = JoyStickPos;
        }
        /// <summary>
        /// ���ؼ���������ʾ
        /// </summary>
        public void HideElement()
        {
            areaManager.HideSkillShowArea();
        }
        #endregion
    }
}
