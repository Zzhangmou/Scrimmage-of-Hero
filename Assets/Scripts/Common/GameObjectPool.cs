using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// 对象池
    /// </summary>
    public class GameObjectPool : MonoSingleton<GameObjectPool>
    {
        /// <summary>
        /// 对象池
        /// </summary>
        private Dictionary<string, List<GameObject>> cache;
        public override void Init()
        {
            base.Init();
            cache = new Dictionary<string, List<GameObject>>();
        }
        /// <summary>
        /// 通过对象池创建对象
        /// </summary>
        /// <param name="key">类别</param>
        /// <param name="prefab">需要创建实例的预制件</param>
        /// <param name="pos">位置</param>
        /// <param name="rotate">旋转</param>
        /// <returns></returns>
        public GameObject CreateObject(string key, GameObject prefab, Vector3 pos, Quaternion rotate)
        {

            GameObject go = null;
            go = FindUsableObject(key);
            if (go == null)
            {
                go = AddObject(key, prefab);
            }
            //使用
            UserObject(pos, rotate, go);
            return go;
        }
        /// <summary>
        /// 回收对象
        /// </summary>
        /// <param name="go">需要被回收的对象</param>
        /// <param name="delay">延迟时间 默认0</param>
        public void CollectObject(GameObject go, float delay = 0)
        {
            StartCoroutine(CollectObjectDelay(go, delay));
        }
        /// <summary>
        /// 清空某个类别
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
        /// 清空全部
        /// </summary>
        public void ClearAll()
        {
            foreach (var item in cache.Keys)
            {
                Clear(item);
            }
        }

        #region 细节
        //使用对象
        private void UserObject(Vector3 pos, Quaternion rotate, GameObject go)
        {
            go.transform.position = pos;
            go.transform.rotation = rotate;
            go.SetActive(true);
        }

        //添加对象
        private GameObject AddObject(string key, GameObject prefab)
        {
            //创建
            GameObject go = Instantiate(prefab);
            //加入池中
            //如果池中没有key 添加记录
            if (!cache.ContainsKey(key)) cache.Add(key, new List<GameObject>());
            cache[key].Add(go);
            return go;
        }
        //查找指定类别中 可以使用的对象
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

