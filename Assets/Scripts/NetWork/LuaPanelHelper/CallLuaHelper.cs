using Common;
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
        void Show(string showText);
        void Close();
        void SetUserInfo(string userName, string userRecord);
        void UpdateText(string text);
    }

    public static class CallLuaHelper
    {
        private static ICallPanel panel;
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
        public static void PanelShowTip(string showText)
        {
            panel = LuaManager.Instance.Global.Get<ICallPanel>("TipPanel");
            panel.Show(showText);
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
    }
}

