using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scrimmage.Skill
{
    /// <summary>
    /// 技能生成方式  通过该枚举数据决定技能释放位置  以及协议格式
    /// </summary>
    public enum SkillGenerateType
    {
        //原地
        Inplace = 0,
        //选择
        Select = 1,
        //开火
        Fire = 2,
        //释放并跟随
        FileAndFllow = 3,
        //释放并且附带位移
        FileAndDIs=4
    }
}
