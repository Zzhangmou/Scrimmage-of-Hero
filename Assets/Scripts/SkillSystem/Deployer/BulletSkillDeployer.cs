using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

namespace Scrimmage.Skill
{
    /// <summary>
    /// ×Óµ¯ÊÍ·ÅÆ÷
    /// </summary>
    public class BulletSkillDeployer : SkillDeployer
    {
        public Vector3 targetTf;
        public float speed;
        public override void DeploySkill()
        {
            targetTf = SkillData.attackPos.TransformPoint(0, 0, SkillData.attackDistance);
            speed = SkillData.bulletSpeed;
        }
        private void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, targetTf, Time.deltaTime * speed);
        }
        private void OnCollisionEnter(Collision collision)
        {
            targetTf = collision.transform.position;
            print(collision.gameObject.name);
        }
    }
}
