using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scrimmage.Skill
{
    /// <summary>
    /// 释放器配置工厂:提供创建释放器算法对象各种算法对象的功能
    /// 作用:将对象 创建 与 使用 分离
    /// </summary>
   public class DeployerConfigFactory
   {
        public static IAttackSelector CreateAttackSelector(SkillData data)
        {
            //skillData.selectorType
            //命名规则 Scrimmage.Skill. +枚举名 + AttackSelector
            string className = string.Format("Scrimmage.Skill.{0}AttackSelector", data.selectorType);
            return CreateObject<IAttackSelector>(className);
        }
        public static IImpactEffect[] CreateImpactEffects(SkillData data)
        {
            IImpactEffect[] impacts = new IImpactEffect[data.impactType.Length];
            //命名规则 Scrimmage.Skill. +impactType[?] + Impact
            for (int i = 0; i < data.impactType.Length; i++)
            {
                string classNameImpact = string.Format("Scrimmage.Skill.{0}Impact", data.impactType[i]);
                impacts[i] = CreateObject<IImpactEffect>(classNameImpact);
            }
            return impacts;
        }
        private static T CreateObject<T>(string className) where T : class
        {
            Type type = Type.GetType(className);
            return Activator.CreateInstance(type) as T;
        }
    }
}

