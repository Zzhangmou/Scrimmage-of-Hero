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
        private SkillData skilldata;
        //ѡ���㷨����
        private IAttackSelector selector;
        //Ӱ��Ч���㷨����
        private IImpactEffect[] impactArray;
        //�ɼ��ܹ������ṩ
        public SkillData SkillData
        {
            get
            {
                return skilldata;
            }
            set
            {
                skilldata = value;
                //�����㷨����
                InitDeployer();
            }
        }
        //��ʼ���ͷ���
        private void InitDeployer()
        {
            //�����㷨����
            //ѡ��
            selector = DeployerConfigFactory.CreateAttackSelector(SkillData);
            //Ӱ��
            impactArray = DeployerConfigFactory.CreateImpactEffects(SkillData);
        }

        //ִ���㷨����
        //ѡ��
        public void CalculateTargets()
        {
            skilldata.attackTargets = selector.SelectTarget(skilldata, transform);

            //����!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            foreach (var item in skilldata.attackTargets)
            {
                print(item + " " + Time.frameCount);
            }
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
        public abstract void DeploySkill();//�����ܹ��������� ������ʵ�� ��������ͷŲ���
    }
}
