using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

namespace Scrimmage.Skill
{
    /// <summary>
    /// 跟随释放器
    /// </summary>
    public class FollowSkillDeployer : SkillDeployer, IResetable
    {
        public override void DeploySkill()
        {
            //跟随释放者
            transform.parent = SkillData.owner.transform;

            //执行选区算法
            CalculateTargets();

            //执行影响算法
            ImpactTargets();
        }

        public void OnReset()
        {
            transform.SetParent(null);
        }
    }
}
