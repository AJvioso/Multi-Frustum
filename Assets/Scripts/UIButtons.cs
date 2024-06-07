using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtons : MonoBehaviour
{

    /// <summary>
    /// Toggle function used by UI buttons for several objects
    /// </summary>
    /// <param name="elementToToggle"></param>
    public void ToggleObject(GameObject elementToToggle)
    {
        elementToToggle.SetActive(!elementToToggle.activeSelf);
    }

    /// <summary>
    /// shut down the app
    /// </summary>
    public void ExitButton()
    {
        Application.Quit();
    }


    /// <summary>
    /// RestartButton reloads the current scene
    /// </summary>
    public void RestartButton()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }


    /// <summary>
    /// switch between two scenes: Manual frustun & VIOSO plugin
    /// </summary>
    public void ToggleVIOSO()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) { SceneManager.LoadScene("VIOSO_Frustum"); }
        else
            SceneManager.LoadScene("Manual_Frustum");

    }

    /// <summary>
    /// Open VIOSOWarpBlend.ini file in notepad
    /// </summary>
    public void OpenIniFileButton()
    {
        string absoluteFilePath;

        if (Application.isEditor)
        {
            absoluteFilePath = Path.Combine(Application.dataPath, "plugins/vioso/VIOSOwarpblend.ini");
        }
        else
        {
            absoluteFilePath = Path.Combine(Application.dataPath, "../", "MultiFrustum_Data/Plugins/x86_64/VIOSOwarpblend.ini");
        }

        Process.Start("notepad.exe", absoluteFilePath);
    }


    /// <summary>
    /// Open VIOSOWarpBlend.log file in notepad
    /// </summary>
    public void OpenLogFileButton()
    {
        string absoluteFilePath;

        if (Application.isEditor)
        {
            absoluteFilePath = Path.Combine(Application.dataPath, "plugins/vioso/VIOSO_Plugin64.log");
        }
        else
        {
            absoluteFilePath = Path.Combine(Application.dataPath, "../", "MultiFrustum_Data/Plugins/x86_64/VIOSO_Plugin64.log");
        }

        Process.Start("notepad.exe", absoluteFilePath);
    }


}
