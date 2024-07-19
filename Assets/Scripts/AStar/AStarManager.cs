using Common;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A*������
/// </summary>
public class AStarManager : MonoSingleton<AStarManager>
{
    //���
    private int mapW;
    private int mapH;

    /// <summary>
    /// ���и���
    /// </summary>
    public AStarNode[,] nodes;
    /// <summary>
    /// �����б�
    /// </summary>
    private List<AStarNode> openList;
    /// <summary>
    /// �ر��б�
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
    /// Ѱ·
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <returns></returns>
    public Stack<AStarNode> FindPath(Vector2 startPos, Vector2 endPos)
    {
        //��ͼ��Χ�� ���Ƿ�Ϸ�
        if (startPos.x >= mapW || startPos.y >= mapH || startPos.x < 0 || startPos.y < 0 ||
            endPos.x >= mapW || endPos.y >= mapH || endPos.x < 0 || endPos.y < 0)
        {
            Debug.Log("���ڵ�ͼ��Χ��");
            return null;
        }
        //���赲
        AStarNode startNode = nodes[(int)startPos.x, (int)startPos.y];
        AStarNode endNode = nodes[(int)endPos.x, (int)endPos.y];
        if (startNode.type == ENode_Type.Stop || endNode.type == ENode_Type.Stop)
        {
            Debug.Log("�㱻�赲");
            return null;
        }
        //���
        openList.Clear();
        closeList.Clear();

        //�ѿ�ʼ����ر��б�
        startNode.father = null;
        startNode.f = 0;
        startNode.g = 0;
        startNode.h = 0;
        closeList.Add(startNode);

        while (true)
        {
            //�жϵ��Ƿ��Ǳ߽� �赲 �Ƿ��ڿ�����ر��б� �����Ƿ��뿪���б�
            //����
            FindNearlyNodeToOpenList(startNode.x - 1, startNode.y + 1, 1.4f, startNode, endNode);
            //��
            FindNearlyNodeToOpenList(startNode.x, startNode.y + 1, 1, startNode, endNode);
            //����
            FindNearlyNodeToOpenList(startNode.x + 1, startNode.y + 1, 1.4f, startNode, endNode);
            //��
            FindNearlyNodeToOpenList(startNode.x - 1, startNode.y, 1, startNode, endNode);
            //��
            FindNearlyNodeToOpenList(startNode.x + 1, startNode.y, 1, startNode, endNode);
            //����
            FindNearlyNodeToOpenList(startNode.x - 1, startNode.y - 1, 1.4f, startNode, endNode);
            //��
            FindNearlyNodeToOpenList(startNode.x, startNode.y - 1, 1, startNode, endNode);
            //����
            FindNearlyNodeToOpenList(startNode.x + 1, startNode.y - 1, 1.4f, startNode, endNode);

            //�����б�Ϊ�� ��û�ҵ��յ� ��·
            if (openList.Count == 0)
            {
                Debug.Log("��·");
                return null;
            }
            //ѡ�������б��� Ѱ·������С�ĵ�
            //openList.Sort(SortOpenList);
            openList.Sort((a, b) =>
            {
                if (a.f > b.f)
                    return 1;
                else if (a.f == b.f)
                    return 1;
                else return -1;
            });
            //����ر��б� �ӿ����б��Ƴ�
            closeList.Add(openList[0]);
            //�ҵ��� ����µ���� ������һ�μ���
            startNode = openList[0];
            openList.RemoveAt(0);

            if (startNode == endNode)
            {
                //���յ� ���ؽ��
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
    /// ���ٽ�����뿪���б�
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="g">Ѱ·���� �븸����ľ���</param>
    /// <param name="fatherNode"></param>
    /// <param name="endNode"></param>
    private void FindNearlyNodeToOpenList(int x, int y, float g, AStarNode fatherNode, AStarNode endNode)
    {
        if (x < 0 || y < 0 || x >= mapW || y >= mapH) return;

        AStarNode node = nodes[x, y];
        //�жϵ��Ƿ�߽� �赲 �ڿ�����ر��б�
        if (node == null || node.type == ENode_Type.Stop) return;
        //����ڵ��Ѿ��ڿ����б��� ����һ����Сֵ��Ҳ�����㵽�õ� ��ô�õ�ĸ��ڵ㲻���Ϊ�����Сֵ�� �ͻᵼ��·��������̵� Ӧ�û�Ҫ����ж�
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
        //����fֵ
        node.father = fatherNode;
        node.g = fatherNode.g + g;
        node.h = Mathf.Abs(endNode.x - node.x) + Mathf.Abs(endNode.y - node.y);
        node.f = node.g + node.h;

        //ͨ����֤ ���뿪���б�
        openList.Add(node);
    }
}