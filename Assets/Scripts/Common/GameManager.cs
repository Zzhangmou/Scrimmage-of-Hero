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
using ns;

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
        MsgGetRoomInfo msg = (MsgGetRoomInfo)msgBase;
        //生成场景   mapId
        GameObject GameMap = ResourcesManager.Load<GameObject>("Grass");
        GameObject.Instantiate(GameMap, GameMap.transform.position, GameMap.transform.rotation);
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
            LuaTable hero = heroTable.Get<int, LuaTable>(msg.players[i].heroId);
            string heroName = hero.Get<string, string>("name");
            GameObject heroGo = ResourcesManager.Load<GameObject>(heroName);

            if (msg.players[i].camp == 1)
            {
                go = GameObject.Instantiate(heroGo, wayPointA[redNum].position, wayPointA[redNum].rotation);
                redNum++;
            }
            if (msg.players[i].camp == 2)
            {
                go = GameObject.Instantiate(heroGo, wayPointB[blueNum].position, wayPointB[blueNum].rotation);
                blueNum++;
            }
            //玩家控制角色
            if (msg.players[i].id == gameMainId)
            {
                /*
                 * ab包不能直接打包预制体及脚本 暂时直接生成
                 */
                GameObject control = Instantiate(ResourcesManager.Load<GameObject>("ControlPanel"));
                control.transform.SetParent(GameObject.Find("Canvas/PanelLayer").transform, false);

                CameraFollow cameraFollow = FindObjectOfType<CameraFollow>();
                //设置属性
                cameraFollow.targetPlayerTF = go.transform;
                cameraFollow.offset = new Vector3(0, 10, -10);
                //添加控制器
                ComponentInit(go);
            }
            else
            {
                go.AddComponent<CharacterSyncMotor>();
            }

            heros.Add(msg.players[i].id, go);
        }

        //生成完毕 发送准备完毕协议
        NetManager.Send(new MsgPrepared());
    }
    /// <summary>
    /// 初始化组件
    /// </summary>
    /// <param name="go"></param>
    private static void ComponentInit(GameObject go)
    {
        CharacterController controller = go.AddComponent<CharacterController>();
        controller.height = 2;
        controller.center = new Vector3(0, 0.92f, 0);
        go.AddComponent<CharacterMotor>();
        go.AddComponent<CharacterInputController>();
    }
}
