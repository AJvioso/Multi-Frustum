using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateMultiDisplay : MonoBehaviour
{
    public GameObject[] cameraList;
    public GameObject cameraParamsPrefab;
    public Transform CanvasListParent;
    private string targetCamName;
    private GameObject cameraEntry;
    private List<GameObject> activeCamerasList= new List<GameObject>();


    //activate GPU displays and corresponding cameras for them
    void Start()
    {
        for (int i = 0; i < Display.displays.Length; i++)
        {
            //activate display and camera
            Display.displays[i].Activate();
            cameraList[i+1].SetActive(true);
            
            // Instantiate a UI camera entry and link it to a unity camera 
            cameraEntry=Instantiate(cameraParamsPrefab, Vector3.zero, Quaternion.identity, CanvasListParent);
            targetCamName = cameraList[i + 1].name;
            cameraEntry.GetComponent<FrustumControl>().cameraName.text=targetCamName;
            cameraEntry.GetComponent<FrustumControl>().targetCamera= cameraList[i+1].GetComponent<Camera>();

        }
    }




}
