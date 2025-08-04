using System;
using GameServer.Common;

namespace GameServer.Service
{
    public interface ITimeService
    {
        int AddTask(uint delay, Action<int> taskCallback, Action<int> cancelCallback = null, int count = 1);

        bool DeleteTask(int taskID);
    }

    [Reflection(typeof(ITimeService))]
    public class TimeService : AbstractService, ITimeService
    {
        TickTimer _tickTimer;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _tickTimer = new TickTimer(0, false);
            _tickTimer.LogFunc = str => Debug.Log(str);
            _tickTimer.WarnFunc = str => Debug.Warn(str);
            _tickTimer.ErrorFunc = str => Debug.Error(str);
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            _tickTimer.UpdateTask();
        }

        public int AddTask(uint delay, Action<int> taskCallback, Action<int> cancelCallback = null, int count = 1)
        {
            return _tickTimer.AddTask(delay, taskCallback, cancelCallback, count);
        }

        public bool DeleteTask(int taskID)
        {
            return _tickTimer.DeleteTask(taskID);
        }
    }
}