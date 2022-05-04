using Helper;
using proto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using Scrimmage;

namespace Character
{
    /// <summary>
    /// ��ҽű�
    /// </summary>
    public class PlayerStatus : CharacterStatus
    {
        private bool isDeathSend = false;
        public override void Death()
        {
            base.Death();
            if (isDeathSend) return;
            //�رղ������
            CallLuaHelper.PanelClose("ControlPanel");
            //ֹͣ����ͬ��λ��
            GetComponent<CharacterMotor>().enabled = false;
            //����Э��
            MsgDeath msgDeath = new MsgDeath()
            {
                belongCamp = camp,
                deathID = id
            };
            NetWorkFK.NetManager.Send(msgDeath);
            GameManager.Instance.teamDatas.Remove(id);
            if (GameManager.Instance.teamDatas.Count > 0)
            {
                foreach (var item in GameManager.Instance.teamDatas.Values)
                {
                    CameraFollow.Instance.ChangeTarget(item);
                    return;
                }
            }
            isDeathSend = true;
        }
    }
}
