using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class TestAStar : MonoBehaviour
{
    public int beginX;
    public int beginY;

    public int offsetX;
    public int offsetY;

    public int mapW;
    public int mapH;

    private Vector2 beginPos = Vector2.right * -1;

    private Dictionary<string, GameObject> cubes = new Dictionary<string, GameObject>();

    public Material red;
    public Material yellow;
    public Material green;
    public Material normal;

    Stack<AStarNode> list = new Stack<AStarNode>();

    private void Start()
    {
        AStarManager.Instance.InitMapInfo(mapW, mapH);

        for (int i = 0; i < mapW; i++)
        {
            for (int j = 0; j < mapH; j++)
            {
                GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                obj.transform.position = new Vector3(beginX + i * offsetX, beginY + j * offsetY, 0);

                obj.name = i + "_" + j;
                cubes.Add(obj.name, obj);

                AStarNode node = AStarManager.Instance.nodes[i, j];
                if (node.type == ENode_Type.Stop)
                {
                    obj.GetComponent<MeshRenderer>().material = red;
                }
                else
                    obj.GetComponent<MeshRenderer>().material = normal;

            }
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 1000))
            {
                if (beginPos == Vector2.right * -1)
                {
                    if (list != null)
                    {
                        foreach (AStarNode node in list)
                        {
                            cubes[node.x + "_" + node.y].gameObject.GetComponent<MeshRenderer>().material = normal;
                        }
                    }

                    string[] str = hit.collider.gameObject.name.Split('_');
                    beginPos = new Vector2(int.Parse(str[0]), int.Parse(str[1]));
                    hit.collider.gameObject.GetComponent<MeshRenderer>().material = yellow;
                }
                else
                {
                    string[] str = hit.collider.gameObject.name.Split('_');
                    Vector2 endPos = new Vector2(int.Parse(str[0]), int.Parse(str[1]));

                    //Ѱ·
                    list = AStarManager.Instance.FindPath(beginPos, endPos);

                    cubes[beginPos.x + "_" + beginPos.y].gameObject.GetComponent<MeshRenderer>().material = normal;

                    if (list != null)
                    {
                        foreach (AStarNode node in list)
                        {
                            cubes[node.x + "_" + node.y].gameObject.GetComponent<MeshRenderer>().material = green;
                        }
                    }
                    beginPos = Vector2.right * -1;
                }
            }
        }
    }
}