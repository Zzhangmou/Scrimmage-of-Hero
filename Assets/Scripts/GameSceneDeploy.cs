using NetWorkFK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using proto;

namespace Common
{
    /// <summary>
    /// 游戏场景配置    未解决方案
    /// </summary>
    public class GameSceneDeploy : MonoBehaviour
    {
        //private void Start()
        //{
        //    print("开始");
        //    int redNum = 1;
        //    int blueNum = 1;

        //    MsgGetRoomInfo msg = GameManager.Instance.roomInfo;
        //    GameObject GameMap = GameObject.Find("Environment");
        //    //获取生成点
        //    Transform[] wayPointA = GameMap.transform.Find("RestartA").transform.GetComponentsInChildren<Transform>();
        //    Transform[] wayPointB = GameMap.transform.Find("RestartB").transform.GetComponentsInChildren<Transform>();
        //    //获取人物模型数据映射表
        //    LuaTable heroTable = GameManager.Instance.luaTable;
        //    //生成模型
        //    GameObject go;
        //    //玩家控制角色 
        //    string gameMainId = FindObjectOfType<GameMain>().id;
        //    //相机

        //    for (int i = 0; i < msg.players.Count; i++)
        //    {
        //        LuaTable heroId = heroTable.Get<int, LuaTable>(msg.players[i].heroId);
        //        string heroName = heroId.Get<string, string>("name");
        //        go = ResourcesManager.Load<GameObject>(heroName);
        //        if (msg.players[i].camp == 1)
        //        {
        //            go = CharacterInitConfigFactory.CreateCharacter(go, wayPointA[redNum], msg.players[i], gameMainId, msg.currentCamp);
        //            redNum++;
        //        }
        //        if (msg.players[i].camp == 2)
        //        {
        //            go = CharacterInitConfigFactory.CreateCharacter(go, wayPointB[blueNum], msg.players[i], gameMainId, msg.currentCamp);
        //            blueNum++;
        //        }
        //        GameManager.Instance.gameDatas.Add(msg.players[i].id, go);
        //    }

        //    //生成完毕 发送准备完毕协议
        //    NetManager.Send(new MsgPrepared());
        //}
    }
}
