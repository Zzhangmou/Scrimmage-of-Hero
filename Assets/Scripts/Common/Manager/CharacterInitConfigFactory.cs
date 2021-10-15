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
        private static Dictionary<int, PlayerJsDataInfo> skillDataDic = SkillJsonDataManager.GetPlayerJsDataInfo();

        private static PlayerInfo playerInfoData;
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
            //��ֵ
            playerInfoData = playerInfo;
            //����
            GameObject go = GameObject.Instantiate(hero, GenerateTF.position, GenerateTF.rotation);
            GameObject uiCanvas = GameObject.Instantiate(ResourcesManager.Load<GameObject>("PlayerUICanvas")
                , go.transform.position + offet, go.transform.rotation);
            uiCanvas.transform.SetParent(go.transform);
            //����
            if (playerInfo.id == gameMainId)
            {
                return MainPlayerComponentInit(go);
            }
            else
            {
                return SyncPlayerComponentInit(go, playerInfo.id == gameMainId);
            }
        }
        #region ���ǳ�ʼ����ؽű�
        /// <summary>
        /// ���������ʼ��
        /// </summary>
        /// <param name="go"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private static GameObject MainPlayerComponentInit(GameObject go)
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
            SkillData[] skilldata = skillDataDic[playerInfoData.heroId].dataList.ToArray();
            characterSkillSystem.SetSkillData(skilldata);

            go.AddComponent<CharacterMotor>().moveSpeed = skillDataDic[playerInfoData.heroId].moveSpeed;
            go.AddComponent<CharacterUIController>();
            go.AddComponent<PlayerStatus>();
            CharacterDataConfig(go);

            go.AddComponent<CharacterInputController>();

            go.tag = "TeamMate";
            return go;
        }

        public static GameObject SyncPlayerComponentInit(GameObject go, bool isTeam)
        {
            go.AddComponent<CharacterSyncMotor>().moveSpeed = skillDataDic[playerInfoData.heroId].moveSpeed;
            Rigidbody rigidbody = go.AddComponent<Rigidbody>();
            rigidbody.useGravity = false;
            rigidbody.freezeRotation = true;

            go.AddComponent<CharacterUIController>();
            go.AddComponent<EnemyStatus>();

            CharacterDataConfig(go);

            //����Tag
            if (isTeam)
                go.tag = "TeamMate";
            else
                go.tag = "Enemy";

            return go;
        }

        private static void CharacterDataConfig(GameObject go)
        {
            CharacterStatus characterStatus = go.GetComponent<CharacterStatus>();
            characterStatus.baseATK = skillDataDic[playerInfoData.heroId].baseATK;
            characterStatus.HP = skillDataDic[playerInfoData.heroId].maxHp;
            characterStatus.maxHp = skillDataDic[playerInfoData.heroId].maxHp;

            characterStatus.camp = playerInfoData.camp;
            characterStatus.userName = playerInfoData.userName;
            characterStatus.id = playerInfoData.id;
            characterStatus.heroId = playerInfoData.heroId;
            //��ʼ������
            go.GetComponent<CharacterUIController>().Init();
        }
        #endregion
    }
}