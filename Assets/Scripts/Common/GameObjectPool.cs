using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
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
            if (!cache.ContainsKey(key)) return;
            foreach (var item in cache[key])
            {
                Destroy(item.gameObject);
            }
            cache.Remove(key);
        }
        /// <summary>
        /// ���ȫ��
        /// </summary>
        public void ClearAll()
        {
            foreach (var item in cache.Keys)
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

