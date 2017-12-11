using UnityEngine;
using UnityEditor;
using System.IO;

[InitializeOnLoad]
class CompileTime : EditorWindow
{
    #region Attributes
    private static bool isTrackingTime;
    private static double startTime;
    #endregion

    #region Core
    /// <summary>
    /// Compte le temps de compilation
    /// </summary>
    static CompileTime()
    {
        EditorApplication.update += Update;
        startTime = PlayerPrefs.GetFloat("CompileStartTime", 0);
        if (startTime > 0)
        {
            isTrackingTime = true;
        }
    }
    #endregion

    #region Core
    private static void Update()
    {
        if (EditorApplication.isCompiling && !isTrackingTime)
        {
            startTime = EditorApplication.timeSinceStartup;
            PlayerPrefs.SetFloat("CompileStartTime", (float)startTime);
            if (!PlayerPrefs.HasKey("TotalCompileTime"))
                PlayerPrefs.SetFloat("TotalCompileTime", (float)0);
            isTrackingTime = true;
        }
        else if (!EditorApplication.isCompiling && isTrackingTime)
        {
            var finishTime = EditorApplication.timeSinceStartup;
            isTrackingTime = false;
            var compileTime = finishTime - startTime;
            PlayerPrefs.DeleteKey("CompileStartTime");
            
            if (PlayerPrefs.HasKey("TotalCompileTime"))
            {
                float totalTime = PlayerPrefs.GetFloat("TotalCompileTime");
                totalTime += (float)compileTime;
                PlayerPrefs.SetFloat("TotalCompileTime", totalTime);
                Debug.Log("TotalCompileTime: " + totalTime.ToString("0.000") + "s");
            }
            Debug.Log("Compilation time: " + compileTime.ToString("0.000") + "s");
        }
    }
    #endregion
}