using System;
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
        [Header("����Id")]
        /// <summary>
        /// ����Id
        /// </summary>
        public int skillId;
        [Header("��������")]
        /// <summary>
        /// ��������
        /// </summary>
        public string skillName;
        [Header("��������")]
        /// <summary>
        /// ��������
        /// </summary>
        public string description;
        [Header("��ȴʱ��")]
        /// <summary>
        /// ��ȴʱ��
        /// </summary>
        public int coolTime;
        [Header("��ȴʣ��")]
        /// <summary>
        /// ��ȴʣ��
        /// </summary>
        [HideInInspector]
        public int coolRemain;
        /// <summary>
        /// ��������λ��
        /// </summary>
        [HideInInspector]
        public Vector3 prefabPos;
        /// <summary>
        /// ����λ��
        /// </summary>
        [HideInInspector]
        public Transform attackPos;
        [Header("������Χ")]
        /// <summary>
        /// ������Χ
        /// </summary>
        public float attackScope;
        [Header("��������")]
        /// <summary>
        /// ��������
        /// </summary>
        public float attackDistance;
        [Header("�����Ƕ�")]
        /// <summary>
        /// �����Ƕ�
        /// </summary>
        public float attackAngle;
        [Header("�ӵ����ͼ��������ٶ�")]
        /// <summary>
        /// �ӵ������ٶ�
        /// </summary>
        public float bulletSpeed;
        [Header("����Ŀ��Tag")]
        /// <summary>
        /// ����Ŀ��Tag
        /// </summary>
        public string[] attackTargetTags;
        /// <summary>
        /// ����Ŀ���������
        /// </summary>
        [HideInInspector]
        public Transform[] attackTargets;
        [Header("����Ӱ������")]
        /// <summary>
        /// ����Ӱ������
        /// </summary>
        public string[] impactType;
        [Header("�˺�����")]
        /// <summary>
        /// �˺�����
        /// </summary>
        public float atkRadio;
        [Header("����ʱ��")]
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public float durationTime;
        [Header("�˺����")]
        /// <summary>
        /// �˺����
        /// </summary>
        public float atkInterval;
        /// <summary>
        /// ��������
        /// </summary>
        [HideInInspector]
        public GameObject owner;
        [Header("����Ԥ�Ƽ�����")]
        /// <summary>
        /// ����Ԥ�Ƽ�����
        /// </summary>
        public string prefabName;
        /// <summary>
        /// Ԥ�Ƽ�����
        /// </summary>
        [HideInInspector]
        public GameObject skillPrefab;
        [Header("��������")]
        /// <summary>
        /// ��������
        /// </summary>
        public string animationName;
        [Header("�ܻ���Ч����")]
        /// <summary>
        /// �ܻ���Ч����
        /// </summary>
        public string hitFxName;
        /// <summary>
        /// �ܻ���ЧԤ�Ƽ�
        /// </summary>
        [HideInInspector]
        public GameObject hitFxPrefab;
        [Header("��������")]
        /// <summary>
        /// �������� ����/Ⱥ��
        /// </summary>
        public SkillAttackType attackType;
        [Header("ѡ������")]
        /// <summary>
        /// ѡ������ ����/����
        /// </summary>
        public SelectorType selectorType;
        [Header("������ʾ��������")]
        /// <summary>
        /// ������������
        /// </summary>
        public SkillAreaType areaType;
        [Header("�����ͷŷ�ʽ")]
        /// <summary>
        /// �����ͷŷ�ʽ
        /// </summary>
        public SkillGenerateType generateType;
    }
}