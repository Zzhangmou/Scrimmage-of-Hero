using NetWorkFK;
using proto;
using ProtoBuf;

namespace Helper
{
    /// <summary>
    /// 
    /// </summary>
    public static class MatchHelper
    {
        public static void OnShow()
        {
            NetManager.AddMsgListener("MsgEnterMatch", OnMsgEnterMatch);
            NetManager.AddMsgListener("MsgLeaveMatch", OnMsgLeaveMatch);
        }

        public static void OnClose()
        {
            NetManager.RemoveMsgListener("MsgEnterMatch", OnMsgEnterMatch);
            NetManager.RemoveMsgListener("MsgLeaveMatch", OnMsgLeaveMatch);
        }

        private static void OnMsgLeaveMatch(IExtensible msgBase)
        {
            MsgLeaveMatch msg = (MsgLeaveMatch)msgBase;
            UpdateMatch(msg.currentMatchNum, msg.allMatchNum);
        }

        private static void OnMsgEnterMatch(IExtensible msgBase)
        {
            MsgEnterMatch msg = (MsgEnterMatch)msgBase;
            UpdateMatch(msg.currentMatchNum, msg.allMatchNum);
        }
        private static void UpdateMatch(int currentNum, int allNum)
        {
            string text = currentNum + "/" + allNum;
            CallLuaHelper.UpdateText(text);
        }
    }
}