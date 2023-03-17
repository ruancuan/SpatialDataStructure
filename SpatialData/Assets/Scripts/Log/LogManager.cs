using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS.QuadTree
{
    public class LogManager : MonoSingleton<LogManager>
    {
        public void Log(string context)
        {
            Debug.Log(context);
        }

        public void LogWarn(string context)
        {
            Debug.LogWarning(context);
        }

        public void LogError(string context)
        {
            Debug.LogError(context);
        }
    }
}
