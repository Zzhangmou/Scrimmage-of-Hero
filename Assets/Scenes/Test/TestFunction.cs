using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using Scrimmage.Skill;

namespace ns
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
            skillDataDic = SkillJsonDataManager.GetPlayerJsDataInfo();
            PlayerJsDataInfo playerJsDataInfo = skillDataDic[18];
            List<SkillData> data = playerJsDataInfo.dataList;
            SkillData[] skillDatas = data.ToArray();
        }
    }
}
