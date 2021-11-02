using Common;
using proto;
using Scrimmage.Skill;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
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
        private CharacterSkillSystem skillSystem;

        public bool reverse;

        #region 同步数据
        //上次发送同步信息时间
        private float lastSendSyncTime = 0;
        //同步帧率
        private float syncInterval = 0.1f;
        #endregion
        private void Awake()
        {
            //查找组件
            joysticks = FindObjectsOfType<ETCJoystick>();
            anim = GetComponent<Animator>();
            characterStatus = GetComponent<CharacterStatus>();
            chMotor = GetComponent<CharacterMotor>();
            skillSystem = GetComponent<CharacterSkillSystem>();
        }

        //脚本启用注册事件
        private void OnEnable()
        {
            //注册事件
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
        //脚本禁用注销事件
        private void OnDisable()
        {
            //注销事件
            for (int i = 0; i < joysticks.Length; i++)
            {
                if (joysticks[i] == null) continue;//编译器层面不加会有一个报错(先销毁 后执行)
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
        #region 同步
        private void Update()
        {
            //每自定义帧上传位置信息
            SyncPosUpdate();
        }

        private void SyncPosUpdate()
        {
            if (Time.time - lastSendSyncTime < syncInterval) return;
            lastSendSyncTime = Time.time;
            SyncPosPackage();
        }
        private void SyncPosPackage()
        {
            MsgSyncPos msgSyncPos = new MsgSyncPos();
            msgSyncPos.x = transform.position.x;
            msgSyncPos.y = transform.position.y;
            msgSyncPos.z = transform.position.z;
            msgSyncPos.eulerY = transform.rotation.eulerAngles.y;
            msgSyncPos.statusList.Add(new StatusList()
            {
                statusName = characterStatus.chParams.run,
                status = anim.GetBool(characterStatus.chParams.run)
            });
            msgSyncPos.statusList.Add(new StatusList()
            {
                statusName = characterStatus.chParams.attack01,
                status = anim.GetBool(characterStatus.chParams.attack01)
            });
            msgSyncPos.statusList.Add(new StatusList()
            {
                statusName = characterStatus.chParams.attack02,
                status = anim.GetBool(characterStatus.chParams.attack02)
            });
            NetWorkFK.NetManager.Send(msgSyncPos);
        }
        #endregion

        #region  技能摇杆操作
        private void OnSkillJoystickMove(Vector2 dir)
        {
            //更新技能区域
            skillSystem.UpdateElement(new Vector3(dir.x, 0, dir.y), reverse);
        }

        private void OnSkillJoysticjMoveEnd(string name)
        {
            //隐藏技能区域
            skillSystem.HideElement();
            //释放技能
            OnSkillButtonDown(name);
        }

        private void OnSkillJoystickMoveStart(string name)
        {
            //创建技能区域
            SkillData data = skillSystem.GetSkillData(GetSkillIdWithName(name));
            if (data != null)
            {
                skillSystem.CreateSkillShowArea(data);
            }
        }
        private bool IsAttacking()
        {
            return anim.GetBool(characterStatus.chParams.attack01) || anim.GetBool(characterStatus.chParams.attack02);
        }
        private void OnSkillButtonDown(string name)
        {
            //如果正在攻击 退出
            if (IsAttacking()) return;
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

        #region 移动摇杆操作
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
            if (reverse)
                dir = -dir;
            chMotor.Movement(new Vector3(dir.x, 0, dir.y));
        }
        #endregion
    }
}