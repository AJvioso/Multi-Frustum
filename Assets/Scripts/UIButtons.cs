using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtons : MonoBehaviour
{
    /// <summary>
    /// Toggle function used by UI buttons
    /// </summary>
    /// <param name="elementToToggle"></param>
    public void ToggleObject(GameObject elementToToggle)
    {
        elementToToggle.SetActive(!elementToToggle.activeSelf);
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
