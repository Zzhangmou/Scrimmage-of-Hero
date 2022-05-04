using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

namespace Scrimmage.Skill
{
    /// <summary>
    /// 技能管理器
    /// </summary>
    public class CharacterSkillManager : MonoBehaviour
    {
        //技能列表
        public SkillData[] skills;

        private void Start()
        {
            for (int i = 0; i < skills.Length; i++)
                InitSkill(skills[i]);
        }

        //初始化技能
        private void InitSkill(SkillData data)
        {
            /*
             * 资源映射表
             * 资源名称---->资源完整路径
             */

            //data.skillPrefab = Resources.Load<GameObject>("Prefabs/SkillFX/" + data.prefabName);
            data.skillPrefab = ResourcesManager.Load<GameObject>(data.prefabName);
            data.owner = gameObject;
            data.attackPos = transform.Find("FirePos");
        }
        //准备技能
        public SkillData PrepareSkill(int id)
        {
            //根据id 查找技能数据
            SkillData data = skills.Find(s => s.skillId == id);
            //判断条件            返回技能数据
            if (data != null && data.coolRemain <= 0)
                return data;
            else
                return null;
        }
        //生成技能
        public void GenerateSkill(SkillData data)
        {
            //创建技能预制件
            GameObject skillGo = GameObjectPool.Instance.CreateObject(data.prefabName, data.skillPrefab, data.prefabPos, transform.rotation);
            //GameObject skillGo = GameObjectPool.Instance.CreateObject(data.prefabName, data.skillPrefab, data.prefabPos, Quaternion.Euler(data.prefabRotation));
            SkillDeployer deployer = skillGo.GetComponent<SkillDeployer>();
            //传递技能数据
            //内部创建算法对象
            deployer.SkillData = data;
            //内部执行算法对象
            deployer.DeploySkill();
            if (data.generateType != SkillGenerateType.FileAndDIs)
                GameObjectPool.Instance.CollectObject(skillGo, data.durationTime);
            //冷却
            StartCoroutine(CoolTimeDown(data));
        }
        //技能冷却
        private IEnumerator CoolTimeDown(SkillData data)
        {
            data.coolRemain = data.coolTime;
            while (data.coolRemain > 0)
            {

                yield return new WaitForSeconds(1);
                data.coolRemain--;
            }
        }
    }
}