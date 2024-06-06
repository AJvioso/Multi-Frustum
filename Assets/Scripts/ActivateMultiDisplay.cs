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

    //activate GPU displays and list them as cameras
    void Start()
    {
        for (int i = 0; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
            // Instantiate a UI camera entry and link it to a unity camera 
            cameraEntry=Instantiate(cameraParamsPrefab, Vector3.zero, Quaternion.identity, CanvasListParent);
            targetCamName = String.Concat("cam",(i+1));
            cameraEntry.GetComponent<FrustumControl>().cameraName.text=targetCamName;
            cameraEntry.GetComponent<FrustumControl>().targetCamera= GameObject.Find(targetCamName).GetComponent<Camera>();
        }
    }


}
