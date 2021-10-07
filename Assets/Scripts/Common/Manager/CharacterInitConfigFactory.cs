using Character;
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

        /// <summary>
        /// ������ɫ
        /// </summary>
        /// <param name="hero"></param>
        /// <param name="GenerateTF"></param>
        /// <param name="id"></param>
        /// <param name="isPlayer"></param>
        /// <returns></returns>
        public static GameObject CreateCharacter(GameObject hero, Transform GenerateTF, int id, bool isPlayer)
        {
            GameObject go;
            go = GameObject.Instantiate(hero, GenerateTF.position, GenerateTF.rotation);
            if (!isPlayer)
            {
                go.AddComponent<CharacterSyncMotor>().moveSpeed = skillDataDic[id].moveSpeed;
                Rigidbody rigidbody = go.AddComponent<Rigidbody>();
                rigidbody.useGravity = false;
                rigidbody.freezeRotation = true;
                go.AddComponent<EnemyStatus>();
                return go;
            }
            return MainPlayerComponentInit(go, id);
        }

        private static GameObject MainPlayerComponentInit(GameObject go, int id)
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
            SkillData[] skilldata = skillDataDic[id].dataList.ToArray();
            characterSkillSystem.SetSkillData(skilldata);

            go.AddComponent<CharacterMotor>().moveSpeed=skillDataDic[id].moveSpeed;
            go.AddComponent<PlayerStatus>();
            go.GetComponent<CharacterStatus>().baseATK = skillDataDic[id].baseATK;
            go.GetComponent<CharacterStatus>().HP = skillDataDic[id].maxHp;
            go.GetComponent<CharacterStatus>().maxHp = skillDataDic[id].maxHp;
            go.AddComponent<CharacterInputController>();

            return go;
        }
    }
}
