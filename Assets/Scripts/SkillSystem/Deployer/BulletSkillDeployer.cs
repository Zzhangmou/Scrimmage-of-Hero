using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using proto;
using Character;

namespace Scrimmage.Skill
{
    /// <summary>
    /// �ӵ��ͷ���
    /// </summary>
    public class BulletSkillDeployer : SkillDeployer, IResetable
    {
        //�ӵ�������Ӫ
        public int camp = -1;
        public Vector3 targetTf;
        public float speed;
        private CharacterStatus status;
        public override void DeploySkill()
        {
            targetTf = SkillData.attackPos.TransformPoint(0, 0, SkillData.attackDistance);
            speed = SkillData.bulletSpeed;
            status = SkillData.owner.GetComponent<CharacterStatus>();
            camp = status.camp;
        }
        private void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, targetTf, Time.deltaTime * speed);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Enemy") return;
            targetTf = other.transform.position;
            if (camp == -1) return;
            print(other.name);
            CharacterStatus otherStaus = other.GetComponent<CharacterStatus>();
            otherStaus.Damage(status.baseATK);
            MsgHit msgHit = new MsgHit();
            msgHit.targetId = otherStaus.id;
            msgHit.hitNum = status.baseATK;
            NetWorkFK.NetManager.Send(msgHit);
        }

        public void OnReset()
        {
            //��������
            camp = -1;
        }
    }
}
