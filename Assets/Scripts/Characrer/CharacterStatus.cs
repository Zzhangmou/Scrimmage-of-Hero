using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    /// <summary>
    /// 角色状态信息类
    /// </summary>
    public class CharacterStatus : MonoBehaviour
    {
        public CharacterAnimationParameter chParams;
        [Header("连接ID")]
        public string id;
        [Header("角色ID")]
        public int heroId;
        [Header("用户名称")]
        public string userName;
        [Header("阵营")]
        public int camp;
        [Header("血量")]
        public float HP;
        [Header("最大血量")]
        public float maxHp;
        [Header("攻击力")]
        public float baseATK;
        private void Start()
        {
            chParams = new CharacterAnimationParameter();
        }

        public void Damage(float val)
        {
            HP -= val;
            GetComponentInChildren<CharacterUIController>().ChangeSliderValue(HP, maxHp);
            if (HP <= 0)
                Death();
        }
        public virtual void Death()
        {
            transform.gameObject.tag = "Death";
            GetComponent<Animator>().SetBool(chParams.dead, true);
            Debug.Log(id + " 死亡 ");
        }
    }
}

