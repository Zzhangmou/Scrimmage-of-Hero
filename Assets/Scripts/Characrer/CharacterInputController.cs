using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns
{
    /// <summary>
    /// ��ɫ���������
    /// </summary>
    public class CharacterInputController : MonoBehaviour
    {
        private ETCJoystick[] joysticks;
        private PlayerStatus playerStatus;
        private Animator anim;
        private CharacterMotor chMotor;

        private bool isPressed;
        private Vector3 deltaVac;
        private Dictionary<SkillAreaElement, Transform> allElementTrans;
        private Transform elementParent;

        float outerRadius = 4f;      // ��Բ�뾶
        float innerRadius = 2f;     // ��Բ�뾶
        float cubeWidth = 2f;       // ���ο�� �����γ���ʹ�õ���Բ�뾶��
        private void Awake()
        {
            //�������
            joysticks = FindObjectsOfType<ETCJoystick>();
            playerStatus = GetComponent<PlayerStatus>();
            anim = GetComponent<Animator>();
            chMotor = GetComponent<CharacterMotor>();
        }
        private void OnEnable()
        {
            for (int i = 0; i < joysticks.Length; i++)
            {
                //ע���¼�
                if (joysticks[i].name == "MainJoystick")
                {
                    joysticks[i].onMove.AddListener(OnJoystickMove);
                    joysticks[i].onMoveStart.AddListener(OnJoystickMoveStart);
                    joysticks[i].onMoveEnd.AddListener(OnJoystickMoveEnd);
                }
                else
                {
                    joysticks[i].onMove.AddListener(OnSkillJoystickMove);
                    joysticks[i].onMoveStart.AddListener(OnSkillJoystickMoveStart);
                    joysticks[i].onMoveEnd.AddListener(OnSkillJoystickMoveEnd);
                }
            }

            allElementTrans = new Dictionary<SkillAreaElement, Transform>();
            allElementTrans.Add(SkillAreaElement.OuterCircle, null);
            allElementTrans.Add(SkillAreaElement.Rectangle, null);
            allElementTrans.Add(SkillAreaElement.InnerCircle, null);
            allElementTrans.Add(SkillAreaElement.Sector120, null);
            allElementTrans.Add(SkillAreaElement.Sector60, null);
        }
        private void OnDisable()
        {
            for (int i = 0; i < joysticks.Length; i++)
            {
                //ע���¼�   
                if (joysticks[i].name == "MainJoystick")
                {
                    joysticks[i].onMove.RemoveListener(OnJoystickMove);
                    joysticks[i].onMoveStart.RemoveListener(OnJoystickMoveStart);
                    joysticks[i].onMoveEnd.RemoveListener(OnJoystickMoveEnd);
                }
                else
                {
                    joysticks[i].onMove.RemoveListener(OnJoystickMove);
                    joysticks[i].onMoveStart.RemoveListener(OnJoystickMoveStart);
                    joysticks[i].onMoveEnd.RemoveListener(OnJoystickMoveEnd);
                }
            }
        }
        private void LateUpdate()
        {
            if (isPressed)
                UpdateElement();
        }

        #region ���ܲ���ҡ��
        private void OnSkillJoystickMoveStart(string name)
        {
            isPressed = true;
            switch (name)
            {
                case "AttackJoystick":
                    CreateSkillArea(SkillAreaType.OuterCircle_InnerRectangle);
                    break;
                case "SkillJoystick":
                    CreateSkillArea(SkillAreaType.OuterCircle_InnerCircle);
                    break;
            }
        }
        private void OnSkillJoystickMoveEnd()
        {
            switch (name)
            {
                case "AttackJoystick":
                    Debug.Log("��������̧��");
                    break;
                case "SkillJoystick":
                    Debug.Log("���ܼ�̧��");
                    break;
            }
            isPressed = false;
            HideElement();//����ָʾ��
        }
        private void OnSkillJoystickMove(Vector2 dir)
        {
            deltaVac = new Vector3(dir.x, 0, dir.y);
        }
        #endregion

        #region �ƶ�����ҡ��
        private void OnJoystickMoveStart(string name)
        {
            //���Ŷ���
            anim.SetBool(playerStatus.chParams.run, true);
        }
        private void OnJoystickMoveEnd()
        {
            //���Ŷ���
            anim.SetBool(playerStatus.chParams.run, false);
        }
        private void OnJoystickMove(Vector2 dir)
        {
            //����ƶ�
            chMotor.Movement(new Vector3(dir.x, 0, dir.y));
        }
        #endregion

        /// <summary>
        /// ÿ֡����Ԫ��
        /// </summary>
        private void UpdateElement()
        {
            UpdaElementPosition(SkillAreaElement.Rectangle);
            UpdaElementPosition(SkillAreaElement.InnerCircle);
            UpdaElementPosition(SkillAreaElement.Sector60);
            UpdaElementPosition(SkillAreaElement.Sector120);
        }
        /// <summary>
        /// ÿ֡����Ԫ��λ��
        /// </summary>
        /// <param name="cube"></param>
        private void UpdaElementPosition(SkillAreaElement element)
        {
            if (allElementTrans[element] == null) return;
            switch (element)
            {
                case SkillAreaElement.OuterCircle:
                    break;
                case SkillAreaElement.InnerCircle:
                    allElementTrans[element].transform.position = GetCirclePosition(outerRadius);
                    break;
                case SkillAreaElement.Rectangle:
                case SkillAreaElement.Sector60:
                case SkillAreaElement.Sector120:
                    allElementTrans[element].transform.LookAt(GetCubeSectorLookAt());
                    break;
            }
        }
        /// <summary>
        /// ��ȡInnerCirclԪ��λ��
        /// </summary>
        /// <param name="outerRadius"></param>
        /// <returns></returns>
        private Vector3 GetCirclePosition(float dist)
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
        private Vector3 GetCubeSectorLookAt()
        {
            Vector3 targetDir = deltaVac;
            float y = Camera.main.transform.rotation.eulerAngles.y;
            targetDir = Quaternion.Euler(0, y, 0) * targetDir;
            return targetDir + this.transform.position;
        }
        /// <summary>
        /// ����Ԫ��
        /// </summary>
        private void HideElement()
        {
            if (elementParent == null) return;
            Transform parent = GetParent();
            for (int i = 0, length = parent.childCount; i < length; i++)
            {
                parent.GetChild(i).gameObject.SetActive(false);
            }
        }
        /// <summary>
        /// ������������չʾ
        /// </summary>
        /// <param name="areaType"></param>
        private void CreateSkillArea(SkillAreaType areaType)
        {
            switch (areaType)
            {
                case SkillAreaType.OuterCircle:
                    CreateElement(SkillAreaElement.OuterCircle);
                    break;
                case SkillAreaType.OuterCircle_InnerRectangle:
                    //CreateElement(SkillAreaElement.OuterCircle);
                    CreateElement(SkillAreaElement.Rectangle);
                    break;
                case SkillAreaType.OuterCircle_InnerSector:
                    CreateElement(SkillAreaElement.OuterCircle);
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
        /// ������������չʾԪ��
        /// </summary>
        /// <param name="skillAreaElement"></param>
        private void CreateElement(SkillAreaElement skillAreaElement)
        {
            Transform elementTrans = GetElement(skillAreaElement);
            if (elementTrans == null) return;
            allElementTrans[skillAreaElement] = elementTrans;
            switch (skillAreaElement)
            {
                case SkillAreaElement.OuterCircle:
                    elementTrans.localScale = new Vector3(outerRadius * 2, 1, outerRadius * 2) / this.transform.localScale.x;
                    elementTrans.gameObject.SetActive(true);
                    break;
                case SkillAreaElement.InnerCircle:
                    elementTrans.localScale = new Vector3(innerRadius * 2, 1, innerRadius * 2) / this.transform.localScale.x;
                    elementTrans.gameObject.SetActive(true);
                    break;
                case SkillAreaElement.Rectangle:
                    elementTrans.localScale = new Vector3(cubeWidth, 1, outerRadius) / this.transform.localScale.x;
                    elementTrans.gameObject.SetActive(true);
                    break;
                case SkillAreaElement.Sector120:
                    elementTrans.localScale = new Vector3(outerRadius, 1, outerRadius) / this.transform.localScale.x;
                    elementTrans.gameObject.SetActive(true);
                    break;
                case SkillAreaElement.Sector60:
                    elementTrans.localScale = new Vector3(outerRadius, 1, outerRadius) / this.transform.localScale.x;
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
                GameObject elementGo = Instantiate(Resources.Load<GameObject>("Effect/Prefabs/" + name));
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

        private void OnSkillButtonDown(string name)
        {
            int id = 0;
            switch (name)
            {
                case "BaseButton":
                    id = 1001;
                    break;
                case "SkillButton01":
                    id = 1002;
                    break;
            }
            CharacterSkillManager skillManager = GetComponent<CharacterSkillManager>();
            //׼������
            SkillData data = skillManager.PrepareSkill(id);
            if (data != null)//����
                skillManager.GenerateSkill(data);
        }
    }
}