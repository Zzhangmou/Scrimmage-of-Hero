using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scrimmage.Skill
{
    /// <summary>
    /// ������������
    /// </summary>
    public enum SkillAreaType
    {
        /// <summary>
        /// ��Բ
        /// </summary>
        OuterCircle = 0,
        /// <summary>
        /// ��Բ/�ھ���
        /// </summary>
        OuterCircle_InnerRectangle = 1,
        /// <summary>
        /// ��Բ/��Բ
        /// </summary>
        OuterCircle_InnerCircle = 2,
        /// <summary>
        /// ��Բ/������60
        /// </summary>
        OuterCircle_InnerSector60 = 3,
        /// <summary>
        /// ��Բ/������120
        /// </summary>
        OuterCircle_InnerSector120 = 4
    }
}
