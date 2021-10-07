using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

namespace Scrimmage.Skill
{
    /// <summary>
    /// �����ͷ���
    /// </summary>
    public class FollowSkillDeployer : SkillDeployer, IResetable
    {
        public override void DeploySkill()
        {
            //�����ͷ���
            transform.parent = SkillData.owner.transform;

            //ִ��ѡ���㷨
            CalculateTargets();

            //ִ��Ӱ���㷨
            ImpactTargets();
        }

        public void OnReset()
        {
            transform.SetParent(null);
        }
    }
}
