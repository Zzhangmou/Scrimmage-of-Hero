using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

namespace ns
{
    /// <summary>
    /// 技能管理器
    /// </summary>
    public class CharacterSkillManager : MonoBehaviour
    {
        //技能列表
        public SkillData[] skills;
        //技能元素列表
        private Dictionary<SkillAreaElement, Transform> allElementTrans;
        //技能指示父级
        private Transform elementParent;

        private void Start()
        {
            for (int i = 0; i < skills.Length; i++)
                InitSkill(skills[i]);
        }
        //初始化技能
        private void InitSkill(SkillData data)
        {
            data.skillPrefab = Resources.Load<GameObject>("Prefabs/SkillFX/" + data.prefabName);
            data.owner = gameObject;
        }
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
            //创建
            GameObject skillGo = Instantiate(data.skillPrefab, transform.position, transform.rotation);
            //销毁
            Destroy(skillGo, data.durationTime);
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


        #region 技能区域提示



        #endregion
    }
}

