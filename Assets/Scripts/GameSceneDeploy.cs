using NetWorkFK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using proto;

namespace Common
{
    /// <summary>
    /// ��Ϸ��������    δ�������
    /// </summary>
    public class GameSceneDeploy : MonoBehaviour
    {
        //private void Start()
        //{
        //    print("��ʼ");
        //    int redNum = 1;
        //    int blueNum = 1;

        //    MsgGetRoomInfo msg = GameManager.Instance.roomInfo;
        //    GameObject GameMap = GameObject.Find("Environment");
        //    //��ȡ���ɵ�
        //    Transform[] wayPointA = GameMap.transform.Find("RestartA").transform.GetComponentsInChildren<Transform>();
        //    Transform[] wayPointB = GameMap.transform.Find("RestartB").transform.GetComponentsInChildren<Transform>();
        //    //��ȡ����ģ������ӳ���
        //    LuaTable heroTable = GameManager.Instance.luaTable;
        //    //����ģ��
        //    GameObject go;
        //    //��ҿ��ƽ�ɫ 
        //    string gameMainId = FindObjectOfType<GameMain>().id;
        //    //���

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

        //    //������� ����׼�����Э��
        //    NetManager.Send(new MsgPrepared());
        //}
    }
}
