using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scrimmage.Skill
{
    /// <summary>
    /// 技能释放器
    /// </summary>
    public abstract class SkillDeployer : MonoBehaviour
    {
        private SkillData skillData;
        //选区算法对象
        private IAttackSelector selector;
        //影响效果算法对象
        private IImpactEffect[] impactArray;
        public SkillData SkillData
        {
            get { return skillData; }
            set
            {
                skillData = value;
                //创建算法对象
                InitDeployer();
            }
        }

        //初始化释放器
        private void InitDeployer()
        {
            //创建算法对象
            //选区
            selector = DeployerConfigFactory.CreateAttackSelector(skillData);
            //影响
            impactArray = DeployerConfigFactory.CreateImpactEffects(skillData);
        }
        //执行算法对象
        //选区
        public void CalculateTargets()
        {
            skillData.attackTargets = selector.SelectTarget(skillData, transform);
            ////////////////////////////////////测试
            //foreach (var item in skillData.attackTargets)
            //{
            //    print(item);
            //}
        }
        //影响
        public void ImpactTargets()
        {
            for (int i = 0; i < impactArray.Length; i++)
            {
                impactArray[i].Execute(this);
            }
        }

        //释放方式
        //供技能管理器调用,由子类实现,定义具体释放策略
        public abstract void DeploySkill();
    }
}

