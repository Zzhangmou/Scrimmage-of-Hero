using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using proto;
using Character;

namespace Scrimmage.Skill
{
    /// <summary>
    /// 子弹释放器
    /// </summary>
    public class BulletSkillDeployer : SkillDeployer, IResetable
    {
        //子弹所属阵营
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
            if (Vector3.Distance(transform.position, targetTf) < 0.1f)
                GameObjectPool.Instance.CollectObject(gameObject);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (camp == -1) return;
            CharacterStatus otherStatus = other.GetComponent<CharacterStatus>();
            if (camp == otherStatus.camp) return;
            GameObjectPool.Instance.CollectObject(gameObject);
            if (other.tag != "Enemy") return;
            print(other.name);
            CharacterStatus otherStaus = other.GetComponent<CharacterStatus>();
            float hurtNum = status.baseATK * SkillData.atkRadio;
            otherStaus.Damage(hurtNum);
            MsgHit msgHit = new MsgHit();
            msgHit.targetId = otherStaus.id;
            msgHit.hitNum = hurtNum;
            NetWorkFK.NetManager.Send(msgHit);
        }

        public void OnReset()
        {
            //重置属性
            camp = -1;
        }
    }
}
