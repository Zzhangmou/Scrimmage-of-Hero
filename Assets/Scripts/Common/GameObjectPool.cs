using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ʹ�÷�ʽ��
 * 1.������ҪƵ������/���ٵ�����,ͨ������ش���/����
 * 2.��Ҫͨ������ش�����������ÿ�δ���ʱִ��XX ��Ҫ�ű�ʵ��IResetale�ӿ�
 */

namespace Common
{
    /// <summary>
    /// ������
    /// </summary>
    public interface IResetable
    {
        void OnReset();
    }

    /// <summary>
    /// �����
    /// </summary>
    public class GameObjectPool : MonoSingleton<GameObjectPool>
    {
        /// <summary>
        /// �����
        /// </summary>
        private Dictionary<string, List<GameObject>> cache;
        public override void Init()
        {
            base.Init();
            cache = new Dictionary<string, List<GameObject>>();
        }
        /// <summary>
        /// ͨ������ش�������
        /// </summary>
        /// <param name="key">���</param>
        /// <param name="prefab">��Ҫ����ʵ����Ԥ�Ƽ�</param>
        /// <param name="pos">λ��</param>
        /// <param name="rotate">��ת</param>
        /// <returns></returns>
        public GameObject CreateObject(string key, GameObject prefab, Vector3 pos, Quaternion rotate)
        {

            GameObject go = null;
            go = FindUsableObject(key);
            if (go == null)
            {
                go = AddObject(key, prefab);
            }
            //ʹ��
            UserObject(pos, rotate, go);
            return go;
        }
        /// <summary>
        /// ���ն���
        /// </summary>
        /// <param name="go">��Ҫ�����յĶ���</param>
        /// <param name="delay">�ӳ�ʱ�� Ĭ��0</param>
        public void CollectObject(GameObject go, float delay = 0)
        {
            StartCoroutine(CollectObjectDelay(go, delay));
        }
        /// <summary>
        /// ���ĳ�����
        /// </summary>
        /// <param name="key"></param>
        public void Clear(string key)
        {
            for (int i = cache[key].Count - 1; i >= 0; i--)
            {
                Destroy(cache[key][i]);
            }
            cache.Remove(key);
        }
        /// <summary>
        /// ���ȫ��
        /// </summary>
        public void ClearAll()
        {
            List<string> keyList = new List<string>(cache.Keys);
            foreach (var item in keyList)
            {
                Clear(item);
            }
        }

        #region ϸ��
        //ʹ�ö���
        private void UserObject(Vector3 pos, Quaternion rotate, GameObject go)
        {
            go.transform.position = pos;
            go.transform.rotation = rotate;
            go.SetActive(true);

            foreach (var item in go.GetComponents<IResetable>())
            {
                item.OnReset();
            }
        }
        //��Ӷ���
        private GameObject AddObject(string key, GameObject prefab)
        {
            //����
            GameObject go = Instantiate(prefab);
            //�������
            //�������û��key ��Ӽ�¼
            if (!cache.ContainsKey(key)) cache.Add(key, new List<GameObject>());
            cache[key].Add(go);
            return go;
        }
        //����ָ������� ����ʹ�õĶ���
        private GameObject FindUsableObject(string key)
        {
            if (cache.ContainsKey(key))
                return cache[key].Find(g => !g.activeInHierarchy);
            return null;
        }
        private IEnumerator CollectObjectDelay(GameObject go, float delay)
        {
            yield return new WaitForSeconds(delay);
            go.SetActive(false);
        }
        #endregion
    }
}