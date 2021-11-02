using System;
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
        [Header("技能Id")]
        /// <summary>
        /// 技能Id
        /// </summary>
        public int skillId;
        [Header("技能名称")]
        /// <summary>
        /// 技能名称
        /// </summary>
        public string skillName;
        [Header("技能描述")]
        /// <summary>
        /// 技能描述
        /// </summary>
        public string description;
        [Header("冷却时间")]
        /// <summary>
        /// 冷却时间
        /// </summary>
        public int coolTime;
        [Header("冷却剩余")]
        /// <summary>
        /// 冷却剩余
        /// </summary>
        [HideInInspector]
        public int coolRemain;
        /// <summary>
        /// 技能生成位置
        /// </summary>
        [HideInInspector]
        public Vector3 prefabPos;
        /// <summary>
        /// 攻击位置
        /// </summary>
        [HideInInspector]
        public Transform attackPos;
        [Header("攻击范围")]
        /// <summary>
        /// 攻击范围
        /// </summary>
        public float attackScope;
        [Header("攻击距离")]
        /// <summary>
        /// 攻击距离
        /// </summary>
        public float attackDistance;
        [Header("攻击角度")]
        /// <summary>
        /// 攻击角度
        /// </summary>
        public float attackAngle;
        [Header("子弹类型技能设置速度")]
        /// <summary>
        /// 子弹飞行速度
        /// </summary>
        public float bulletSpeed;
        [Header("攻击目标Tag")]
        /// <summary>
        /// 攻击目标Tag
        /// </summary>
        public string[] attackTargetTags;
        /// <summary>
        /// 攻击目标对象数组
        /// </summary>
        [HideInInspector]
        public Transform[] attackTargets;
        [Header("技能影响类型")]
        /// <summary>
        /// 技能影响类型
        /// </summary>
        public string[] impactType;
        [Header("伤害比率")]
        /// <summary>
        /// 伤害比率
        /// </summary>
        public float atkRadio;
        [Header("持续时间")]
        /// <summary>
        /// 持续时间
        /// </summary>
        public float durationTime;
        [Header("伤害间隔")]
        /// <summary>
        /// 伤害间隔
        /// </summary>
        public float atkInterval;
        /// <summary>
        /// 技能所属
        /// </summary>
        [HideInInspector]
        public GameObject owner;
        [Header("技能预制件名称")]
        /// <summary>
        /// 技能预制件名称
        /// </summary>
        public string prefabName;
        /// <summary>
        /// 预制件对象
        /// </summary>
        [HideInInspector]
        public GameObject skillPrefab;
        [Header("动画名称")]
        /// <summary>
        /// 动画名称
        /// </summary>
        public string animationName;
        [Header("受击特效名称")]
        /// <summary>
        /// 受击特效名称
        /// </summary>
        public string hitFxName;
        /// <summary>
        /// 受击特效预制件
        /// </summary>
        [HideInInspector]
        public GameObject hitFxPrefab;
        [Header("攻击类型")]
        /// <summary>
        /// 攻击类型 单攻/群攻
        /// </summary>
        public SkillAttackType attackType;
        [Header("选择类型")]
        /// <summary>
        /// 选择类型 扇形/矩形
        /// </summary>
        public SelectorType selectorType;
        [Header("技能显示区域类型")]
        /// <summary>
        /// 技能区域类型
        /// </summary>
        public SkillAreaType areaType;
        [Header("技能释放方式")]
        /// <summary>
        /// 技能释放方式
        /// </summary>
        public SkillGenerateType generateType;
    }
}