using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scrimmage.Skill
{
    /// <summary>
    /// Ӱ��Ч���㷨�ӿ�
    /// </summary>
   public interface IImpactEffect
   {
        /// <summary>
        /// �˺�����
        /// </summary>
        /// <param name="data"></param>
        void Execute(SkillDeployer deployer);
   }
}

