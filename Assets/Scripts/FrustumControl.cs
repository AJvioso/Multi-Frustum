using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

        //Lock frustum buttons in VIOSO mode: read-only
        if (SceneManager.GetActiveScene().buildIndex != 0) 
        {
            inputFOV_L.interactable = false;
            inputFOV_R.interactable = false;
            inputFOV_T.interactable = false;
            inputFOV_B.interactable = false; 
            inputDir_P.interactable = false;
            inputDir_Y.interactable = false;
            inputDir_R.interactable = false;

        }

        //run delayed start next to read the views
        StartCoroutine(DelayedStart());

    }


    /// <summary>
    /// Delayed start to allow VIOSO views to initialize before updating UI
    /// </summary>
    /// <returns></returns>
    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(1);
        WriteCameraViewToUI();
    }

   
    void UpdateFOV()
    {
        SetFrustum(targetCamera, float.Parse(inputFOV_L.text), float.Parse(inputFOV_R.text), float.Parse(inputFOV_B.text), float.Parse(inputFOV_T.text));
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


    /// <summary>
    /// Get the camera projection matrix, calculate its FOV values and write to UI
    /// </summary>
    public void WriteCameraViewToUI()
    {
        
        inputDir_P.text = (-1f* targetCamera.transform.eulerAngles.x).ToString(); //match VIOSO coordinate system
        inputDir_Y.text = targetCamera.transform.eulerAngles.y.ToString();
        inputDir_R.text = targetCamera.transform.eulerAngles.z.ToString();

        // Get projection matrix and calculate field of view values
        Matrix4x4 projectionMatrix = targetCamera.projectionMatrix;

        float near = 1f;

        float left = -near * (projectionMatrix[0, 2] + 1) / projectionMatrix[0, 0];
        float right = near * (1 - projectionMatrix[0, 2]) / projectionMatrix[0, 0];
        float bottom = -near * (projectionMatrix[1, 2] + 1) / projectionMatrix[1, 1];
        float top = near * (1 - projectionMatrix[1, 2]) / projectionMatrix[1, 1];

        // Calculate FOVs in degrees
        float leftFOV = Mathf.Atan(left / near) * Mathf.Rad2Deg *-1f; // all angles positive in UI
        float rightFOV = Mathf.Atan(right / near) * Mathf.Rad2Deg ;
        float bottomFOV = Mathf.Atan(bottom / near) * Mathf.Rad2Deg *-1f;
        float topFOV = Mathf.Atan(top / near) * Mathf.Rad2Deg ;

        // Write to UI
        inputFOV_B.text = bottomFOV.ToString();
        inputFOV_T.text = topFOV.ToString();
        inputFOV_L.text = leftFOV.ToString();
        inputFOV_R.text = rightFOV.ToString();


    }
}
