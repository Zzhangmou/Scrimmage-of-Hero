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
    /// ��ɫ���������
    /// </summary>
    public class CharacterInputController : MonoBehaviour
    {
        private ETCJoystick[] joysticks;
        private CharacterStatus characterStatus;
        private Animator anim;
        private CharacterMotor chMotor;
        private CharacterSkillSystem skillSystem;

        public bool reverse;

        #region ͬ������
        //�ϴη���ͬ����Ϣʱ��
        private float lastSendSyncTime = 0;
        //ͬ��֡��
        private float syncInterval = 0.1f;
        #endregion
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
                if (joysticks[i] == null) continue;//���������治�ӻ���һ������(������ ��ִ��)
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
        #region ͬ��
        private void Update()
        {
            //ÿ�Զ���֡�ϴ�λ����Ϣ
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

        #region  ����ҡ�˲���
        private void OnSkillJoystickMove(Vector2 dir)
        {
            //���¼�������
            skillSystem.UpdateElement(new Vector3(dir.x, 0, dir.y), reverse);
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
        private bool IsAttacking()
        {
            return anim.GetBool(characterStatus.chParams.attack01) || anim.GetBool(characterStatus.chParams.attack02);
        }
        private void OnSkillButtonDown(string name)
        {
            //������ڹ��� �˳�
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
            if (reverse)
                dir = -dir;
            chMotor.Movement(new Vector3(dir.x, 0, dir.y));
        }
        #endregion
    }
}