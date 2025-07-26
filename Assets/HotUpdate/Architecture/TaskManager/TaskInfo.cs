using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Task
{
    public readonly struct TaskInfo
    {
        public readonly int TaskID;
        public readonly uint Times;

        public TaskInfo(int taskID, uint times)
        {
            TaskID = taskID;
            Times = times;
        }
    }
}