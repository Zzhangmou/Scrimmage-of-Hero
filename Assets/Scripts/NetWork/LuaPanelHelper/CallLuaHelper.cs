using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Helper
{
    [CSharpCallLua]
    /// <summary>
    /// 用接口模拟lua类
    /// </summary>
    public interface ICallPanel
    {
        void Show();
        void Show(string showText);
        void Close();
    }

    public static class CallLuaHelper
    {
        public static void PanelClose(string panelName)
        {
            ICallPanel panel = LuaManager.Instance.Global.Get<ICallPanel>(panelName);
            panel.Close();
        }

        public static void PanelShow(string panelName)
        {
            ICallPanel panel = LuaManager.Instance.Global.Get<ICallPanel>(panelName);
            panel.Show();
        }
        public static void PanelShowTip(string showText)
        {
            ICallPanel panel = LuaManager.Instance.Global.Get<ICallPanel>("TipPanel");
            panel.Show(showText);
        }
    }
}

