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
        private ETCJoystick joystick;
        private ETCButton[] skillButtons;
        private PlayerStatus playerStatus;
        private Animator anim;
        private CharacterMotor chMotor;
        private void Awake()
        {
            //�������
            joystick = FindObjectOfType<ETCJoystick>();
            skillButtons = FindObjectsOfType<ETCButton>();
            playerStatus = GetComponent<PlayerStatus>();
            anim = GetComponent<Animator>();
            chMotor = GetComponent<CharacterMotor>();
        }
        private void OnEnable()
        {
            //ע���¼�
            joystick.onMove.AddListener(OnJoystickMove);
            joystick.onMoveStart.AddListener(OnJoystickMoveStart);
            joystick.onMoveEnd.AddListener(OnJoystickMoveEnd);

            for(int i = 0; i < skillButtons.Length; i++)
            {
                skillButtons[i].onDown.AddListener(OnSkillButtonDown);
            }
        }

        private void OnDisable()
        {
            //ע���¼�   
            joystick.onMove.RemoveListener(OnJoystickMove);
            joystick.onMoveStart.RemoveListener(OnJoystickMoveStart);
            joystick.onMoveEnd.RemoveListener(OnJoystickMoveEnd);

            for (int i = 0; i < skillButtons.Length; i++)
            {
                skillButtons[i].onDown.RemoveListener(OnSkillButtonDown);
            }
        }
        private void OnSkillButtonDown()
        {
            Debug.Log("��ť����");
        }

        private void OnJoystickMoveStart()
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
    }
}