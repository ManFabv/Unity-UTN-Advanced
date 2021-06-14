using System;
using UnityEngine;

public static class Logger
{
    public static void LogError(GameObject go, Type goType, string paramName)
    {
        if(go == null)
            Debug.LogError("EL " + goType + " ES NULO EN " + paramName);
    }
}