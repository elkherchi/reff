using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public static class EditorDebugger
{
    [Conditional("ENABLE_LOG")]
    public static void Log(object message)
    {
        Debug.Log(message);
    }

    [Conditional("ENABLE_LOG")]
    public static void Log(object message, Object context)
    {
        Debug.Log(message, context);
    }

    [Conditional("ENABLE_LOG")]
    public static void LogWarning(object message)
    {
        Debug.LogWarning(message);
    }

    [Conditional("ENABLE_LOG")]
    public static void LogWarning(object message, Object context)
    {
        Debug.LogWarning(message, context);
    }

}
