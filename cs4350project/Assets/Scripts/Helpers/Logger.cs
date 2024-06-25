using UnityEngine;

public enum LogLevel
{
    LOG,
    WARNING,
    ERROR
}
public static class Logger
{
    private static string LogObjectFormat = "<color=purple>[Class]</color> <color=blue>[{0}]</color> <color=green>[{1}]</color> {2}"; // class name, object name, message
    private static string LogSingletonFormat = "<color=purple>[Singleton]</color> <color=blue>[{0}]</color> {1}"; // class name, message

    public static void Log(string singletonName, string message, LogLevel level)
    {
        Log(level, string.Format(LogSingletonFormat, singletonName, message));
    }

    public static void Log(string className, string objectName, string message, Object context, LogLevel level)
    {
        Log(level, string.Format(LogObjectFormat, className, objectName, message), context);
    }

    private static void Log(LogLevel log, string message)
    {
        switch (log)
        {
            case LogLevel.LOG:
                Debug.Log(message);
                break;
            case LogLevel.WARNING:
                Debug.LogWarning(message);
                break;
            case LogLevel.ERROR:
                Debug.LogError(message);
                break;
        }
    }

    private static void Log(LogLevel log, string message, Object context)
    {
        switch (log)
        {
            case LogLevel.LOG:
                Debug.Log(message, context);
                break;
            case LogLevel.WARNING:
                Debug.LogWarning(message, context);
                break;
            case LogLevel.ERROR:
                Debug.LogError(message, context);
                break;
        }
    }
}