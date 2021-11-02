using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Helper;
using NetWorkFK;
using proto;
using XLua;

namespace Common
{
    /// <summary>
    /// �������ع�����       δ�������
    /// </summary>
    public class LoadScenesManager : MonoSingleton<LoadScenesManager>
    {
        public void LoadScene(string sceneName)
        {
            StartCoroutine(Load(sceneName));
        }

        public void LoadSceneWithPlayer(string sceneName, MsgGetRoomInfo msg)
        {
            StartCoroutine(LoadGamePlayer(sceneName, msg));
        }
        private IEnumerator LoadGamePlayer(string sceneName, MsgGetRoomInfo msg)
        {
            yield return StartCoroutine(Load(sceneName));
            //int redNum = 1;
            //int blueNum = 1;

            //GameObject GameMap = GameObject.Find("Environment");
            ////��ȡ���ɵ�
            //Transform[] wayPointA = GameMap.transform.Find("RestartA").transform.GetComponentsInChildren<Transform>();
            //Transform[] wayPointB = GameMap.transform.Find("RestartB").transform.GetComponentsInChildren<Transform>();
            ////��ȡ����ģ������ӳ���
            //LuaTable heroTable = LuaManager.Instance.Global.Get<LuaTable>("heroiconDataList");
            ////����ģ��
            //GameObject go;
            ////��ҿ��ƽ�ɫ 
            //string gameMainId = FindObjectOfType<GameMain>().id;
            ////���

            //for (int i = 0; i < msg.players.Count; i++)
            //{
            //    LuaTable heroId = heroTable.Get<int, LuaTable>(msg.players[i].heroId);
            //    string heroName = heroId.Get<string, string>("name");
            //    go = ResourcesManager.Load<GameObject>(heroName);

            //    if (msg.players[i].camp == 1)
            //    {
            //        go = CharacterInitConfigFactory.CreateCharacter(go, wayPointA[redNum], msg.players[i], gameMainId, msg.currentCamp);
            //        redNum++;
            //    }
            //    if (msg.players[i].camp == 2)
            //    {
            //        go = CharacterInitConfigFactory.CreateCharacter(go, wayPointB[blueNum], msg.players[i], gameMainId, msg.currentCamp);
            //        blueNum++;
            //    }
            //    GameManager.Instance.gameDatas.Add(msg.players[i].id, go);
            //}

            ////������� ����׼�����Э��
            //NetManager.Send(new MsgPrepared());
        }
        private IEnumerator Load(string sceneName)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            operation.allowSceneActivation = false;
            while (operation.progress < 0.9f)
            {
                CallLuaHelper.ChangeSliderValue(operation.progress);
                yield return null;
            }
            CallLuaHelper.ChangeSliderValue(1);
            yield return null;
            operation.allowSceneActivation = true;
            print("�����������");
        }
    }
}
