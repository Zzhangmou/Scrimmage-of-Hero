using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NetWorkFK;

namespace Common
{
    /// <summary>
    /// ��Ϸ��Ҫ���
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
                    updateText.text = "�����½���,������Ϸ";
                    ThreadCrossHelper.Instance.ExecuteOnMainThread(() =>
                    {
                        Destroy(updatePanel);
                        LuaManager.Instance.DoLuaFile("Main");
                    }, 3f);
                }
                else
                {
                    updateText.text = "����ʧ��";
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