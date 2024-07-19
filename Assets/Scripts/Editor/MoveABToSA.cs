using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class MoveABToSA
{
    //[MenuItem("Tools/AB/�ƶ���Դ��StreamAssets")]
    private static void MoveABToStreamAssets()
    {
        //ͨ���༭��Selection���еķ��� ��ȡ��Project������ѡ�е���Դ 
        Object[] selectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        //���һ����Դ��û��ѡ�� ��û�б�Ҫ���������߼���
        if (selectedAsset.Length == 0)
            return;
        //����ƴ�ӱ���Ĭ��AB����Դ��Ϣ���ַ���
        string abCompareInfo = "";
        //����ѡ�е���Դ����
        foreach (Object asset in selectedAsset)
        {
            //ͨ��Assetdatabase�� ��ȡ ��Դ��·��
            string assetPath = AssetDatabase.GetAssetPath(asset);
            //��ȡ·�����е��ļ��� ������Ϊ StreamingAssets�е��ļ���
            string fileName = assetPath.Substring(assetPath.LastIndexOf('/'));

            //�ж��Ƿ���.���� ����� ֤���к�׺ ������
            if (fileName.IndexOf('.') != -1)
                continue;
            //�㻹�����ڿ���֮ǰ ȥ��ȡȫ·�� Ȼ��ͨ��FIleInfoȥ��ȡ��׺���ж� �������ӵ�׼ȷ

            //����AssetDatabase�е�API ��ѡ���ļ� ���Ƶ�Ŀ��·��
            AssetDatabase.CopyAsset(assetPath, "Assets/StreamingAssets" + fileName);

            //��ȡ������StreamingAssets�ļ����е��ļ���ȫ����Ϣ
            FileInfo fileInfo = new FileInfo(Application.streamingAssetsPath + fileName);
            //ƴ��AB����Ϣ���ַ�����
            abCompareInfo += fileInfo.Name + " " + fileInfo.Length + " " + GenerateABCompare.GetMD5(fileInfo.FullName);
            //��һ�����Ÿ������AB����Ϣ
            abCompareInfo += "|";
        }
        //ȥ�����һ��|���� Ϊ��֮�����ַ�������
        abCompareInfo = abCompareInfo.Substring(0, abCompareInfo.Length - 1);
        //������Ĭ����Դ�ĶԱ���Ϣ �����ļ�
        File.WriteAllText(Application.streamingAssetsPath + "/ABCompareInfo.txt", abCompareInfo);
        //ˢ�´���
        AssetDatabase.Refresh();
    }
}
