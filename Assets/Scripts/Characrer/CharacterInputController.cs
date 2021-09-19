using Common;
using Scrimmage.Skill;
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
        private CharacterStatus characterStatus;
        private Animator anim;
        private CharacterMotor chMotor;
        private CharacterSkillManager skillManager;

        //ҡ���Ƿ���
        private bool isPressed;
        //ҡ��λ��
        private Vector3 deltaVac;
        //��������
        private float dist;

        private void Awake()
        {
            //�������
            joysticks = FindObjectsOfType<ETCJoystick>();
            characterStatus = GetComponent<CharacterStatus>();
            anim = GetComponent<Animator>();
            chMotor = GetComponent<CharacterMotor>();
            skillManager = GetComponent<CharacterSkillManager>();
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
                skillManager.UpdateElement(dist, deltaVac);
        }

        #region ���ܲ���ҡ��
        private void OnSkillJoystickMoveStart(string name)
        {
            isPressed = true;
            int id = 0;
            switch (name)
            {
                case "AttackJoystick":
                    id = 1001;
                    break;
                case "SkillJoystick":
                    id = 1002;
                    break;
            }
            //׼�����ܷ�Χָʾ
            SkillData data = skillManager.skills.Find(s => s.skillId == id);
            if (data != null)//����
            {
                dist = data.attackDistance;
                skillManager.CreateSkillArea(data);
            }
        }
        private void OnSkillJoystickMoveEnd(string name)
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
            OnSkillButtonDown(name, deltaVac);
            isPressed = false;
            skillManager.HideElement();//����ָʾ��
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
            anim.SetBool(characterStatus.chParams.run, true);
            chMotor.statusName = characterStatus.chParams.run;
            chMotor.status = true;
        }
        private void OnJoystickMoveEnd(string name)
        {
            //���Ŷ���
            anim.SetBool(characterStatus.chParams.run, false);
            chMotor.statusName = characterStatus.chParams.run;
            chMotor.status = false;
        }
        private void OnJoystickMove(Vector2 dir)
        {
            //����ƶ�
            chMotor.Movement(new Vector3(dir.x, 0, dir.y));
        }
        #endregion

        private void OnSkillButtonDown(string name, Vector3 deltaVac)
        {
            int id = 0;
            switch (name)
            {
                case "AttackJoystick":
                    id = 1001;
                    break;
                case "SkillJoystick":
                    id = 1002;
                    break;
            }
            //׼������
            SkillData data = skillManager.PrepareSkill(id);
            if (data != null)//����
                skillManager.GenerateSkill(data, deltaVac);
        }
    }
}