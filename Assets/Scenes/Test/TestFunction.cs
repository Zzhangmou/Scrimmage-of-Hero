using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

namespace ns
{
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    /// <summary>
    /// ���Խű�
    /// </summary>
    public class TestFunction : MonoBehaviour
    {
        void Start()
        {
            //��ȡ����  
            MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
            Material[] materials = new Material[meshRenderers.Length];
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                materials[i] = meshRenderers[i].sharedMaterial;
            }
            //��ȡmesh��ʹ��CombineInstance������ΪCombineMeshes��������Ҫ  
            MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
            CombineInstance[] combineInstances = new CombineInstance[meshFilters.Length];
            for (int i = 0; i < meshFilters.Length; i++)
            {
                combineInstances[i].mesh = meshFilters[i].sharedMesh;
                //ģ�Ϳռ�����ת��Ϊ��������  
                combineInstances[i].transform = meshFilters[i].transform.localToWorldMatrix;
                //����������  
                meshFilters[i].gameObject.SetActive(false);
            }
            //�ϲ�����  
            transform.GetComponent<MeshRenderer>().sharedMaterials = materials;
            //�ϲ�����  
            transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combineInstances, false);
            transform.gameObject.SetActive(true);
        }
    }
}
