using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameServer.Common
{
    public class AsyncTimer : Timer
    {
        class AsyncTaskPack
        {
            public int tid;
            public Action<int> cb;
            public AsyncTaskPack(int tid, Action<int> cb)
            {
                this.tid = tid;
                this.cb = cb;
            }
        }

        private bool setHandle;
        private readonly ConcurrentDictionary<int, AsyncTask> taskDic;
        private ConcurrentQueue<AsyncTaskPack> packQue;
        private const string tidLock = "AsyncTimer_tidLock";

        public AsyncTimer(bool setHandle)
        {
            taskDic = new ConcurrentDictionary<int, AsyncTask>();
            this.setHandle = setHandle;
            if (setHandle)
            {
                packQue = new ConcurrentQueue<AsyncTaskPack>();
            }
        }

        public override int AddTask(
            uint delay,
            Action<int> taskCB,
            Action<int> cancelCB,
            int count = 1)
        {
            int tid = GenerateTid();
            AsyncTask task = new AsyncTask(tid, delay, count, taskCB, cancelCB);
            RunTaskInPool(task);

            if (taskDic.TryAdd(tid, task))
            {
                return tid;
            }
            else
            {
                WarnFunc?.Invoke($"key:{tid} already exist.");
                return -1;
            }
        }
        public override bool DeleteTask(int tid)
        {
            if (taskDic.TryRemove(tid, out AsyncTask task))
            {
                LogFunc?.Invoke($"Remvoe tid:{task.tid} task in taskDic Succ.");

                task.cts.Cancel();

                if (setHandle && task.cancelCB != null)
                {
                    packQue.Enqueue(new AsyncTaskPack(task.tid, task.cancelCB));
                }
                else
                {
                    task.cancelCB?.Invoke(task.tid);
                }
                return true;
            }
            else
            {
                ErrorFunc?.Invoke($"Remove tid:{task.tid} task in taskDic failed.");
                return false;
            }
        }
        public override void Reset()
        {
            if (packQue != null && !packQue.IsEmpty)
            {
                WarnFunc?.Invoke("Call Queue is not Empty.");
            }
            taskDic.Clear();
            tid = 0;
        }

        public void HandleTask()
        {
            while (packQue != null && packQue.Count > 0)
            {
                if (packQue.TryDequeue(out AsyncTaskPack pack))
                {
                    pack.cb?.Invoke(pack.tid);
                }
                else
                {
                    WarnFunc?.Invoke($"packQue dequeue data failed.");
                }
            }
        }

        void RunTaskInPool(AsyncTask task)
        {
            Task.Run(async () =>
            {
                if (task.count > 0)
                {
                    do
                    {
                        //限次数循环任务
                        --task.count;
                        ++task.loopIndex;
                        int delay = (int)(task.delay + task.fixDelta);
                        if (delay > 0)
                        {
                            await Task.Delay(delay, task.ct);
                        }
                        TimeSpan ts = DateTime.UtcNow - task.startTime;
                        task.fixDelta = (int)(task.delay * task.loopIndex - ts.TotalMilliseconds);
                        CallBackTaskCB(task);
                    } while (task.count > 0);
                }
                else
                {
                    //永久循环任务
                    while (true)
                    {
                        //限次数循环任务
                        ++task.loopIndex;
                        int delay = (int)(task.delay + task.fixDelta);
                        if (delay > 0)
                        {
                            await Task.Delay(delay, task.ct);
                        }
                        TimeSpan ts = DateTime.UtcNow - task.startTime;
                        task.fixDelta = (int)(task.delay * task.loopIndex - ts.TotalMilliseconds);
                        CallBackTaskCB(task);
                    }
                }
            });
        }
        void CallBackTaskCB(AsyncTask task)
        {
            if (setHandle)
            {
                packQue.Enqueue(new AsyncTaskPack(task.tid, task.taskCB));
            }
            else
            {
                task.taskCB.Invoke(task.tid);
            }

            if (task.count == 0)
            {
                if (taskDic.TryRemove(task.tid, out AsyncTask temp))
                {
                    LogFunc?.Invoke($"Task tid:{task.tid} run to completion.");
                }
                else
                {
                    ErrorFunc?.Invoke($"Remove tid:{task.tid} task in taskDic failed.");
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

        class AsyncTask
        {
            public int tid;
            public uint delay;
            public int count;

            public Action<int> taskCB;
            public Action<int> cancelCB;

            public DateTime startTime;
            public ulong loopIndex;
            public int fixDelta;

            public CancellationTokenSource cts;
            public CancellationToken ct;

            public AsyncTask(
                int tid,
                uint delay,
                int count,
                Action<int> taskCB,
                Action<int> cancelCB)
            {
                this.tid = tid;
                this.delay = delay;
                this.count = count;
                this.taskCB = taskCB;
                this.cancelCB = cancelCB;

                startTime = DateTime.UtcNow;

                this.loopIndex = 0;
                this.fixDelta = 0;
                cts = new CancellationTokenSource();
                ct = cts.Token;
            }
        }
    }
}
