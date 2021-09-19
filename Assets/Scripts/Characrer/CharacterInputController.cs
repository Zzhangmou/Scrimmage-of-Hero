using Common;
using Scrimmage.Skill;
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
        private ETCJoystick[] joysticks;
        private CharacterStatus characterStatus;
        private Animator anim;
        private CharacterMotor chMotor;
        private CharacterSkillManager skillManager;

        //摇杆是否按下
        private bool isPressed;
        //摇杆位置
        private Vector3 deltaVac;
        //攻击距离
        private float dist;

        private void Awake()
        {
            //查找组件
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
                //注册事件
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
                //注销事件   
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

        #region 技能操作摇杆
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
            //准备技能范围指示
            SkillData data = skillManager.skills.Find(s => s.skillId == id);
            if (data != null)//生成
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
                    Debug.Log("攻击技能抬起");
                    break;
                case "SkillJoystick":
                    Debug.Log("技能键抬起");
                    break;
            }
            OnSkillButtonDown(name, deltaVac);
            isPressed = false;
            skillManager.HideElement();//隐藏指示器
        }
        private void OnSkillJoystickMove(Vector2 dir)
        {
            deltaVac = new Vector3(dir.x, 0, dir.y);
        }
        #endregion

        #region 移动操作摇杆
        private void OnJoystickMoveStart(string name)
        {
            //播放动画
            anim.SetBool(characterStatus.chParams.run, true);
            chMotor.statusName = characterStatus.chParams.run;
            chMotor.status = true;
        }
        private void OnJoystickMoveEnd(string name)
        {
            //播放动画
            anim.SetBool(characterStatus.chParams.run, false);
            chMotor.statusName = characterStatus.chParams.run;
            chMotor.status = false;
        }
        private void OnJoystickMove(Vector2 dir)
        {
            //马达移动
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
            //准备技能
            SkillData data = skillManager.PrepareSkill(id);
            if (data != null)//生成
                skillManager.GenerateSkill(data, deltaVac);
        }
    }
}