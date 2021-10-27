using Character;
using proto;
using Scrimmage;
using Scrimmage.Skill;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// 角色初始化工厂
    /// </summary>
    public static class CharacterInitConfigFactory
    {
        //获取JS数据
        public static Dictionary<int, PlayerJsDataInfo> skillDataDic;

        //static CharacterInitConfigFactory()
        //{
        //    skillDataDic = SkillJsonDataManager.GetPlayerJsDataInfo();
        //}
        //ui位移
        private static Vector3 offset = new Vector3(0, 2.4f, 0);


        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="hero"></param>
        /// <param name="GenerateTF"></param>
        /// <param name="id"></param>
        /// <param name="isPlayer"></param>
        /// <returns></returns>
        public static GameObject CreateCharacter(GameObject hero, Transform GenerateTF, PlayerInfo playerInfo, string gameMainId, int currentCamp)
        {
            skillDataDic = SkillJsonDataManager.GetPlayerJsDataInfo();
            //生成
            //GameObject go = GameObject.Instantiate(hero, GenerateTF.position, GenerateTF.rotation);
            GameObject go = GameObjectPool.Instance.CreateObject(hero.name, hero, GenerateTF.position, GenerateTF.rotation);
            GameObject uiCanvas = GameObject.Instantiate(ResourcesManager.Load<GameObject>("PlayerUICanvas")
                , go.transform.position + offset, go.transform.rotation);
            uiCanvas.transform.SetParent(go.transform);
            //配置
            if (playerInfo.id == gameMainId)
            {
                return MainPlayerComponentInit(go, playerInfo);
            }
            else
            {
                return SyncPlayerComponentInit(go, playerInfo, playerInfo.camp == currentCamp);
            }
        }
        #region 主角初始化相关脚本
        /// <summary>
        /// 主角组件初始化
        /// </summary>
        /// <param name="go"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private static GameObject MainPlayerComponentInit(GameObject go, PlayerInfo playerInfo)
        {
            //添加组件
            CharacterController controller = go.AddComponent<CharacterController>();
            controller.height = 2;
            controller.center = new Vector3(0, 1f, 0);
            Rigidbody rigidbody = go.AddComponent<Rigidbody>();
            rigidbody.useGravity = false;
            rigidbody.freezeRotation = true;

            CharacterSkillSystem characterSkillSystem = go.AddComponent<CharacterSkillSystem>();
            SkillData[] skillDatas = skillDataDic[playerInfo.heroId].dataList.ToArray();

            //Debug.Log(skillDatas[1].coolRemain);

            characterSkillSystem.SetSkillData(skillDatas);

            go.AddComponent<CharacterMotor>().moveSpeed = skillDataDic[playerInfo.heroId].moveSpeed;
            go.AddComponent<CharacterUIController>();
            go.AddComponent<PlayerStatus>();
            CharacterDataConfig(go, playerInfo);

            //设置相机
            CameraFollow cameraFollow = Object.FindObjectOfType<CameraFollow>();
            //根据阵营设置属性
            if (playerInfo.camp == 1)
            {
                cameraFollow.CameraInit(go.transform, new Vector3(0, 22, -9));
                go.AddComponent<CharacterInputController>();
            }
            else
            {
                cameraFollow.CameraInit(go.transform, new Vector3(0, 22, 9));
                go.AddComponent<CharacterInputController>().reverse = true;
            }
            go.tag = "TeamMate";
            return go;
        }

        public static GameObject SyncPlayerComponentInit(GameObject go, PlayerInfo playerInfo, bool isTeam)
        {
            go.AddComponent<CharacterSyncMotor>().moveSpeed = skillDataDic[playerInfo.heroId].moveSpeed;
            Rigidbody rigidbody = go.AddComponent<Rigidbody>();
            rigidbody.useGravity = false;
            rigidbody.freezeRotation = true;

            go.AddComponent<CharacterUIController>();
            go.AddComponent<EnemyStatus>();

            CharacterDataConfig(go, playerInfo);

            //设置Tag
            if (isTeam)
                go.tag = "TeamMate";
            else
                go.tag = "Enemy";

            return go;
        }

        private static void CharacterDataConfig(GameObject go, PlayerInfo playerInfo)
        {
            CharacterStatus characterStatus = go.GetComponent<CharacterStatus>();
            characterStatus.baseATK = skillDataDic[playerInfo.heroId].baseATK;
            characterStatus.HP = skillDataDic[playerInfo.heroId].maxHp;
            characterStatus.maxHp = skillDataDic[playerInfo.heroId].maxHp;

            characterStatus.camp = playerInfo.camp;
            characterStatus.userName = playerInfo.userName;
            characterStatus.id = playerInfo.id;
            characterStatus.heroId = playerInfo.heroId;
            //初始化数据
            go.GetComponent<CharacterUIController>().Init();
            //添加动画事件
            go.AddComponent<AnimatorEventBehaviour>();
        }
        #endregion
    }
}