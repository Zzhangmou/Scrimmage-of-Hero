using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 格子类型
/// </summary>
public enum ENode_Type
{
    Walk,
    Stop
}
/// <summary>
/// A*格子类
/// </summary>
public class AStarNode
{
    public int x;
    public int y;

    /// <summary>
    /// 寻路消耗
    /// </summary>
    public float f;
    /// <summary>
    /// 离起点的距离
    /// </summary>
    public float g;
    /// <summary>
    /// 离终点的距离
    /// </summary>
    public float h;
    /// <summary>
    /// 父格子对象
    /// </summary>
    public AStarNode father;

    public ENode_Type type;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="x">x坐标</param>
    /// <param name="y">y坐标</param>
    /// <param name="type">格子类型</param>
    public AStarNode(int x, int y, ENode_Type type)
    {
        this.x = x;
        this.y = y;
        this.type = type;
    }
}