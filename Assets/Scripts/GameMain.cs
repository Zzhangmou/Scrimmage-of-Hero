using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NetWorkFK;

namespace Common
{
    /// <summary>
    /// 游戏主要入口
    /// </summary>
    public class GameMain : MonoSingleton<GameMain>
    {
        public string id = "";

        public GameObject updatePanel;
        public Text updateText;

        public override void Init()
        {
            base.Init();
            updatePanel.SetActive(true);
            AbUpdateManager.Instance.CheckUpdate((isOver) =>
            {
                if (isOver)
                {
                    updateText.text = "检查更新结束,进入游戏";
                    ThreadCrossHelper.Instance.ExecuteOnMainThread(() =>
                    {
                        Destroy(updatePanel);
                        LuaManager.Instance.DoLuaFile("Main");
                    }, 3f);
                }
                else
                {
                    updateText.text = "更新失败";
                }
            }, (result) =>
            {
                updateText.text = result;
            });
            //LuaManager.Instance.DoLuaFile("Main");
            //NetManager.Connect("127.0.0.1", 18188);
            //NetManager.Connect("server.natappfree.cc", 44553);
        }

        private void Update()
        {
            NetManager.Update();
        }
    }
}