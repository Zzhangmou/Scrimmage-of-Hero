using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using Scrimmage.Skill;

namespace ns
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
            skillDataDic = SkillJsonDataManager.GetPlayerJsDataInfo();
            PlayerJsDataInfo playerJsDataInfo = skillDataDic[18];
            List<SkillData> data = playerJsDataInfo.dataList;
            SkillData[] skillDatas = data.ToArray();
        }
    }
}
