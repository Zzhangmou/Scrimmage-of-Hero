using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scrimmage.Skill
{
    /// <summary>
    /// �����ͷ���
    /// </summary>
    public class FollowSkillDeployer : SkillDeployer
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
    }
}
