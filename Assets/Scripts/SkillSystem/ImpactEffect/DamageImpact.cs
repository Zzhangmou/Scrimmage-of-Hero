using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using proto;

namespace Scrimmage.Skill
{
    /// <summary>
    /// 伤害生命
    /// </summary>
    public class DamageImpact : IImpactEffect
    {
        //private SkillData data;
        public void Execute(SkillDeployer deployer)
        {
            //data = deployer.SkillData;
            deployer.StartCoroutine(RepeatDamage(deployer));
        }

        private IEnumerator RepeatDamage(SkillDeployer deployer)
        {
            float atkTime = 0;
            do
            {
                //伤害目标生命
                OnceDamage(deployer.SkillData);
                yield return new WaitForSeconds(deployer.SkillData.atkInterval);
                atkTime += deployer.SkillData.atkInterval;
                //重新计算一次目标
                deployer.CalculateTargets();
            } while (atkTime < deployer.SkillData.durationTime);//攻击时间没到
        }

        private void OnceDamage(SkillData data)
        {
            //deployer.SkillData.attackTargets
            //技能攻击力 :攻击比率 * 基础攻击力
            float atk = data.atkRadio * data.owner.GetComponent<CharacterStatus>().baseATK;
            for (int i = 0; i < data.attackTargets.Length; i++)
            {
                var status = data.attackTargets[i].GetComponent<CharacterStatus>();
                status.Damage(atk);
                //发送伤害协议
                MsgHit msgHit = new MsgHit();
                msgHit.targetId = status.id;
                msgHit.hitNum = atk;
                NetWorkFK.NetManager.Send(msgHit);
            }
            //创建攻击特效
        }
    }
}
