using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using proto;

namespace Scrimmage.Skill
{
    /// <summary>
    /// �˺�����
    /// </summary>
    public class DamageImpact : IImpactEffect
    {
        private SkillData data;
        public void Execute(SkillDeployer deployer)
        {
            data = deployer.SkillData;
            deployer.StartCoroutine(RepeatDamage(deployer));
        }

        private IEnumerator RepeatDamage(SkillDeployer deployer)
        {
            float atkTime = 0;
            do
            {
                //�˺�Ŀ������
                OnceDamage();
                yield return new WaitForSeconds(data.atkInterval);
                atkTime += data.atkInterval;
                //���¼���һ��Ŀ��
                deployer.CalculateTargets();
            } while (atkTime < data.durationTime);//����ʱ��û��
        }

        private void OnceDamage()
        {
            //deployer.SkillData.attackTargets
            //���ܹ����� :�������� * ����������
            float atk = data.atkRadio * data.owner.GetComponent<CharacterStatus>().baseATK;
            for (int i = 0; i < data.attackTargets.Length; i++)
            {
                var status = data.attackTargets[i].GetComponent<CharacterStatus>();
                status.Damage(atk);
                //�����˺�Э��
                MsgHit msgHit = new MsgHit();
                msgHit.targetId = status.id;
                msgHit.hitNum = atk;
                NetWorkFK.NetManager.Send(msgHit);
            }
            //����������Ч
        }
    }
}
