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
        private SkillData skilldata;
        //选区算法对象
        private IAttackSelector selector;
        //影响效果算法对象
        private IImpactEffect[] impactArray;
        //由技能管理器提供
        public SkillData SkillData
        {
            get
            {
                return skilldata;
            }
            set
            {
                skilldata = value;
                //创建算法对象
                InitDeployer();
            }
        }
        //初始化释放器
        private void InitDeployer()
        {
            //创建算法对象
            //选区
            selector = DeployerConfigFactory.CreateAttackSelector(SkillData);
            //影响
            impactArray = DeployerConfigFactory.CreateImpactEffects(SkillData);
        }

        //执行算法对象
        //选区
        public void CalculateTargets()
        {
            skilldata.attackTargets = selector.SelectTarget(skilldata, transform);

            //测试!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            foreach (var item in skilldata.attackTargets)
            {
                print(item + " " + Time.frameCount);
            }
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
        public abstract void DeploySkill();//供技能管理器调用 由子类实现 定义具体释放策略
    }
}
