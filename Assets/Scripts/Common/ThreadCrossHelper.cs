using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// �߳̽������������
    /// </summary>
    public class ThreadCrossHelper : MonoSingleton<ThreadCrossHelper>
    {
        class DelayedItem
        {
            public Action CurrentAction { get; set; }
            public DateTime Time { get; set; }
        }

        private List<DelayedItem> actionList;

        public override void Init()
        {
            base.Init();
            actionList = new List<DelayedItem>();
        }

        private void Update()
        {
            lock (actionList)
            {
                for (int i = actionList.Count - 1; i >= 0; i--)
                {
                    //������ֵ���ִ��ʱ�� ��
                    if (actionList[i].Time <= DateTime.Now)
                    {
                        //ִ��
                        actionList[i].CurrentAction();
                        //�Ƴ�
                        actionList.RemoveAt(i);
                    }
                }
            }
        }

        public void ExecuteOnMainThread(Action action, float delay = 0)
        {
            lock (actionList)
            {
                var item = new DelayedItem()
                {
                    CurrentAction = action,
                    Time = DateTime.Now.AddSeconds(delay)
                };
                actionList.Add(item);
            }
        }
    }
}