using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

namespace Scrimmage.Skill
{
    /// <summary>
    /// ����/Բ��ѡ��
    /// </summary>
    public class SectorAttackSelector : IAttackSelector
    {
        public Transform[] SelectTarget(SkillData data, Transform skillTF)
        {
            List<Transform> targets = new List<Transform>();
            //���ݼ��������еı�ǩ ��ȡ����Ŀ��
            for (int i = 0; i < data.attackTargetTags.Length; i++)
            {
                GameObject[] tempGoArray = GameObject.FindGameObjectsWithTag(data.attackTargetTags[i]);
                targets.AddRange(tempGoArray.Select(g => g.transform));
            }
            //�жϹ�����Χ(����/Բ��)
            targets = targets.FindAll(t =>
                  Vector3.Distance(t.position, data.prefabTF.position) <= data.attackWide &&
                  Vector3.Angle(data.prefabTF.forward, t.position - data.prefabTF.position) <= data.attackAngle / 2);
            //ɸѡ����Ľ�ɫ
            targets = targets.FindAll(t => t.GetComponent<ns.CharacterStatus>().HP > 0);
            //����Ŀ��(����/Ⱥ��)
            Transform[] result = targets.ToArray();
            if (data.attackType == SkillAttackType.Group || result.Length == 0)
                return result;
            //�������
            Transform min = result.FindMin(t => Vector3.Distance(t.position, data.prefabTF.position));
            return new Transform[] { min };
        }
    }
}