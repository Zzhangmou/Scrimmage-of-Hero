using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

namespace Scrimmage.Skill
{
    /// <summary>
    /// 技能管理器
    /// </summary>
    public class CharacterSkillManager : MonoBehaviour
    {
        //技能列表
        public SkillData[] skills;
        //技能元素列表
        private Dictionary<SkillAreaElement, Transform> allElementTrans;
        //技能指示父级
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
        //初始化技能
        private void InitSkill(SkillData data)
        {
            /*
             * 资源映射表
             * 资源名称---->资源完整路径
             */

            //data.skillPrefab = Resources.Load<GameObject>("Prefabs/SkillFX/" + data.prefabName);
            data.skillPrefab = ResourcesManager.Load<GameObject>(data.prefabName);
            data.owner = gameObject;
            data.attackPos = transform.Find("FirePos");
        }
        public SkillData PrepareSkill(int id)
        {
            //根据id 查找技能数据
            SkillData data = skills.Find(s => s.skillId == id);
            //判断条件            返回技能数据
            if (data != null && data.coolRemain <= 0)
                return data;
            else
                return null;
        }
        //生成技能
        public void GenerateSkill(SkillData data, Vector3 deltaVac)
        {
            GameObject skillGo;
            //看向技能方向
            transform.LookAt(deltaVac + transform.position);
            //创建
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
            //传递技能数据
            SkillDeployer deployer = skillGo.GetComponent<SkillDeployer>();
            deployer.SkillData = data;//内部创建算法对象
            deployer.DeploySkill();//内部执行算法对象

            //销毁
            //Destroy(skillGo, data.durationTime);
            GameObjectPool.Instance.CollectObject(skillGo, data.durationTime);
            //冷却
            StartCoroutine(CoolTimeDown(data));
        }
        //技能冷却
        private IEnumerator CoolTimeDown(SkillData data)
        {
            data.coolRemain = data.coolTime;
            while (data.coolRemain > 0)
            {
                yield return new WaitForSeconds(1);
                data.coolRemain--;
            }
        }


        #region 技能区域提示

        /// <summary>
        /// 创建技能区域展示
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
        /// 隐藏元素
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
        /// 每帧更新元素
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
        /// 每帧更新元素位置
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
        /// 获取InnerCircl元素位置
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
        /// 获取Cube,Sector元素朝向
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
        /// 创建技能区域展示元素
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
        /// 获取元素物体
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
        /// 获取元素父对象
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