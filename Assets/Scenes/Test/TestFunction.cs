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


            //AbUpdateManager.Instance.CheckUpdate((isOver) =>
            //{
            //    if (isOver)
            //    {
            //        print("检查更新结束");
            //    }
            //    else
            //    {
            //        print("更新失败");
            //    }
            //}, (result) =>
            //{
            //    print(result);
            //});


        }
        private void Update()
        {
            float x = Mathf.Sin(30 * Mathf.Deg2Rad) * 10;
            float z = Mathf.Cos(30 * Mathf.Deg2Rad) * 10;
            Debug.DrawLine(transform.position, transform.position + new Vector3(-x, 0, z), Color.red);
            Debug.DrawLine(transform.position, transform.position + new Vector3(x, 0, z), Color.red);
            Debug.DrawLine(transform.position, transform.TransformPoint(new Vector3(0, 0, 10)), Color.blue);

            //等价
            Debug.DrawLine(transform.position, transform.position + Quaternion.Euler(0, 30, 0) * transform.forward, Color.green);
            Debug.DrawLine(transform.position, transform.position + Quaternion.Euler(0, -30, 0) * transform.forward, Color.green);
        }
    }
}
