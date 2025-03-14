using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EvaluationModule : MonoBehaviour
{
    private static String _filename;
    private void Start()
    {

        Directory.CreateDirectory(Application.streamingAssetsPath + "/User_Logs/");
        _filename = Application.streamingAssetsPath + "/User_Logs/log_1.txt";
        if (!File.Exists(_filename))
        {
            File.WriteAllText(_filename, "Start Log At: " + Time.realtimeSinceStartup + "\n");
        }
    }

    public static void LogEntry(String entry)
    {
        File.AppendAllText(_filename, "[" + Time.realtimeSinceStartup + "]" + entry + "\n");
    }

    private void OnApplicationQuit()
    {
        foreach (EvaluationTrigger trigger in FindObjectsOfType<EvaluationTrigger>())
        {
            if (!trigger.EnteredTrigger())
                File.AppendAllText(_filename,
                    "[" + Time.realtimeSinceStartup + "] Trigger " + trigger.TriggerLabel + " was not triggered.\n");
        }
        File.AppendAllText(_filename, "[" + Time.realtimeSinceStartup + "] Game Closed.\n");
    }
}
