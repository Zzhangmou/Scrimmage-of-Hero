using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scrimmage.Skill;

namespace Common
{
    /// <summary>
    /// ���Խű�
    /// </summary>
    public class TestFunction : MonoBehaviour
    {
        //��ȡJS����
        public static Dictionary<int, PlayerJsDataInfo> skillDataDic;
        void Start()
        {
            //skillDataDic = SkillJsonDataManager.GetPlayerJsDataInfo();
            //PlayerJsDataInfo playerJsDataInfo = skillDataDic[18];
            //List<SkillData> data = playerJsDataInfo.dataList;
            //SkillData[] skillDatas = data.ToArray();


            AbUpdateManager.Instance.CheckUpdate((isOver) =>
            {
                if (isOver)
                {
                    print("�����½���");
                }
                else
                {
                    print("����ʧ��");
                }
            }, (result) =>
            {
                print(result);
            });
        }
    }
}
