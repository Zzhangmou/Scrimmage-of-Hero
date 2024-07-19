using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scrimmage.Skill;

namespace Common
{
    /// <summary>
    /// 测试脚本
    /// </summary>
    public class TestFunction : MonoBehaviour
    {
        //获取JS数据
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
                    print("检查更新结束");
                }
                else
                {
                    print("更新失败");
                }
            }, (result) =>
            {
                print(result);
            });
        }
    }
}
