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
        //����Ԫ���б�
        private Dictionary<SkillAreaElement, Transform> allElementTrans;
        //����ָʾ����
        private Transform elementParent;

        private void Start()
        {
            for (int i = 0; i < skills.Length; i++)
                InitSkill(skills[i]);

            allElementTrans = new Dictionary<SkillAreaElement, Transform>
            {
                { SkillAreaElement.OuterCircle, null },
                { SkillAreaElement.Rectangle, null },
                { SkillAreaElement.InnerCircle, null },
                { SkillAreaElement.Sector120, null },
                { SkillAreaElement.Sector60, null }
            };
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
        public void GenerateSkill(SkillData data, Vector3 deltaVac)
        {
            GameObject skillGo;
            //�����ܷ���
            transform.LookAt(deltaVac + transform.position);
            //����
            if (data.selectorType != SelectorType.Attack)
            {
                skillGo = GameObjectPool.Instance.CreateObject(data.name, data.skillPrefab, transform.position + deltaVac * data.attackDistance, transform.rotation);
            }
            else
            {
                skillGo = GameObjectPool.Instance.CreateObject(data.name, data.skillPrefab, data.attackPos.position, transform.rotation);
                skillGo.GetComponent<ns.BulletEffect>().Init();
                skillGo.GetComponent<Rigidbody>().AddForce(this.transform.forward * 500);
                return;
            }
            data.prefabTF = skillGo.transform;
            //���ݼ�������
            SkillDeployer deployer = skillGo.GetComponent<SkillDeployer>();
            deployer.SkillData = data;//�ڲ������㷨����
            deployer.DeploySkill();//�ڲ�ִ���㷨����

            //����
            //Destroy(skillGo, data.durationTime);
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


        #region ����������ʾ

        /// <summary>
        /// ������������չʾ
        /// </summary>
        /// <param name="areaType"></param>
        public void CreateSkillArea(SkillData data)
        {
            switch (data.areaType)
            {
                case SkillAreaType.OuterCircle:
                    CreateElement(SkillAreaElement.OuterCircle, data);
                    break;
                case SkillAreaType.OuterCircle_InnerRectangle:
                    CreateElement(SkillAreaElement.Rectangle, data);
                    break;
                case SkillAreaType.OuterCircle_InnerSector:
                    CreateElement(SkillAreaElement.OuterCircle, data);
                    CreateElement(SkillAreaElement.Sector60, data);
                    break;
                case SkillAreaType.OuterCircle_InnerCircle:
                    CreateElement(SkillAreaElement.OuterCircle, data);
                    CreateElement(SkillAreaElement.InnerCircle, data);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// ����Ԫ��
        /// </summary>
        public void HideElement()
        {
            if (elementParent == null) return;
            Transform parent = GetParent();
            for (int i = 0, length = parent.childCount; i < length; i++)
            {
                parent.GetChild(i).gameObject.SetActive(false);
            }
        }
        /// <summary>
        /// ÿ֡����Ԫ��
        /// </summary>
        public void UpdateElement(float dist, Vector3 deltaVac)
        {
            UpdaElementPosition(SkillAreaElement.OuterCircle, dist, deltaVac);
            UpdaElementPosition(SkillAreaElement.Rectangle, dist, deltaVac);
            UpdaElementPosition(SkillAreaElement.InnerCircle, dist, deltaVac);
            UpdaElementPosition(SkillAreaElement.Sector60, dist, deltaVac);
            UpdaElementPosition(SkillAreaElement.Sector120, dist, deltaVac);
        }

        /// <summary>
        /// ÿ֡����Ԫ��λ��
        /// </summary>
        /// <param name="cube"></param>
        private void UpdaElementPosition(SkillAreaElement element, float dist, Vector3 deltaVac)
        {
            if (allElementTrans[element] == null) return;
            switch (element)
            {
                case SkillAreaElement.OuterCircle:
                    break;
                case SkillAreaElement.InnerCircle:
                    allElementTrans[element].transform.position = GetCirclePosition(dist, deltaVac);
                    break;
                case SkillAreaElement.Rectangle:
                case SkillAreaElement.Sector60:
                case SkillAreaElement.Sector120:
                    allElementTrans[element].transform.LookAt(GetCubeSectorLookAt(deltaVac));
                    break;
            }
        }
        /// <summary>
        /// ��ȡInnerCirclԪ��λ��
        /// </summary>
        /// <param name="outerRadius"></param>
        /// <returns></returns>
        private Vector3 GetCirclePosition(float dist, Vector3 deltaVac)
        {
            Vector3 targetDir = deltaVac * dist;
            float y = Camera.main.transform.rotation.eulerAngles.y;
            targetDir = Quaternion.Euler(0, y, 0) * targetDir;
            return targetDir + this.transform.position;
        }
        /// <summary>
        /// ��ȡCube,SectorԪ�س���
        /// </summary>
        /// <returns></returns>
        private Vector3 GetCubeSectorLookAt(Vector3 deltaVac)
        {
            Vector3 targetDir = deltaVac;
            float y = Camera.main.transform.rotation.eulerAngles.y;
            targetDir = Quaternion.Euler(0, y, 0) * targetDir;
            return targetDir + this.transform.position;
        }
        /// <summary>
        /// ������������չʾԪ��
        /// </summary>
        /// <param name="skillAreaElement"></param>
        private void CreateElement(SkillAreaElement skillAreaElement, SkillData data)
        {
            Transform elementTrans = GetElement(skillAreaElement);
            if (elementTrans == null) return;
            allElementTrans[skillAreaElement] = elementTrans;
            switch (skillAreaElement)
            {
                case SkillAreaElement.OuterCircle:
                    elementTrans.localScale = new Vector3(data.attackDistance * 2, 1, data.attackDistance * 2) / this.transform.localScale.x;
                    elementTrans.gameObject.SetActive(true);
                    break;
                case SkillAreaElement.InnerCircle:
                    elementTrans.localScale = new Vector3(data.attackWide * 2, 1, data.attackWide * 2) / this.transform.localScale.x;
                    elementTrans.gameObject.SetActive(true);
                    break;
                case SkillAreaElement.Rectangle:
                    elementTrans.localScale = new Vector3(data.attackWide, 1, data.attackDistance) / this.transform.localScale.x;
                    elementTrans.gameObject.SetActive(true);
                    break;
                case SkillAreaElement.Sector120:
                    elementTrans.localScale = new Vector3(data.attackDistance, 1, data.attackDistance) / this.transform.localScale.x;
                    elementTrans.gameObject.SetActive(true);
                    break;
                case SkillAreaElement.Sector60:
                    elementTrans.localScale = new Vector3(data.attackDistance, 1, data.attackDistance) / this.transform.localScale.x;
                    elementTrans.gameObject.SetActive(true);
                    break;
            }
        }
        /// <summary>
        /// ��ȡԪ������
        /// </summary>
        /// <param name="skillAreaElement"></param>
        /// <returns></returns>
        private Transform GetElement(SkillAreaElement skillAreaElement)
        {
            string name = skillAreaElement.ToString();
            Transform parent = GetParent();
            Transform elementTrans = parent.Find(name);
            if (elementTrans == null)
            {
                GameObject elementGo = Instantiate(ResourcesManager.Load<GameObject>(name));
                elementGo.transform.parent = parent;
                elementGo.gameObject.SetActive(false);
                elementGo.name = name;
                elementTrans = elementGo.transform;
            }
            elementTrans.localEulerAngles = Vector3.zero;
            elementTrans.localPosition = Vector3.zero;
            elementTrans.localScale = Vector3.one;
            return elementTrans;
        }
        /// <summary>
        /// ��ȡԪ�ظ�����
        /// </summary>
        /// <returns></returns>
        private Transform GetParent()
        {
            if (elementParent == null)
                elementParent = this.transform.Find("SkillArea");
            if (elementParent == null)
            {
                elementParent = new GameObject("SkillArea").transform;
                elementParent.parent = this.transform;
                elementParent.localEulerAngles = Vector3.zero;
                elementParent.localPosition = Vector3.zero;
                elementParent.localScale = Vector3.one;
            }
            return elementParent;
        }
        #endregion
    }
}