using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    /// <summary>
    /// ��ɫ״̬��Ϣ��
    /// </summary>
    public class CharacterStatus : MonoBehaviour
    {
        public CharacterAnimationParameter chParams;
        [Header("����ID")]
        public string id;
        [Header("��ɫID")]
        public int heroId;
        [Header("�û�����")]
        public string userName;
        [Header("��Ӫ")]
        public int camp;
        [Header("Ѫ��")]
        public float HP;
        [Header("���Ѫ��")]
        public float maxHp;
        [Header("������")]
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
            Debug.Log(id + " ���� ");
        }
    }
}

