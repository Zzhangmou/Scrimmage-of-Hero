using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scrimmage.Skill
{
    /// <summary>
    /// ����ѡ��
    /// </summary>
    public interface IAttackSelector
    {
        /// <summary>
        /// ����Ŀ��
        /// </summary>
        /// <param name="data">��������</param>
        /// <param name="skillTF">������������ı任���</param>
        /// <returns></returns>
        Transform[] SelectTarget(SkillData data, Transform skillTF);
    }
}

