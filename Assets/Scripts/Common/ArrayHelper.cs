using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// 数组助手类
    /// </summary>
    public static class ArrayHelper
    {
        /// <summary>
        /// 查找满足条件的单个元素
        /// </summary>
        /// <typeparam name="T">数组类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="condition">查找条件</param>
        /// <returns></returns>
        public static T Find<T>(this T[] array, Func<T, bool> condition)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (condition(array[i]))
                {
                    return array[i];
                }
            }
            return default;
        }
        /// <summary>
        /// 查找所有满足条件的元素
        /// </summary>
        /// <typeparam name="T">数组类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="condition">满足条件</param>
        /// <returns></returns>
        public static T[] FindAll<T>(this T[] array, Func<T, bool> condition)
        {
            List<T> list = new List<T>();
            for (int i = 0; i < array.Length; i++)
            {
                if (condition(array[i]))
                {
                    list.Add(array[i]);
                }
            }
            return list.ToArray();
        }
        /// <summary>
        /// 升序方法
        /// </summary>
        /// <typeparam name="T">数组类型</typeparam>
        /// <typeparam name="Q">返回值类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="condition">比较方法</param>
        public static T[] OrderAscending<T, Q>(this T[] array, Func<T, Q> condition) where Q : IComparable
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = 0; j < array.Length - 1 - i; j++)
                {
                    if (condition(array[j]).CompareTo(condition(array[j + 1])) > 0)
                    {
                        T temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                }
            }
            return array;
        }
        /// <summary>
        /// 降序方法
        /// </summary>
        /// <typeparam name="T">数组类型</typeparam>
        /// <typeparam name="Q">返回值类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="condition">比较条件</param>
        public static T[] OrderDescending<T, Q>(this T[] array, Func<T, Q> condition) where Q : IComparable
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = 0; j < array.Length - 1 - i; j++)
                {
                    if (condition(array[j]).CompareTo(condition(array[j + 1])) < 0)
                    {
                        T temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                }
            }
            return array;
        }
        /// <summary>
        /// 求最大值
        /// </summary>
        /// <typeparam name="T">数组类型</typeparam>
        /// <typeparam name="Q">返回值类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static T FindMax<T, Q>(this T[] array, Func<T, Q> condition) where Q : IComparable
        {
            T max = array[0];
            for (int i = 1; i < array.Length; i++)
            {
                if (condition(max).CompareTo(array[i]) < 0)
                {
                    max = array[i];
                }
            }
            return max;
        }
        /// <summary>
        /// 求最小值
        /// </summary>
        /// <typeparam name="T">数组类型</typeparam>
        /// <typeparam name="Q">返回值类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static T FindMin<T, Q>(this T[] array, Func<T, Q> condition) where Q : IComparable
        {
            T max = array[0];
            for (int i = 1; i < array.Length; i++)
            {
                if (condition(max).CompareTo(array[i]) > 0)
                {
                    max = array[i];
                }
            }
            return max;
        }
        /// <summary>
        /// 筛选
        /// </summary>
        /// <typeparam name="T">进行筛选的数据类型</typeparam>
        /// <typeparam name="Q">筛选出的数据类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="condition">筛选条件</param>
        /// <returns></returns>
        public static Q[] Select<T, Q>(this T[] array, Func<T, Q> condition)
        {
            Q[] result = new Q[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = condition(array[i]);
            }
            return result;
        }
    }
}
