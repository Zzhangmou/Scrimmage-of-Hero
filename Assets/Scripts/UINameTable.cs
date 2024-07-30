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
                    //����һ���ֵ�ʵ���������ַ�����ֵ�� GameObject��ʹ�� StringComparer.Ordinal ����ȷ�����ıȽ��ǻ���ÿ���ַ��� Unicode ����ֵ������һ�����������ִ�Сд�ıȽϷ�ʽ��
                    //StringComparer.Ordinal ��һ���ַ����Ƚ��������ڶ��ֵ�ļ����бȽϡ�Ordinal �Ƚ����ǻ����ַ������ֽ�����бȽϵģ������ıȽϷ�ʽ���������Ļ��ʹ�Сдת����
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
