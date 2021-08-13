using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scrimmage.Skill
{
    [System.Serializable]
    /// <summary>
    /// 技能数据
    /// </summary>
    public class SkillData
    {
        /// <summary>
        /// 技能Id
        /// </summary>
        public int skillId;
        /// <summary>
        /// 技能名称
        /// </summary>
        public string name;
        /// <summary>
        /// 技能描述
        /// </summary>
        public string description;
        /// <summary>
        /// 冷却时间
        /// </summary>
        public int coolTime;
        /// <summary>
        /// 冷却剩余
        /// </summary>
        public int coolRemain;
        /// <summary>
        /// 技能生成位置
        /// </summary>
        [HideInInspector]
        public Transform prefabTF;
        /// <summary>
        /// 攻击位置
        /// </summary>
        [HideInInspector]
        public Transform attackPos;
        /// <summary>
        /// 攻击距离
        /// </summary>
        public float attackDistance;
        /// <summary>
        /// 攻击角度
        /// </summary>
        public float attackAngle;
        /// <summary>
        /// 技能攻击范围
        /// </summary>
        public float attackWide = 2f;
        /// <summary>
        /// 攻击目标Tag
        /// </summary>
        public string[] attackTargetTags;
        /// <summary>
        /// 攻击目标对象数组
        /// </summary>
        [HideInInspector]
        public Transform[] attackTargets;
        /// <summary>
        /// 技能影响类型
        /// </summary>
        public string[] impactType;
        /// <summary>
        /// 伤害比率
        /// </summary>
        public float atkRadio;
        /// <summary>
        /// 持续时间
        /// </summary>
        public float durationTime;
        /// <summary>
        /// 伤害间隔
        /// </summary>
        public float atkInterval;
        /// <summary>
        /// 技能所属
        /// </summary>
        [HideInInspector]
        public GameObject owner;
        /// <summary>
        /// 技能预制件名称
        /// </summary>
        public string prefabName;
        /// <summary>
        /// 预制件对象
        /// </summary>
        [HideInInspector]
        public GameObject skillPrefab;
        /// <summary>
        /// 动画名称
        /// </summary>
        public string animationName;
        /// <summary>
        /// 受击特效名称
        /// </summary>
        public string hitFxName;
        /// <summary>
        /// 受击特效预制件
        /// </summary>
        [HideInInspector]
        public GameObject hitFxPrefab;
        /// <summary>
        /// 攻击类型
        /// </summary>
        public SkillAttackType attackType;
        /// <summary>
        /// 选择类型
        /// </summary>
        public SelectorType selectorType;
        /// <summary>
        /// 技能指示类型
        /// </summary>
        public SkillAreaType areaType;
    }
}