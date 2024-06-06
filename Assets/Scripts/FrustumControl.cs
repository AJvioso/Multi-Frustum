using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.Events;

public class FrustumControl : MonoBehaviour
{

    public InputField inputFOV_L, inputFOV_R, inputFOV_B, inputFOV_T, inputDir_P, inputDir_Y, inputDir_R;
    public Text cameraName;
    private float leftDist, rightDist, topDist, bottomDist;
    private Vector3 inputDir=Vector3.zero;
    public Camera targetCamera;



    void Start()
    {
        // Setup UI listeners
        inputFOV_L.onEndEdit.AddListener(delegate { UpdateFOV(); });
        inputFOV_R.onEndEdit.AddListener(delegate { UpdateFOV(); });
        inputFOV_T.onEndEdit.AddListener(delegate { UpdateFOV(); });
        inputFOV_B.onEndEdit.AddListener(delegate { UpdateFOV(); });
        inputDir_P.onEndEdit.AddListener(delegate { UpdateDir(); });
        inputDir_Y.onEndEdit.AddListener(delegate { UpdateDir(); });
        inputDir_R.onEndEdit.AddListener(delegate { UpdateDir(); });

    }

   
    void UpdateFOV()
    {
        SetFrustum(targetCamera, float.Parse(inputFOV_L.text), float.Parse(inputFOV_R.text), float.Parse(inputFOV_T.text), float.Parse(inputFOV_B.text));
    }

    void UpdateDir()
    {
        inputDir = new Vector3(-1f*float.Parse(inputDir_P.text), float.Parse(inputDir_Y.text), float.Parse(inputDir_R.text));
        targetCamera.transform.eulerAngles = inputDir;
    }


    /// <summary>
    /// Sets the camera frustum based on given FOV angles (positive, in degrees).
    /// </summary>
    public void SetFrustum(Camera cam, float left, float right, float bottom, float top)
    {
        leftDist = -1f * MathF.Tan(left * Mathf.Deg2Rad);
        rightDist = 1f * MathF.Tan(right * Mathf.Deg2Rad);
        bottomDist = -1f * MathF.Tan(bottom * Mathf.Deg2Rad);
        topDist = 1f * MathF.Tan(top * Mathf.Deg2Rad);

        Matrix4x4 m = PerspectiveOffCenter(leftDist, rightDist, bottomDist, topDist, 1f, 1000f);
        cam.projectionMatrix = m;
    }


    /// <summary>
    /// returns a projection matrix built from plane distances.
    /// </summary>
    static Matrix4x4 PerspectiveOffCenter(float left, float right, float bottom, float top, float near, float far)
    {
        float x = 2.0F * near / (right - left);
        float y = 2.0F * near / (top - bottom);
        float a = (right + left) / (right - left);
        float b = (top + bottom) / (top - bottom);
        float c = -(far + near) / (far - near);
        float d = -(2.0F * far * near) / (far - near);
        float e = -1.0F;
        Matrix4x4 m = new Matrix4x4();
        m[0, 0] = x; m[0, 1] = 0; m[0, 2] = a; m[0, 3] = 0;
        m[1, 0] = 0; m[1, 1] = y; m[1, 2] = b; m[1, 3] = 0;
        m[2, 0] = 0; m[2, 1] = 0; m[2, 2] = c; m[2, 3] = d;
        m[3, 0] = 0; m[3, 1] = 0; m[3, 2] = e; m[3, 3] = 0;
        return m;
    }

    
}
