using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateMultiDisplay : MonoBehaviour
{
    public GameObject cameraParamsPrefab;
    public Transform CanvasListParent;
    private string targetCamName;
    private GameObject cameraEntry;
    private List<GameObject> activeCamerasList= new List<GameObject>();
    private bool bViosoActive = false;

    //activate GPU displays and list them as cameras
    void Start()
    {
        for (int i = 0; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
            //Store a camera for each display
            GameObject camObj = GameObject.Find(String.Concat("cam", (i + 1)));
            activeCamerasList.Add(camObj);
            
            // Instantiate a UI camera entry and link it to a unity camera 
            cameraEntry=Instantiate(cameraParamsPrefab, Vector3.zero, Quaternion.identity, CanvasListParent);
            targetCamName =camObj.name;
            cameraEntry.GetComponent<FrustumControl>().cameraName.text=targetCamName;
            cameraEntry.GetComponent<FrustumControl>().targetCamera= camObj.GetComponent<Camera>();

        }
    }

    public void ToggleViosoPlugin()
    {
        bViosoActive = !bViosoActive;

        if (bViosoActive) //enable VIOSO
        {
            foreach (GameObject cameraObject in activeCamerasList)
            {
                cameraObject.GetComponent<VIOSOCamera>().enabled = true;
            }
            cameraEntry.GetComponent<FrustumControl>().WriteCameraViewToUI();
        }
        else
        {
            foreach (GameObject cameraObject in activeCamerasList)
            {
                cameraObject.GetComponent<VIOSOCamera>().enabled = false;
            }
            cameraEntry.GetComponent<FrustumControl>().WriteCameraViewToUI();
        }


    }


}
