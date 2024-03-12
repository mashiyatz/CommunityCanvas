using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class debugLogViewer : MonoBehaviour
{
    public TMP_Text debugLogText;

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logText, string stackTrace, LogType logType)
    {
        // Append the new log message to the existing text
        debugLogText.text += logText + "\n";
    }
}
