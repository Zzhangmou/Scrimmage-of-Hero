using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            if (skill == null) return;
            //�����ܷ���
            transform.LookAt(transform.position + deltaVac);
            skill.prefabPos = SwitchPosByAttackType(skill);
            //���Ŷ���
            //���ɼ���
            skillManager.GenerateSkill(skill);
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
        public void UpdateElement(Vector3 JoyStickPos)
        {
            deltaVac = JoyStickPos;
            areaManager.UpdateElement(JoyStickPos);
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
