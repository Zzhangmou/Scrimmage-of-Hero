using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Character
{
    /// <summary>
    /// ��ɫUIѪ������
    /// </summary>
    public class CharacterUIController : MonoBehaviour
    {
        [SerializeField]
        private Text nameText;
        [SerializeField]
        private Text hpText;
        [SerializeField]
        private Slider hpSlider;
        public Transform uiTF;
        private void Awake()
        {
            uiTF = transform.Find("PlayerUICanvas" + "(Clone)");
            nameText = uiTF.Find("UserNameText").GetComponent<Text>();
            hpText = uiTF.Find("UserHpText").GetComponent<Text>();
            hpSlider = uiTF.Find("UserHpSlider").GetComponent<Slider>();
        }

        private void LateUpdate()
        {
            //Canvas ���������
            uiTF.rotation = Camera.main.transform.rotation;
        }
        public void Init()
        {
            //��ʼ��
            CharacterStatus characterStatus = transform.GetComponent<CharacterStatus>();
            nameText.text = characterStatus.userName;
            hpText.text = characterStatus.maxHp.ToString();
            hpSlider.value = characterStatus.HP / characterStatus.maxHp;
        }

        public void ChangeSliderValue(float hpValue, float maxHpValue)
        {
            if (hpValue <= 0) hpValue = 0;
            hpText.text = hpValue.ToString();
            hpSlider.value = hpValue / maxHpValue;
        }
    }
}
