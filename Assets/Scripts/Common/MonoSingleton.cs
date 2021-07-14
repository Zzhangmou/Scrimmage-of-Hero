using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 备注:
 * 1.适用性:场景中存在唯一的对象,即可让该对象继承当前类.
 * 2.如何使用:
 *      --继承时必须传递子类类型.
 *      --在任意脚本生命周期中,通过子类类型访问Instance属性.
 */
namespace Common
{
    /// <summary>
    ///脚本单例类
    ///</summary>
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    //在场景中根据类型查找引用
                    instance = FindObjectOfType<T>();
                    if (instance == null)
                    {
                        //创建脚本对象（立即执行Awake）
                        new GameObject("Singleton of " + typeof(T)).AddComponent<T>();
                    }
                    else
                    {
                        instance.Init();
                    }
                }
                return instance;
            }
        }

        protected void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
                Init();
            }
        }

        public virtual void Init()
        {

        }
    }
}