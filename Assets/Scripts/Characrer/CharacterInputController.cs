using Common;
using Scrimmage.Skill;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
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
        private CharacterSkillSystem skillSystem;

        private void Awake()
        {
            //�������
            joysticks = FindObjectsOfType<ETCJoystick>();
            anim = GetComponent<Animator>();
            characterStatus = GetComponent<CharacterStatus>();
            chMotor = GetComponent<CharacterMotor>();
            skillSystem = GetComponent<CharacterSkillSystem>();
        }

        //�ű�����ע���¼�
        private void OnEnable()
        {
            //ע���¼�
            for (int i = 0; i < joysticks.Length; i++)
            {
                if (joysticks[i].name == "MainJoystick")
                {
                    joysticks[i].onMove.AddListener(OnMainJoystickMove);
                    joysticks[i].onMoveStart.AddListener(OnMainJoystickMoveStart);
                    joysticks[i].onMoveEnd.AddListener(OnMainJoystickMoveEnd);
                }
                else
                {
                    joysticks[i].onMoveStart.AddListener(OnSkillJoystickMoveStart);
                    joysticks[i].onMoveEnd.AddListener(OnSkillJoysticjMoveEnd);
                    joysticks[i].onMove.AddListener(OnSkillJoystickMove);
                }
            }
        }
        //�ű�����ע���¼�
        private void OnDisable()
        {
            //ע���¼�
            for (int i = 0; i < joysticks.Length; i++)
            {
                if (joysticks[i].name == "MainJoystick")
                {
                    joysticks[i].onMove.RemoveListener(OnMainJoystickMove);
                    joysticks[i].onMoveStart.RemoveListener(OnMainJoystickMoveStart);
                    joysticks[i].onMoveEnd.RemoveListener(OnMainJoystickMoveEnd);
                }
                else
                {
                    joysticks[i].onMoveStart.RemoveListener(OnSkillJoystickMoveStart);
                    joysticks[i].onMoveEnd.RemoveListener(OnSkillJoysticjMoveEnd);
                    joysticks[i].onMove.RemoveListener(OnSkillJoystickMove);
                }
            }
        }
        #region  ����ҡ�˲���
        private void OnSkillJoystickMove(Vector2 dir)
        {
            //���¼�������
            skillSystem.UpdateElement(new Vector3(dir.x, 0, dir.y));
        }

        private void OnSkillJoysticjMoveEnd(string name)
        {
            //���ؼ�������
            skillSystem.HideElement();
            //�ͷż���
            OnSkillButtonDown(name);
        }

        private void OnSkillJoystickMoveStart(string name)
        {
            //������������
            SkillData data = skillSystem.GetSkillData(GetSkillIdWithName(name));
            if (data != null)
            {
                skillSystem.CreateSkillShowArea(data);
            }
        }
        
        private void OnSkillButtonDown(string name)
        {
            int id = GetSkillIdWithName(name);
            skillSystem.AttackUseSkill(id);
        }

        private int GetSkillIdWithName(string name)
        {
            switch (name)
            {
                case "AttackJoystick":
                    return 1001;
                case "SkillJoystick":
                    return 1002;
            }

            return 0;
        }
        #endregion

        #region �ƶ�ҡ�˲���
        private void OnMainJoystickMoveEnd(string arg0)
        {
            anim.SetBool(characterStatus.chParams.run, false);
        }

        private void OnMainJoystickMoveStart(string arg0)
        {
            anim.SetBool(characterStatus.chParams.run, true);
        }

        private void OnMainJoystickMove(Vector2 dir)
        {
            chMotor.Movement(new Vector3(dir.x, 0, dir.y));
        }
        #endregion
    }
}