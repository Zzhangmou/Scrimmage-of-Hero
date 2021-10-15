using Helper;
using proto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    /// <summary>
    /// ��ҽű�
    /// </summary>
    public class PlayerStatus : CharacterStatus
    {
        public override void Death()
        {
            base.Death();
            //�رղ������
            CallLuaHelper.PanelClose("ControlPanel");
            //����Э��
            MsgDeath msgDeath = new MsgDeath()
            {
                belongCamp = camp,
                deathID = id
            };
            NetWorkFK.NetManager.Send(msgDeath);
            print("���ͽ�ɫ����Э��");
        }
    }
}
