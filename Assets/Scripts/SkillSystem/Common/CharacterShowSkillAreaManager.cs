using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scrimmage.Skill
{
    /// <summary>
    /// 角色技能区域显示管理器
    /// </summary>
    public class CharacterShowSkillAreaManager : MonoBehaviour
    {
        private SkillData data;
        Dictionary<SkillAreaElement, Transform> allElementTransDic;
        private enum SkillAreaElement
        {
            /// <summary>
            /// 外园
            /// </summary>
            OuterCircle,
            /// <summary>
            /// 内圆
            /// </summary>
            InnerCircle,
            /// <summary>
            /// 矩形
            /// </summary>
            Rectangle,
            /// <summary>
            /// 扇形60
            /// </summary>
            Sector60,
            /// <summary>
            /// 扇形120
            /// </summary>
            Sector120,
        }

        private void Start()
        {
            allElementTransDic = new Dictionary<SkillAreaElement, Transform>();
            allElementTransDic.Add(SkillAreaElement.OuterCircle, null);
            allElementTransDic.Add(SkillAreaElement.InnerCircle, null);
            allElementTransDic.Add(SkillAreaElement.Rectangle, null);
            allElementTransDic.Add(SkillAreaElement.Sector60, null);
            allElementTransDic.Add(SkillAreaElement.Sector120, null);
        }
        /// <summary>
        /// 创建技能展示区域
        /// </summary>
        /// <param name="data"></param>
        public void CreateSkillShowArea(SkillData data)
        {
            this.data = data;
            switch (data.areaType)
            {
                case SkillAreaType.OuterCircle:
                    CreateElement(SkillAreaElement.OuterCircle);
                    break;
                case SkillAreaType.OuterCircle_InnerRectangle:
                    CreateElement(SkillAreaElement.Rectangle);
                    break;
                case SkillAreaType.OuterCircle_InnerSector60:

                    CreateElement(SkillAreaElement.Sector60);
                    break;
                case SkillAreaType.OuterCircle_InnerSector120:
                    CreateElement(SkillAreaElement.Sector120);
                    break;
                case SkillAreaType.OuterCircle_InnerCircle:
                    CreateElement(SkillAreaElement.OuterCircle);
                    CreateElement(SkillAreaElement.InnerCircle);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 创建技能区域展示元素
        /// </summary>
        /// <param name="skillAreaElement"></param>
        private void CreateElement(SkillAreaElement skillAreaElement)
        {
            Transform elementTrans = GetElement(skillAreaElement);
            if (elementTrans == null) return;
            allElementTransDic[skillAreaElement] = elementTrans;
            switch (skillAreaElement)
            {
                case SkillAreaElement.OuterCircle:
                    elementTrans.localScale = new Vector3(data.attackScope, 1, data.attackScope);
                    elementTrans.gameObject.SetActive(true);
                    break;
                case SkillAreaElement.InnerCircle:
                    elementTrans.localScale = new Vector3(data.attackDistance, 1, data.attackDistance);
                    break;
                case SkillAreaElement.Rectangle:
                    elementTrans.localScale = new Vector3(1, 1, data.attackDistance);
                    elementTrans.gameObject.SetActive(true);
                    break;
                case SkillAreaElement.Sector120:
                    elementTrans.localScale = new Vector3(data.attackDistance, 1, data.attackDistance);
                    break;
                case SkillAreaElement.Sector60:
                    elementTrans.localScale = new Vector3(data.attackDistance, 1, data.attackDistance);
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
            GameObject elementGo = ResourcesManager.Load<GameObject>(name);
            elementGo = GameObjectPool.Instance.CreateObject(name, elementGo, transform.position, transform.rotation);
            elementGo.transform.parent = this.transform;
            return elementGo.transform;
        }

        /// <summary>
        /// 更新元素位置
        /// </summary>
        /// <param name="JoyStickPos">摇杆位置</param>
        public void UpdateElement(Vector3 JoyStickPos)
        {
            UpdaElementPosition(SkillAreaElement.OuterCircle, JoyStickPos);
            UpdaElementPosition(SkillAreaElement.Rectangle, JoyStickPos);
            UpdaElementPosition(SkillAreaElement.InnerCircle, JoyStickPos);
            UpdaElementPosition(SkillAreaElement.Sector60, JoyStickPos);
            UpdaElementPosition(SkillAreaElement.Sector120, JoyStickPos);
        }
        private void UpdaElementPosition(SkillAreaElement element, Vector3 JoyStickPos)
        {
            if (allElementTransDic[element] == null) return;
            switch (element)
            {
                case SkillAreaElement.OuterCircle:
                    break;
                case SkillAreaElement.InnerCircle:
                    allElementTransDic[element].transform.position = GetCirclePosition(JoyStickPos);
                    break;
                case SkillAreaElement.Rectangle:
                case SkillAreaElement.Sector60:
                case SkillAreaElement.Sector120:
                    allElementTransDic[element].transform.LookAt(GetRectSectorLookAt(JoyStickPos));
                    break;
            }
        }
        private Vector3 GetCirclePosition(Vector3 JoyStickPos)
        {
            Vector3 targetDir = JoyStickPos * data.attackScope / 2;
            float y = Camera.main.transform.rotation.eulerAngles.y;
            targetDir = Quaternion.Euler(0, y, 0) * targetDir;
            return targetDir + this.transform.position;
        }
        private Vector3 GetRectSectorLookAt(Vector3 JoyStickPos)
        {
            Vector3 targetDir = JoyStickPos;
            float y = Camera.main.transform.rotation.eulerAngles.y;
            targetDir = Quaternion.Euler(0, y, 0) * targetDir;
            return targetDir + this.transform.position;
        }
        //隐藏技能展示区域
        public void HideSkillShowArea()
        {
            foreach (var key in allElementTransDic.Keys)
            {
                if (allElementTransDic[key] != null)
                    GameObjectPool.Instance.CollectObject(allElementTransDic[key].gameObject);
            }
        }
    }
}