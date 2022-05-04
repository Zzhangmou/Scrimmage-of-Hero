using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

namespace Scrimmage.Skill
{
    /// <summary>
    /// ���ܹ�����
    /// </summary>
    public class CharacterSkillManager : MonoBehaviour
    {
        //�����б�
        public SkillData[] skills;

        private void Start()
        {
            for (int i = 0; i < skills.Length; i++)
                InitSkill(skills[i]);
        }

        //��ʼ������
        private void InitSkill(SkillData data)
        {
            /*
             * ��Դӳ���
             * ��Դ����---->��Դ����·��
             */

            //data.skillPrefab = Resources.Load<GameObject>("Prefabs/SkillFX/" + data.prefabName);
            data.skillPrefab = ResourcesManager.Load<GameObject>(data.prefabName);
            data.owner = gameObject;
            data.attackPos = transform.Find("FirePos");
        }
        //׼������
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
            //��������Ԥ�Ƽ�
            GameObject skillGo = GameObjectPool.Instance.CreateObject(data.prefabName, data.skillPrefab, data.prefabPos, transform.rotation);
            //GameObject skillGo = GameObjectPool.Instance.CreateObject(data.prefabName, data.skillPrefab, data.prefabPos, Quaternion.Euler(data.prefabRotation));
            SkillDeployer deployer = skillGo.GetComponent<SkillDeployer>();
            //���ݼ�������
            //�ڲ������㷨����
            deployer.SkillData = data;
            //�ڲ�ִ���㷨����
            deployer.DeploySkill();
            if (data.generateType != SkillGenerateType.FileAndDIs)
                GameObjectPool.Instance.CollectObject(skillGo, data.durationTime);
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
    }
}