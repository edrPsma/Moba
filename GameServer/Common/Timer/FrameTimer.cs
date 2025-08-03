﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Common
{
    public class FrameTimer : Timer
    {
        private ulong currentFrame;
        private readonly Dictionary<int, FrameTask> taskDic;
        private const string tidLock = "AsyncTimer_tidLock";
        private List<int> tidLst;
        public FrameTimer(ulong frameID = 0)
        {
            currentFrame = frameID;
            taskDic = new Dictionary<int, FrameTask>();
            tidLst = new List<int>();
        }

        public override int AddTask(
            uint delay,
            Action<int> taskCB,
            Action<int> cancelCB,
            int count = 1)
        {
            int tid = GenerateTid();
            ulong destFrame = currentFrame + delay;
            FrameTask task = new FrameTask(tid, delay, count, destFrame, taskCB, cancelCB);
            if (taskDic.ContainsKey(tid))
            {
                WarnFunc?.Invoke($"key:{tid} already exist.");
                return -1;
            }
            else
            {
                taskDic.Add(tid, task);
                return tid;
            }
        }

        public override bool DeleteTask(int tid)
        {
            if (taskDic.TryGetValue(tid, out FrameTask task))
            {
                if (taskDic.Remove(tid))
                {
                    task.cancelCB?.Invoke(tid);
                    return true;
                }
                else
                {
                    ErrorFunc?.Invoke($"Remove tid:{tid} in taskDic failed.");
                    return false;
                }
            }
            else
            {
                WarnFunc?.Invoke($"tid:{tid} is not exist.");
                return false;
            }
        }
        public override void Reset()
        {
            taskDic.Clear();
            tidLst.Clear();
            currentFrame = 0;
        }
        public void UpdateTask()
        {
            ++currentFrame;
            tidLst.Clear();

            foreach (var item in taskDic)
            {
                FrameTask task = item.Value;
                if (task.destFrame <= currentFrame)
                {
                    task.taskCB.Invoke(task.tid);
                    task.destFrame += task.delay;
                    --task.count;
                    if (task.count == 0)
                    {
                        tidLst.Add(task.tid);
                    }
                }
            }

            for (int i = 0; i < tidLst.Count; i++)
            {
                if (taskDic.Remove(tidLst[i]))
                {
                    LogFunc?.Invoke($"Task tid:{tidLst[i]} run to completion.");
                }
                else
                {
                    ErrorFunc?.Invoke($"Remove tid:{tidLst[i]} task in taskDic failed.");
                }
            }
        }

        protected override int GenerateTid()
        {
            lock (tidLock)
            {
                while (true)
                {
                    ++tid;
                    if (tid == int.MaxValue)
                    {
                        tid = 0;
                    }
                    if (!taskDic.ContainsKey(tid))
                    {
                        return tid;
                    }
                }
            }
        }

        class FrameTask
        {
            public int tid;
            public uint delay;
            public int count;
            public ulong destFrame;
            public Action<int> taskCB;
            public Action<int> cancelCB;
            public FrameTask(
                int tid,
                uint delay,
                int count,
                ulong destFrame,
                Action<int> taskCB,
                Action<int> cancelCB)
            {
                this.tid = tid;
                this.delay = delay;
                this.count = count;
                this.destFrame = destFrame;
                this.taskCB = taskCB;
                this.cancelCB = cancelCB;
            }
        }
    }
}
