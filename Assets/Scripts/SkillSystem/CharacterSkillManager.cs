using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

namespace ns
{
    /// <summary>
    /// ���ܹ�����
    /// </summary>
    public class CharacterSkillManager : MonoBehaviour
    {
        //�����б�
        public SkillData[] skills;
        //����Ԫ���б�
        private Dictionary<SkillAreaElement, Transform> allElementTrans;
        //����ָʾ����
        private Transform elementParent;

        private void Start()
        {
            for (int i = 0; i < skills.Length; i++)
                InitSkill(skills[i]);
        }
        //��ʼ������
        private void InitSkill(SkillData data)
        {
            data.skillPrefab = Resources.Load<GameObject>("Prefabs/SkillFX/" + data.prefabName);
            data.owner = gameObject;
        }
        public SkillData PrepareSkill(int id)
        {
            //����id ���Ҽ�������
            SkillData data = skills.Find(s => s.skillId == id);
            //�ж�����            ���ؼ�������
            if (data != null && data.coolRemain <= 0)
                return data;
            else
                return null;
        }
        //���ɼ���
        public void GenerateSkill(SkillData data)
        {
            //����
            GameObject skillGo = Instantiate(data.skillPrefab, transform.position, transform.rotation);
            //����
            Destroy(skillGo, data.durationTime);
            //��ȴ
            StartCoroutine(CoolTimeDown(data));
        }
        //������ȴ
        private IEnumerator CoolTimeDown(SkillData data)
        {
            data.coolRemain = data.coolTime;
            while (data.coolRemain > 0)
            {
                yield return new WaitForSeconds(1);
                data.coolRemain--;
            }
        }


        #region ����������ʾ



        #endregion
    }
}

