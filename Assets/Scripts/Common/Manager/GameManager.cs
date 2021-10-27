using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetWorkFK;
using proto;
using Helper;
using XLua;
using ProtoBuf;
using Character;
using System;

namespace Common
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public Dictionary<string, GameObject> gameDatas;

        public override void Init()
        {
            base.Init();
            gameDatas = new Dictionary<string, GameObject>();

            NetManager.AddMsgListener("MsgGetRoomInfo", OnMsgGetRoomInfo);
            NetManager.AddMsgListener("MsgPrepared", OnMsgPrepared);
            NetManager.AddMsgListener("MsgStartGame", OnMsgStartGame);

            NetManager.AddMsgListener("MsgSyncPos", OnMsgSyncPos);
            NetManager.AddMsgListener("MsgHit", OnMsgHit);

            NetManager.AddMsgListener("MsgBattleResult", OnMsgBattleResult);
        }

        private void OnMsgBattleResult(IExtensible msgBase)
        {
            //处理对局消息
            //打开结果面板 显示 
            CallLuaHelper.PanelClose("ControlPanel");

            string id = GameMain.Instance.id;
            int camp = GetHero(id).GetComponent<CharacterStatus>().camp;
            MsgBattleResult msg = (MsgBattleResult)msgBase;
            if (camp == msg.winCamp)
                CallLuaHelper.PanelShow("ResultPanel", "胜利");
            else
                CallLuaHelper.PanelShow("ResultPanel", "失败");

            //场景清理
            //GameObjectPool.Instance.ClearAll();//清理对象池(游戏开始时 清理)
            //生成角色清理
            CollectAllData();
            //清理地图
        }

        private void OnMsgHit(IExtensible msgBase)
        {
            MsgHit msg = (MsgHit)msgBase;
            if (GameMain.Instance.id == msg.attackId) return;
            GameObject hitHero = GetHero(msg.targetId);
            hitHero.GetComponent<CharacterStatus>().Damage(msg.hitNum);
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

        public GameObject GetHero(string id)
        {
            if (gameDatas.ContainsKey(id))
                return gameDatas[id];
            return null;
        }
        public void CollectAllData()
        {
            List<string> keyList = new List<string>(gameDatas.Keys);
            foreach (var key in keyList)
            {
                GameObjectPool.Instance.CollectObject(gameDatas[key]);
            }
            gameDatas.Clear();
        }

        private void OnMsgStartGame(IExtensible msgBase)
        {
            CallLuaHelper.PanelClose("ProgressPanel");
            CallLuaHelper.PanelClose("MatchPanel");
            CallLuaHelper.PanelClose("GameMainPanel");
        }

        private void OnMsgPrepared(IExtensible msgBase)
        {
            MsgPrepared msg = (MsgPrepared)msgBase;
            float num = msg.currentNum / msg.maxNum;
            CallLuaHelper.ChangeSliderValue(num);
        }

        //开始生成场景 人物模型
        private void OnMsgGetRoomInfo(IExtensible msgBase)
        {
            //清理上场数据
            GameObjectPool.Instance.ClearAll();

            int redNum = 1;
            int blueNum = 1;
            CallLuaHelper.PanelShow("ProgressPanel");

            //ab包不能直接打包预制体及脚本 暂时直接生成
            //GameObject controlPanel = Instantiate(ResourcesManager.Load<GameObject>("ControlPanel"));
            //controlPanel.transform.SetParent(GameObject.Find("Canvas/PanelLayer").transform, false);
            CallLuaHelper.PanelShow("ControlPanel");

            MsgGetRoomInfo msg = (MsgGetRoomInfo)msgBase;
            //生成场景   mapId
            GameObject GameMap = ResourcesManager.Load<GameObject>("Forest");
            //GameMap = Instantiate(GameMap, GameMap.transform.position, GameMap.transform.rotation);
            GameMap = GameObjectPool.Instance.CreateObject("Forest", GameMap, GameMap.transform.position, GameMap.transform.rotation);
            gameDatas.Add("Map", GameMap);
            //获取生成点
            Transform[] wayPointA = GameMap.transform.Find("RestartA").transform.GetComponentsInChildren<Transform>();
            Transform[] wayPointB = GameMap.transform.Find("RestartB").transform.GetComponentsInChildren<Transform>();
            //获取人物模型数据映射表
            LuaTable heroTable = LuaManager.Instance.Global.Get<LuaTable>("heroiconDataList");
            //生成模型
            GameObject go;
            //玩家控制角色 
            string gameMainId = FindObjectOfType<GameMain>().id;
            //相机

            for (int i = 0; i < msg.players.Count; i++)
            {
                LuaTable heroId = heroTable.Get<int, LuaTable>(msg.players[i].heroId);
                string heroName = heroId.Get<string, string>("name");
                go = ResourcesManager.Load<GameObject>(heroName);

                if (msg.players[i].camp == 1)
                {
                    go = CharacterInitConfigFactory.CreateCharacter(go, wayPointA[redNum], msg.players[i], gameMainId,msg.currentCamp);
                    redNum++;
                }
                if (msg.players[i].camp == 2)
                {
                    go = CharacterInitConfigFactory.CreateCharacter(go, wayPointB[blueNum], msg.players[i], gameMainId,msg.currentCamp);
                    blueNum++;
                }
                gameDatas.Add(msg.players[i].id, go);
            }

            //生成完毕 发送准备完毕协议
            NetManager.Send(new MsgPrepared());
        }
    }
}