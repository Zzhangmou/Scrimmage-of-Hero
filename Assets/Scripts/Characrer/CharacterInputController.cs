using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns
{
    /// <summary>
    /// 角色输入控制器
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
            //查找组件
            joystick = FindObjectOfType<ETCJoystick>();
            skillButtons = FindObjectsOfType<ETCButton>();
            playerStatus = GetComponent<PlayerStatus>();
            anim = GetComponent<Animator>();
            chMotor = GetComponent<CharacterMotor>();
        }
        private void OnEnable()
        {
            //注册事件
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
            //注销事件   
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
            Debug.Log("按钮按下");
        }

        private void OnJoystickMoveStart()
        {
            //播放动画
            anim.SetBool(playerStatus.chParams.run, true);
        }

        private void OnJoystickMoveEnd()
        {
            //播放动画
            anim.SetBool(playerStatus.chParams.run, false);
        }

        private void OnJoystickMove(Vector2 dir)
        {
            //马达移动
            chMotor.Movement(new Vector3(dir.x, 0, dir.y));
        }
    }
}