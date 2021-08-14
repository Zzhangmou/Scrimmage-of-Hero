using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scrimmage.Skill
{
    /// <summary>
    /// 影响效果算法接口
    /// </summary>
   public interface IImpactEffect
   {
        /// <summary>
        /// 伤害生命
        /// </summary>
        /// <param name="data"></param>
        void Execute(SkillDeployer deployer);
   }
}

