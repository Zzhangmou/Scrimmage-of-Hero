using Helper;
using proto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    /// <summary>
    /// 玩家脚本
    /// </summary>
    public class PlayerStatus : CharacterStatus
    {
        public override void Death()
        {
            base.Death();
            //关闭操作面板
            CallLuaHelper.PanelClose("ControlPanel");
            //发送协议
            MsgDeath msgDeath = new MsgDeath()
            {
                belongCamp = camp,
                deathID = id
            };
            NetWorkFK.NetManager.Send(msgDeath);
            print("发送角色死亡协议");
        }
    }
}
