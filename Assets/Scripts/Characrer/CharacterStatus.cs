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
        [Header("ID")]
        public string id;
        [Header("血量")]
        public float HP;
        [Header("最大血量")]
        public float maxHp;
        [Header("攻击力")]
        public float baseATK;
        [Header("攻击间隔")]
        public float attackInterval;
        [Header("攻击距离")]
        public float attackDistance;


        public void Damage(float val)
        {
            HP -= val;
            if (HP <= 0)
                Death();
        }
        public virtual void Death()
        {
            GetComponent<Animator>().SetBool(chParams.dead, true);
            Debug.Log(id + " 死亡 ");
        }
    }
}

