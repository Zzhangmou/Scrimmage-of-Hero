using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scrimmage.Skill
{
    /// <summary>
    /// 近身释放器
    /// </summary>
    public class MeleeSkillDeployer : SkillDeployer
    {
        public override void DeploySkill()
        {
            //执行选区算法
            CalculateTargets();

            //执行影响算法
            ImpactTargets();
        }
    }
}

