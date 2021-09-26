using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using Character;

namespace Scrimmage.Skill
{
    /// <summary>
    /// ���Ρ�Բ��ѡ��
    /// </summary>
    public class SectorAttackSelector : IAttackSelector
    {
        public Transform[] SelectTarget(SkillData data, Transform skillTF)
        {
            //���ݼ��������еı�ǩ��ȡ����Ŀ��
            List<Transform> targets = new List<Transform>();
            for (int i = 0; i < data.attackTargetTags.Length; i++)
            {
                GameObject[] tempGoArray = GameObject.FindGameObjectsWithTag(data.attackTargetTags[i]);
                targets.AddRange(tempGoArray.Select(g => g.transform));
            }
            //�жϹ�����Χ
            targets = targets.FindAll(t =>
                  Vector3.Distance(t.position, skillTF.position) <= data.attackDistance &&
                  Vector3.Angle(skillTF.forward, t.position - skillTF.position) <= data.attackAngle / 2
              );
            //ɸѡ����ý�ɫ
            targets = targets.FindAll(t => t.GetComponent<CharacterStatus>().HP > 0);
            //����Ŀ��
            Transform[] result = targets.ToArray();
            if (data.attackType == SkillAttackType.Group || result.Length == 0)
                return result;
            //�������
            Transform min = result.FindMin(t => Vector3.Distance(t.position, skillTF.position));
            return new Transform[] { min };
        }
    }
}