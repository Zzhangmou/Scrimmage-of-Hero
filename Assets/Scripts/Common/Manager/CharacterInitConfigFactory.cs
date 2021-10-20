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
    /// ��ɫ��ʼ������
    /// </summary>
    public static class CharacterInitConfigFactory
    {
        //��ȡJS����
        public static Dictionary<int, PlayerJsDataInfo> skillDataDic;

        //static CharacterInitConfigFactory()
        //{
        //    skillDataDic = SkillJsonDataManager.GetPlayerJsDataInfo();
        //}
        //uiλ��
        private static Vector3 offet = new Vector3(0, 2.2f, 0);


        /// <summary>
        /// ������ɫ
        /// </summary>
        /// <param name="hero"></param>
        /// <param name="GenerateTF"></param>
        /// <param name="id"></param>
        /// <param name="isPlayer"></param>
        /// <returns></returns>
        public static GameObject CreateCharacter(GameObject hero, Transform GenerateTF, PlayerInfo playerInfo, string gameMainId)
        {
            skillDataDic = SkillJsonDataManager.GetPlayerJsDataInfo();
            //����
            //GameObject go = GameObject.Instantiate(hero, GenerateTF.position, GenerateTF.rotation);
            GameObject go = GameObjectPool.Instance.CreateObject(hero.name, hero, GenerateTF.position, GenerateTF.rotation);
            GameObject uiCanvas = GameObject.Instantiate(ResourcesManager.Load<GameObject>("PlayerUICanvas")
                , go.transform.position + offet, go.transform.rotation);
            uiCanvas.transform.SetParent(go.transform);
            //����
            if (playerInfo.id == gameMainId)
            {
                return MainPlayerComponentInit(go, playerInfo);
            }
            else
            {
                return SyncPlayerComponentInit(go, playerInfo, playerInfo.id == gameMainId);
            }
        }
        #region ���ǳ�ʼ����ؽű�
        /// <summary>
        /// ���������ʼ��
        /// </summary>
        /// <param name="go"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private static GameObject MainPlayerComponentInit(GameObject go, PlayerInfo playerInfo)
        {
            //�������
            CameraFollow cameraFollow = Object.FindObjectOfType<CameraFollow>();
            //��������
            cameraFollow.targetPlayerTF = go.transform;
            cameraFollow.offset = new Vector3(0, 10, -10);
            //������
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

            go.AddComponent<CharacterInputController>();

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

            //����Tag
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
            //��ʼ������
            go.GetComponent<CharacterUIController>().Init();
            //��Ӷ����¼�
            go.AddComponent<AnimatorEventBehaviour>();
        }
        #endregion
    }
}