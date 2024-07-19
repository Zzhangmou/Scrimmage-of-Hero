using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������
/// </summary>
public enum ENode_Type
{
    Walk,
    Stop
}
/// <summary>
/// A*������
/// </summary>
public class AStarNode
{
    public int x;
    public int y;

    /// <summary>
    /// Ѱ·����
    /// </summary>
    public float f;
    /// <summary>
    /// �����ľ���
    /// </summary>
    public float g;
    /// <summary>
    /// ���յ�ľ���
    /// </summary>
    public float h;
    /// <summary>
    /// �����Ӷ���
    /// </summary>
    public AStarNode father;

    public ENode_Type type;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="x">x����</param>
    /// <param name="y">y����</param>
    /// <param name="type">��������</param>
    public AStarNode(int x, int y, ENode_Type type)
    {
        this.x = x;
        this.y = y;
        this.type = type;
    }
}