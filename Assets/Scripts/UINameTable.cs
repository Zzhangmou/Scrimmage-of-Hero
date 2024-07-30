using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Nirvana;
using UnityEngine;

namespace Nirvana
{
    [AddComponentMenu("Nirvana/UI/Bind/UI Name Table")]
    public sealed class UINameTable : MonoBehaviour
    {
        [Serializable]
        public struct BindPair
        {
            [Tooltip("The name of this bind.")]
            public string Name;

            [Tooltip("The widget of this UI.")]
            public GameObject Widget;
        }

        [Tooltip("The bind list.")]
        [SerializeField]
        private BindPair[] binds;

        private Dictionary<string, GameObject> _2004_2050;

        [CompilerGenerated]
        private static Comparison<BindPair> _2004_2051;

        public Dictionary<string, GameObject> Lookup
        {
            get
            {
                if (_2004_2050 == null)
                {
                    //创建一个字典实例，键是字符串，值是 GameObject。使用 StringComparer.Ordinal 可以确保键的比较是基于每个字符的 Unicode 编码值，这是一个快速且区分大小写的比较方式。
                    //StringComparer.Ordinal 是一个字符串比较器，用于对字典的键进行比较。Ordinal 比较器是基于字符串的字节序进行比较的，是最快的比较方式，不考虑文化和大小写转换。
                    _2004_2050 = new Dictionary<string, GameObject>(StringComparer.Ordinal);
                    if (binds != null)
                    {
                        BindPair[] array = binds;
                        for (int i = 0; i < array.Length; i++)
                        {
                            BindPair bindPair = array[i];
                            _2004_2050.Add(bindPair.Name, bindPair.Widget);
                        }
                    }
                }
                return _2004_2050;
            }
        }

        public GameObject Find(string key)
        {
            if (Lookup.TryGetValue(key, out var value))
            {
                return value;
            }
            return null;
        }

        public void Sort()
        {
            Array.Sort(binds, (BindPair P_0, BindPair P_1) => P_0.Name.CompareTo(P_1.Name));
        }

        public BindPair[] Search(string key)
        {
            List<BindPair> list = new List<BindPair>();
            BindPair[] array = binds;
            for (int i = 0; i < array.Length; i++)
            {
                BindPair item = array[i];
                if (item.Name.StartsWith(key))
                {
                    list.Add(item);
                }
            }
            return list.ToArray();
        }

        [CompilerGenerated]
        private static int _00A0(BindPair P_0, BindPair P_1)
        {
            return P_0.Name.CompareTo(P_1.Name);
        }
    }
}
