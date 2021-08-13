using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

namespace Scrimmage.Skill
{
    /// <summary>
    /// 扇形/圆形选区
    /// </summary>
    public class SectorAttackSelector : IAttackSelector
    {
        public Transform[] SelectTarget(SkillData data, Transform skillTF)
        {
            List<Transform> targets = new List<Transform>();
            //根据技能数据中的标签 获取所有目标
            for (int i = 0; i < data.attackTargetTags.Length; i++)
            {
                GameObject[] tempGoArray = GameObject.FindGameObjectsWithTag(data.attackTargetTags[i]);
                targets.AddRange(tempGoArray.Select(g => g.transform));
            }
            //判断攻击范围(扇形/圆形)
            targets = targets.FindAll(t =>
                  Vector3.Distance(t.position, data.prefabTF.position) <= data.attackWide &&
                  Vector3.Angle(data.prefabTF.forward, t.position - data.prefabTF.position) <= data.attackAngle / 2);
            //筛选出活的角色
            targets = targets.FindAll(t => t.GetComponent<ns.CharacterStatus>().HP > 0);
            //返回目标(单攻/群攻)
            Transform[] result = targets.ToArray();
            if (data.attackType == SkillAttackType.Group || result.Length == 0)
                return result;
            //距离最近
            Transform min = result.FindMin(t => Vector3.Distance(t.position, data.prefabTF.position));
            return new Transform[] { min };
        }
    }
}