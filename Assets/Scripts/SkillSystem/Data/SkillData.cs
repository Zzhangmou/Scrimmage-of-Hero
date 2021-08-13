using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scrimmage.Skill
{
    [System.Serializable]
    /// <summary>
    /// ��������
    /// </summary>
    public class SkillData
    {
        /// <summary>
        /// ����Id
        /// </summary>
        public int skillId;
        /// <summary>
        /// ��������
        /// </summary>
        public string name;
        /// <summary>
        /// ��������
        /// </summary>
        public string description;
        /// <summary>
        /// ��ȴʱ��
        /// </summary>
        public int coolTime;
        /// <summary>
        /// ��ȴʣ��
        /// </summary>
        public int coolRemain;
        /// <summary>
        /// ��������λ��
        /// </summary>
        [HideInInspector]
        public Transform prefabTF;
        /// <summary>
        /// ����λ��
        /// </summary>
        [HideInInspector]
        public Transform attackPos;
        /// <summary>
        /// ��������
        /// </summary>
        public float attackDistance;
        /// <summary>
        /// �����Ƕ�
        /// </summary>
        public float attackAngle;
        /// <summary>
        /// ���ܹ�����Χ
        /// </summary>
        public float attackWide = 2f;
        /// <summary>
        /// ����Ŀ��Tag
        /// </summary>
        public string[] attackTargetTags;
        /// <summary>
        /// ����Ŀ���������
        /// </summary>
        [HideInInspector]
        public Transform[] attackTargets;
        /// <summary>
        /// ����Ӱ������
        /// </summary>
        public string[] impactType;
        /// <summary>
        /// �˺�����
        /// </summary>
        public float atkRadio;
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public float durationTime;
        /// <summary>
        /// �˺����
        /// </summary>
        public float atkInterval;
        /// <summary>
        /// ��������
        /// </summary>
        [HideInInspector]
        public GameObject owner;
        /// <summary>
        /// ����Ԥ�Ƽ�����
        /// </summary>
        public string prefabName;
        /// <summary>
        /// Ԥ�Ƽ�����
        /// </summary>
        [HideInInspector]
        public GameObject skillPrefab;
        /// <summary>
        /// ��������
        /// </summary>
        public string animationName;
        /// <summary>
        /// �ܻ���Ч����
        /// </summary>
        public string hitFxName;
        /// <summary>
        /// �ܻ���ЧԤ�Ƽ�
        /// </summary>
        [HideInInspector]
        public GameObject hitFxPrefab;
        /// <summary>
        /// ��������
        /// </summary>
        public SkillAttackType attackType;
        /// <summary>
        /// ѡ������
        /// </summary>
        public SelectorType selectorType;
        /// <summary>
        /// ����ָʾ����
        /// </summary>
        public SkillAreaType areaType;
    }
}