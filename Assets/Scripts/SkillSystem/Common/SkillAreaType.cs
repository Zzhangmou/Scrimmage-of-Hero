using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scrimmage.Skill
{
    /// <summary>
    /// 技能区域类型
    /// </summary>
    public enum SkillAreaType
    {
        /// <summary>
        /// 外圆
        /// </summary>
        OuterCircle = 0,
        /// <summary>
        /// 外圆/内矩形
        /// </summary>
        OuterCircle_InnerRectangle = 1,
        /// <summary>
        /// 外圆/内圆
        /// </summary>
        OuterCircle_InnerCircle = 2,
        /// <summary>
        /// 外圆/内扇形60
        /// </summary>
        OuterCircle_InnerSector60 = 3,
        /// <summary>
        /// 外圆/内扇形120
        /// </summary>
        OuterCircle_InnerSector120 = 4
    }
}
