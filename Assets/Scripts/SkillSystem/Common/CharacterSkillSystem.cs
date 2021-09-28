using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            if (skill == null) return;
            //面向技能方向
            transform.LookAt(transform.position + deltaVac);
            skill.prefabPos = SwitchPosByAttackType(skill);
            //播放动画
            //生成技能
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
        public void UpdateElement(Vector3 JoyStickPos)
        {
            deltaVac = JoyStickPos;
            areaManager.UpdateElement(JoyStickPos);
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
