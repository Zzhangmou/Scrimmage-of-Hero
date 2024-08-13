using Common;
using System.Collections.Generic;
using XLua;
using UnityEngine;

namespace Helper
{
    public static class XLuaConfig
    {
        [CSharpCallLua]
        public static List<System.Type> CSharpCallLua = new List<System.Type>()
        {
            typeof(UnityEngine.Events.UnityAction<UnityEngine.Vector2>)
             // 在这里添加其他需要的委托类型
        };
    }
    /// <summary>
    /// 用接口模拟lua类
    /// </summary>
    [CSharpCallLua]
    public interface ICallPanel
    {
        //ViewManager Open
        void OpenView(string panelName);
        //TipCtrl
        void ShowMessage(string message);
        //ViewManager Close
        void CloseView(string panelName);
        //progressCtrl
        void ShowHero(string heroId);
        void ChangeSliderValue(float value);
        //ShowMainCtrl
        void SetUserInfo(string userName, string userRecord);
        //MatchCtrl
        void UpdateText(string text);

        //BattleMessageCtrl
        void InitBattleMessage(int camp, int heroId, string id);
        void FlushData(int camp, string id);
    }
    public static class CallLuaHelper
    {
        private static ICallPanel panel = LuaManager.Instance.Global.Get<ICallPanel>("CallLuaMethod");
        public static void PanelClose(string panelName)
        {
            Debug.Log("PanelClose " + panelName);
            panel.CloseView(panelName);
        }

        public static void PanelShow(string panelName)
        {
            Debug.Log("PanelShow " + panelName);
            panel.OpenView(panelName);
        }
        public static void ShowMessage(string message)
        {
            panel.ShowMessage(message);
        }
        public static void ShowHero(string heroId)
        {
            panel.ShowHero(heroId);
        }
        public static void SetUserInfo(string userName, string userRecord)
        {
            panel.SetUserInfo(userName, userRecord);
        }
        public static void UpdateText(string text)
        {
            panel.UpdateText(text);
        }
        public static void ChangeSliderValue(float value)
        {
            panel.ChangeSliderValue(value);
        }

        public static void InitBattleMessage(int camp, int heroId, string id)
        {
            panel.InitBattleMessage(camp, heroId, id);
        }

        public static void FlushData(int camp, string id)
        {
            panel.FlushData(camp, id);
        }
    }
}

