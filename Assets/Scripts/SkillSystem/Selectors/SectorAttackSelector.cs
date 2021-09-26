using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using Character;

namespace Scrimmage.Skill
{
    /// <summary>
    /// 扇形、圆形选区
    /// </summary>
    public class SectorAttackSelector : IAttackSelector
    {
        public Transform[] SelectTarget(SkillData data, Transform skillTF)
        {
            //根据技能数据中的标签获取所有目标
            List<Transform> targets = new List<Transform>();
            for (int i = 0; i < data.attackTargetTags.Length; i++)
            {
                GameObject[] tempGoArray = GameObject.FindGameObjectsWithTag(data.attackTargetTags[i]);
                targets.AddRange(tempGoArray.Select(g => g.transform));
            }
            //判断攻击范围
            targets = targets.FindAll(t =>
                  Vector3.Distance(t.position, skillTF.position) <= data.attackDistance &&
                  Vector3.Angle(skillTF.forward, t.position - skillTF.position) <= data.attackAngle / 2
              );
            //筛选出活得角色
            targets = targets.FindAll(t => t.GetComponent<CharacterStatus>().HP > 0);
            //返回目标
            Transform[] result = targets.ToArray();
            if (data.attackType == SkillAttackType.Group || result.Length == 0)
                return result;
            //距离最近
            Transform min = result.FindMin(t => Vector3.Distance(t.position, skillTF.position));
            return new Transform[] { min };
        }
    }
}