using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns
{
    /// <summary>
    /// ��ɫ״̬��Ϣ��
    /// </summary>
    public class PlayerStatus : MonoBehaviour
    {
        public CharacterAnimationParameter chParams;
        [Header("ID")]
        public string id;
        [Header("Ѫ��")]
        public float HP;
        [Header("���Ѫ��")]
        public float maxHp;
        [Header("������")]
        public float baseATK;
        [Header("�������")]
        public float attackInterval;
        [Header("��������")]
        public float attackDistance;


        public void Damage(float val)
        {
            HP -= val;
            if (HP <= 0)
                Death();
        }
        public void Death()
        {
            GetComponent<Animator>().SetBool(chParams.dead, true);
            Debug.Log(id + " ���� ");
        }
    }
}

