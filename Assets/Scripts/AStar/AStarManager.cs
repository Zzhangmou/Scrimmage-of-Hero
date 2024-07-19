using Common;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A*管理器
/// </summary>
public class AStarManager : MonoSingleton<AStarManager>
{
    //宽高
    private int mapW;
    private int mapH;

    /// <summary>
    /// 所有格子
    /// </summary>
    public AStarNode[,] nodes;
    /// <summary>
    /// 开启列表
    /// </summary>
    private List<AStarNode> openList;
    /// <summary>
    /// 关闭列表
    /// </summary>
    private List<AStarNode> closeList;

    public void InitMapInfo(int mapW, int mapH)
    {
        this.mapW = mapW;
        this.mapH = mapH;
        nodes = new AStarNode[mapW, mapH];
        openList = new List<AStarNode>();
        closeList = new List<AStarNode>();
        for (int i = 0; i < mapW; i++)
        {
            for (int j = 0; j < mapH; j++)
            {
                AStarNode node = new AStarNode(i, j, UnityEngine.Random.Range(0, 100) >= 20 ? ENode_Type.Walk : ENode_Type.Stop);
                nodes[i, j] = node;
            }
        }
    }

    /// <summary>
    /// 寻路
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <returns></returns>
    public Stack<AStarNode> FindPath(Vector2 startPos, Vector2 endPos)
    {
        //地图范围内 点是否合法
        if (startPos.x >= mapW || startPos.y >= mapH || startPos.x < 0 || startPos.y < 0 ||
            endPos.x >= mapW || endPos.y >= mapH || endPos.x < 0 || endPos.y < 0)
        {
            Debug.Log("不在地图范围内");
            return null;
        }
        //不阻挡
        AStarNode startNode = nodes[(int)startPos.x, (int)startPos.y];
        AStarNode endNode = nodes[(int)endPos.x, (int)endPos.y];
        if (startNode.type == ENode_Type.Stop || endNode.type == ENode_Type.Stop)
        {
            Debug.Log("点被阻挡");
            return null;
        }
        //清空
        openList.Clear();
        closeList.Clear();

        //把开始放入关闭列表
        startNode.father = null;
        startNode.f = 0;
        startNode.g = 0;
        startNode.h = 0;
        closeList.Add(startNode);

        while (true)
        {
            //判断点是否是边界 阻挡 是否在开启或关闭列表 都不是放入开启列表
            //左上
            FindNearlyNodeToOpenList(startNode.x - 1, startNode.y + 1, 1.4f, startNode, endNode);
            //上
            FindNearlyNodeToOpenList(startNode.x, startNode.y + 1, 1, startNode, endNode);
            //右上
            FindNearlyNodeToOpenList(startNode.x + 1, startNode.y + 1, 1.4f, startNode, endNode);
            //左
            FindNearlyNodeToOpenList(startNode.x - 1, startNode.y, 1, startNode, endNode);
            //右
            FindNearlyNodeToOpenList(startNode.x + 1, startNode.y, 1, startNode, endNode);
            //左下
            FindNearlyNodeToOpenList(startNode.x - 1, startNode.y - 1, 1.4f, startNode, endNode);
            //下
            FindNearlyNodeToOpenList(startNode.x, startNode.y - 1, 1, startNode, endNode);
            //右下
            FindNearlyNodeToOpenList(startNode.x + 1, startNode.y - 1, 1.4f, startNode, endNode);

            //开启列表为空 还没找到终点 死路
            if (openList.Count == 0)
            {
                Debug.Log("死路");
                return null;
            }
            //选出开启列表中 寻路消耗最小的点
            //openList.Sort(SortOpenList);
            openList.Sort((a, b) =>
            {
                if (a.f > b.f)
                    return 1;
                else if (a.f == b.f)
                    return 1;
                else return -1;
            });
            //放入关闭列表 从开启列表移除
            closeList.Add(openList[0]);
            //找到点 变成新的起点 进行下一次计算
            startNode = openList[0];
            openList.RemoveAt(0);

            if (startNode == endNode)
            {
                //是终点 返回结果
                Stack<AStarNode> path = new Stack<AStarNode>();
                path.Push(endNode);
                while (endNode.father != null)
                {
                    path.Push(endNode.father);
                    endNode = endNode.father;
                }
                return path;
            }
        }
    }
    private int SortOpenList(AStarNode a, AStarNode b)
    {
        if (a.f > b.f)
            return 1;
        else if (a.f == b.f)
            return 1;
        else
            return -1;
    }
    /// <summary>
    /// 把临近点放入开启列表
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="g">寻路消耗 与父对象的距离</param>
    /// <param name="fatherNode"></param>
    /// <param name="endNode"></param>
    private void FindNearlyNodeToOpenList(int x, int y, float g, AStarNode fatherNode, AStarNode endNode)
    {
        if (x < 0 || y < 0 || x >= mapW || y >= mapH) return;

        AStarNode node = nodes[x, y];
        //判断点是否边界 阻挡 在开启或关闭列表
        if (node == null || node.type == ENode_Type.Stop) return;
        //如果节点已经在开启列表中 但下一个最小值点也可以算到该点 那么该点的父节点不会变为这个最小值点 就会导致路径不是最短的 应该还要添加判断
        if (openList.Contains(node))
        {
            float gThis = fatherNode.g + g;
            if (gThis < node.g)
            {
                node.g = gThis;
                node.f = node.g + node.h;
                node.father = fatherNode;
                return;
            }
            else
            {
                return;
            }
        }
        //if (openList.Contains(node)) return;
        if (closeList.Contains(node)) return;
        //计算f值
        node.father = fatherNode;
        node.g = fatherNode.g + g;
        node.h = Mathf.Abs(endNode.x - node.x) + Mathf.Abs(endNode.y - node.y);
        node.f = node.g + node.h;

        //通过验证 加入开启列表
        openList.Add(node);
    }
}