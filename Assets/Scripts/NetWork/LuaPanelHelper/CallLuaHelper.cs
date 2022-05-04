using Common;
using proto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Helper
{
    /// <summary>
    /// 用接口模拟lua类
    /// </summary>
    [CSharpCallLua]
    public interface ICallPanel
    {
        void Show();
        void Show(string message);
        void Close();
        void SetUserInfo(string userName, string userRecord);
        void UpdateText(string text);
        void ChangeSliderValue(float value);
    }
    [CSharpCallLua]
    public interface ICallBattleMessagePanel
    {
        void InitBattleMessage(int camp,int heroId,string id);
        void FlushData(int camp,string id);
    }
    public static class CallLuaHelper
    {
        private static ICallPanel panel;
        private static ICallBattleMessagePanel battleMessagePanel;
        public static void PanelClose(string panelName)
        {
            panel = LuaManager.Instance.Global.Get<ICallPanel>(panelName);
            panel.Close();
        }

        public static void PanelShow(string panelName)
        {
            panel = LuaManager.Instance.Global.Get<ICallPanel>(panelName);
            panel.Show();
        }
        public static void PanelShow(string panelName,string message)
        {
            panel = LuaManager.Instance.Global.Get<ICallPanel>(panelName);
            panel.Show(message);
        }
        public static void SetUserInfo(string userName, string userRecord)
        {
            panel = LuaManager.Instance.Global.Get<ICallPanel>("GameMainPanel");
            panel.SetUserInfo(userName, userRecord);
        }
        public static void UpdateText(string text)
        {
            panel = LuaManager.Instance.Global.Get<ICallPanel>("MatchPanel");
            panel.UpdateText(text);
        }
        public static void ChangeSliderValue(float value)
        {
            panel = LuaManager.Instance.Global.Get<ICallPanel>("ProgressPanel");
            panel.ChangeSliderValue(value);
        }

        public static void InitBattleMessage(int camp, int heroId, string id)
        {
            battleMessagePanel = LuaManager.Instance.Global.Get<ICallBattleMessagePanel>("BattleMessagePanel");
            battleMessagePanel.InitBattleMessage(camp, heroId, id);
        }

        public static void FlushData(int camp,string id)
        {
            battleMessagePanel = LuaManager.Instance.Global.Get<ICallBattleMessagePanel>("BattleMessagePanel");
            battleMessagePanel.FlushData(camp, id);
        }
    }
}

