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
            //����Ծ���Ϣ
            //�򿪽����� ��ʾ 
            CallLuaHelper.PanelClose("ControlPanel");

            string id = GameMain.Instance.id;
            int camp = GetHero(id).GetComponent<CharacterStatus>().camp;
            MsgBattleResult msg = (MsgBattleResult)msgBase;
            if (camp == msg.winCamp)
                CallLuaHelper.PanelShow("ResultPanel", "ʤ��");
            else
                CallLuaHelper.PanelShow("ResultPanel", "ʧ��");

            //��������
            //GameObjectPool.Instance.ClearAll();//��������(��Ϸ��ʼʱ ����)
            //���ɽ�ɫ����
            CollectAllData();
            //�����ͼ
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

        //��ʼ���ɳ��� ����ģ��
        private void OnMsgGetRoomInfo(IExtensible msgBase)
        {
            //�����ϳ�����
            GameObjectPool.Instance.ClearAll();

            int redNum = 1;
            int blueNum = 1;
            CallLuaHelper.PanelShow("ProgressPanel");

            //ab������ֱ�Ӵ��Ԥ���弰�ű� ��ʱֱ������
            //GameObject controlPanel = Instantiate(ResourcesManager.Load<GameObject>("ControlPanel"));
            //controlPanel.transform.SetParent(GameObject.Find("Canvas/PanelLayer").transform, false);
            CallLuaHelper.PanelShow("ControlPanel");

            MsgGetRoomInfo msg = (MsgGetRoomInfo)msgBase;
            //���ɳ���   mapId
            GameObject GameMap = ResourcesManager.Load<GameObject>("Forest");
            //GameMap = Instantiate(GameMap, GameMap.transform.position, GameMap.transform.rotation);
            GameMap = GameObjectPool.Instance.CreateObject("Forest", GameMap, GameMap.transform.position, GameMap.transform.rotation);
            gameDatas.Add("Map", GameMap);
            //��ȡ���ɵ�
            Transform[] wayPointA = GameMap.transform.Find("RestartA").transform.GetComponentsInChildren<Transform>();
            Transform[] wayPointB = GameMap.transform.Find("RestartB").transform.GetComponentsInChildren<Transform>();
            //��ȡ����ģ������ӳ���
            LuaTable heroTable = LuaManager.Instance.Global.Get<LuaTable>("heroiconDataList");
            //����ģ��
            GameObject go;
            //��ҿ��ƽ�ɫ 
            string gameMainId = FindObjectOfType<GameMain>().id;
            //���

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

            //������� ����׼�����Э��
            NetManager.Send(new MsgPrepared());
        }
    }
}