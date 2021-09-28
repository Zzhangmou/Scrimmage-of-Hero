using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Character;

namespace Scrimmage.Skill
{
    public class PlayerJsDataInfo
    {
        public float baseATK;
        public float maxHp;

        public List<SkillData> dataList;
    }
    /// <summary>
    /// 用于解析人物技能数据json表
    /// </summary>
    public static class SkillJsonDataManager
    {
        private static Dictionary<int, PlayerJsDataInfo> skillDataDic;

        public static Dictionary<int, PlayerJsDataInfo> GetPlayerJsDataInfo()
        {
            skillDataDic = new Dictionary<int, PlayerJsDataInfo>();
            List<SkillData> dataList = new List<SkillData>();
            TextAsset skillJsonData = (TextAsset)Common.AbManager.Instance.LoadRes("json", "PlayerDataInfo", typeof(TextAsset));
            JArray skillJsArray = (JArray)JsonConvert.DeserializeObject(skillJsonData.text);

            for (int i = 0; i < skillJsArray.Count; i++)
            {
                PlayerJsDataInfo playerJs = new PlayerJsDataInfo();
                int id = (int)skillJsArray[i]["id"];
                if (!skillDataDic.ContainsKey(id))
                {
                    skillDataDic.Add(id, playerJs);
                    CharacterStatus status = new CharacterStatus();
                    skillDataDic[id].baseATK = (float)skillJsArray[i]["baseATK"];
                    skillDataDic[id].maxHp = (float)skillJsArray[i]["maxHp"];
                }
                SkillData data = new SkillData();
                string str;
                data.skillId = (int)skillJsArray[i]["skillId"];
                data.skillName = skillJsArray[i]["skillName"].ToString();
                data.description = skillJsArray[i]["description"].ToString();
                data.coolTime = (int)skillJsArray[i]["coolTime"];
                data.attackScope = (float)skillJsArray[i]["attackScope"];
                data.attackDistance = (float)skillJsArray[i]["attackDistance"];
                data.bulletSpeed = (float)skillJsArray[i]["bulletSpeed"];
                data.attackAngle = (float)skillJsArray[i]["attackAngle"];

                if ((str = skillJsArray[i]["attackTargetTags"].ToString()) != "")
                    data.attackTargetTags = str.Split(',');
                else
                    data.attackTargetTags = new string[0];
                if ((str = skillJsArray[i]["impactType"].ToString()) != "")
                    data.impactType = str.Split(',');
                else
                    data.impactType = new string[0];

                data.atkRadio = (float)skillJsArray[i]["atkRadio"];
                data.durationTime = (float)skillJsArray[i]["durationTime"];
                data.atkInterval = (float)skillJsArray[i]["atkInterval"];
                data.prefabName = skillJsArray[i]["prefabName"].ToString();
                data.animationName = skillJsArray[i]["animationName"].ToString();
                data.hitFxName = skillJsArray[i]["hitFxName"].ToString();
                data.attackType = (SkillAttackType)(int)skillJsArray[i]["attackType"];
                data.selectorType = (SelectorType)(int)skillJsArray[i]["selectorType"];
                data.areaType = (SkillAreaType)(int)skillJsArray[i]["areaType"];
                data.generateType = (SkillGenerateType)(int)skillJsArray[i]["generateType"];
                dataList.Add(data);
                skillDataDic[id].dataList = dataList;
            }
            return skillDataDic;
        }
    }
}