using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scrimmage.Skill
{
    /// <summary>
    /// �ͷ������ù���:�ṩ�����ͷ����㷨��������㷨����Ĺ���
    /// ����:������ ���� �� ʹ�� ����
    /// </summary>
    public class DeployerConfigFactory
    {
        private static Dictionary<string, object> cache;
        static DeployerConfigFactory()
        {
            cache = new Dictionary<string, object>();
        }
        public static IAttackSelector CreateAttackSelector(SkillData data)
        {
            //skillData.selectorType
            //�������� Scrimmage.Skill. +ö���� + AttackSelector
            string className = string.Format("Scrimmage.Skill.{0}AttackSelector", data.selectorType);
            return CreateObject<IAttackSelector>(className);
        }
        public static IImpactEffect[] CreateImpactEffects(SkillData data)
        {
            IImpactEffect[] impacts = new IImpactEffect[data.impactType.Length];
            //�������� Scrimmage.Skill. +impactType[?] + Impact
            for (int i = 0; i < data.impactType.Length; i++)
            {
                string classNameImpact = string.Format("Scrimmage.Skill.{0}Impact", data.impactType[i]);
                impacts[i] = CreateObject<IImpactEffect>(classNameImpact);
            }
            return impacts;
        }
        private static T CreateObject<T>(string className) where T : class
        {
            if (!cache.ContainsKey(className))
            {
                Debug.Log("����");
                Type type = Type.GetType(className);
                object instance = Activator.CreateInstance(type);
                cache.Add(className, instance);
            }
            return cache[className] as T;
        }
    }
}

