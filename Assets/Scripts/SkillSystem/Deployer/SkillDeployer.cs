using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scrimmage.Skill
{
    /// <summary>
    /// �����ͷ���
    /// </summary>
    public abstract class SkillDeployer : MonoBehaviour
    {
        private SkillData skillData;
        //ѡ���㷨����
        private IAttackSelector selector;
        //Ӱ��Ч���㷨����
        private IImpactEffect[] impactArray;
        public SkillData SkillData
        {
            get { return skillData; }
            set
            {
                skillData = value;
                //�����㷨����
                InitDeployer();
            }
        }

        //��ʼ���ͷ���
        private void InitDeployer()
        {
            //�����㷨����
            //ѡ��
            selector = DeployerConfigFactory.CreateAttackSelector(skillData);
            //Ӱ��
            impactArray = DeployerConfigFactory.CreateImpactEffects(skillData);
        }
        //ִ���㷨����
        //ѡ��
        public void CalculateTargets()
        {
            skillData.attackTargets = selector.SelectTarget(skillData, transform);
            ////////////////////////////////////����
            //foreach (var item in skillData.attackTargets)
            //{
            //    print(item);
            //}
        }
        //Ӱ��
        public void ImpactTargets()
        {
            for (int i = 0; i < impactArray.Length; i++)
            {
                impactArray[i].Execute(this);
            }
        }

        //�ͷŷ�ʽ
        //�����ܹ���������,������ʵ��,��������ͷŲ���
        public abstract void DeploySkill();
    }
}

