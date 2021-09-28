using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using NetWorkFK;
using proto;
using Helper;
using XLua;
using ProtoBuf;
using System;
using Character;
using Scrimmage.Skill;

namespace ns
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public static Dictionary<string, GameObject> heros;

        public override void Init()
        {
            base.Init();
            heros = new Dictionary<string, GameObject>();

            NetManager.AddMsgListener("MsgGetRoomInfo", OnMsgGetRoomInfo);
            NetManager.AddMsgListener("MsgPrepared", OnMsgPrepared);
            NetManager.AddMsgListener("MsgStartGame", OnMsgStartGame);

            NetManager.AddMsgListener("MsgSyncPos", OnMsgSyncPos);
        }

        private void OnMsgSyncPos(IExtensible msgBase)
        {
            MsgSyncPos msg = (MsgSyncPos)msgBase;

            if (msg.id == GameMain.Instance.id) return;
            GameObject currentHero = GetHero(msg.id);
            CharacterSyncMotor motor = currentHero.GetComponent<CharacterSyncMotor>();
            motor.targetPos = new Vector3(msg.x, msg.y, msg.z);
            motor.targetRot = new Vector3(0, msg.eulerY, 0);
            motor.statusName = msg.statusName;
            motor.status = msg.status;
            motor.forecastTime = Time.time;
        }

        public static GameObject GetHero(string id)
        {
            if (heros.ContainsKey(id))
                return heros[id];
            return null;
        }

        private static void OnMsgStartGame(IExtensible msgBase)
        {
            CallLuaHelper.PanelClose("ProgressPanel");
            CallLuaHelper.PanelClose("MatchPanel");
            CallLuaHelper.PanelClose("GameMainPanel");
        }

        private static void OnMsgPrepared(IExtensible msgBase)
        {
            MsgPrepared msg = (MsgPrepared)msgBase;
            float num = msg.currentNum / msg.maxNum;
            CallLuaHelper.ChangeSliderValue(num);
        }

        //开始生成场景 人物模型
        private static void OnMsgGetRoomInfo(IExtensible msgBase)
        {
            int redNum = 1;
            int blueNum = 1;
            CallLuaHelper.PanelShow("ProgressPanel");

            //ab包不能直接打包预制体及脚本 暂时直接生成
            GameObject controlPanel = Instantiate(ResourcesManager.Load<GameObject>("ControlPanel"));
            controlPanel.transform.SetParent(GameObject.Find("Canvas/PanelLayer").transform, false);

            MsgGetRoomInfo msg = (MsgGetRoomInfo)msgBase;
            //生成场景   mapId
            GameObject GameMap = ResourcesManager.Load<GameObject>("Grass");
            Instantiate(GameMap, GameMap.transform.position, GameMap.transform.rotation);
            //获取生成点
            Transform[] wayPointA = GameMap.transform.Find("RestartA").transform.GetComponentsInChildren<Transform>();
            Transform[] wayPointB = GameMap.transform.Find("RestartB").transform.GetComponentsInChildren<Transform>();
            //获取人物模型数据映射表
            LuaTable heroTable = LuaManager.Instance.Global.Get<LuaTable>("heroiconDataList");
            //生成模型
            GameObject go = null;
            //玩家控制角色 
            string gameMainId = FindObjectOfType<GameMain>().id;
            //相机

            for (int i = 0; i < msg.players.Count; i++)
            {
                LuaTable heroId = heroTable.Get<int, LuaTable>(msg.players[i].heroId);
                string heroName = heroId.Get<string, string>("name");
                GameObject heroGo = ResourcesManager.Load<GameObject>(heroName);

                if (msg.players[i].camp == 1)
                {
                    go = CharacterInitConfigFactory.CreateCharacter(heroGo, wayPointA[redNum], msg.players[i].heroId, msg.players[i].id == gameMainId);
                    redNum++;
                }
                if (msg.players[i].camp == 2)
                {
                    go = CharacterInitConfigFactory.CreateCharacter(heroGo, wayPointB[blueNum], msg.players[i].heroId, msg.players[i].id == gameMainId);
                    blueNum++;
                }
                heros.Add(msg.players[i].id, go);
            }

            //生成完毕 发送准备完毕协议
            NetManager.Send(new MsgPrepared());
        }
    }
}