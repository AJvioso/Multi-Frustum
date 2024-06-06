using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.EventSystems;
using System.Runtime.InteropServices;

[HelpURL("https://helpdesk.vioso.com/documentation/integrate-3d-engines/unity/")]

public class VIOSOCamera : MonoBehaviour
{

    #region Inspector Attributes
    [Header("Custom Parameters ")]

    [Tooltip("Disables the frame by frame view calculations.")]
    public bool staticEyepoint = false;

    [Tooltip("VIOSOWarpBlend.ini configuration file path. Defaults to the plugin (.dll) directory")]
    public string customIniPath = ""; // example =@"D:\Calibration\VIOSOWarpBlend.ini"
    #endregion

    #region VIOSO variables
    private Camera cam;
    private int viosoID = -1;
    private int b3D = 1;
    private bool bDynamic = true;
    private Quaternion orig_rot = Quaternion.identity;
    private Vector3 orig_pos = Vector3.zero;
    private Dictionary<RenderTexture, IntPtr> texMap = new Dictionary<RenderTexture, IntPtr>();

    public enum ERROR
    {
        NONE = 0,         /// No error, we succeeded
        GENERIC = -1,     /// a generic error, this might be anything, check log file
        PARAMETER = -2,   /// a parameter error, provided parameter are missing or inappropriate
        INI_LOAD = -3,    /// ini could notbe loaded
        BLEND = -4,       /// blend invalid or coud not be loaded to graphic hardware, check log file
        WARP = -5,        /// warp invalid or could not be loaded to graphic hardware, check log file
        SHADER = -6,      /// shader program failed to load, usually because of not supported hardware, check log file
        VWF_LOAD = -7,    /// mappings file broken or version mismatch
        VWF_FILE_NOT_FOUND = -8, /// cannot find mapping file
        NOT_IMPLEMENTED = -9,     /// Not implemented, this function is yet to come
        NETWORK = -10,        /// Network could not be initialized
        FALSE = -16,        /// No error, but nothing has been done
    };

    ERROR globalError = ERROR.FALSE;

    #endregion

    #region DLL imports

    [DllImport("VIOSO_Plugin64")]
    private static extern IntPtr GetRenderEventFunc();
    [DllImport("VIOSO_Plugin64")]
    private static extern ERROR Init(ref int id, string name, string pathToIni); 
    [DllImport("VIOSO_Plugin64")]
    private static extern ERROR UpdateTex(int id, IntPtr texHandleSrc, IntPtr texHandleDest );
    [DllImport("VIOSO_Plugin64")]
    private static extern ERROR Destroy(int id);
    [DllImport("VIOSO_Plugin64")]
    private static extern ERROR GetError(int id, ref int err);
    [DllImport("VIOSO_Plugin64")]
    private static extern ERROR GetViewProj(int id, ref Vector3 pos, ref Vector3 rot, ref Matrix4x4 view, ref Matrix4x4 proj);
    [DllImport("VIOSO_Plugin64")]
    private static extern ERROR GetViewClip(int id, ref Vector3 pos, ref Vector3 rot, ref Matrix4x4 view, ref FrustumPlanes clip);
    [DllImport("VIOSO_Plugin64")]
    private static extern void SetTimeFromUnity(float t);
    [DllImport("VIOSO_Plugin64")]
    private static extern ERROR Is3D(int id, ref int b3D);
    #endregion


    private void Start()
    {
        cam = GetComponent<Camera>();

        orig_rot = cam.transform.localRotation;
        orig_pos = cam.transform.localPosition;


        globalError = Init(ref viosoID, cam.name,customIniPath);
        if (globalError == ERROR.NONE)
        {
            GL.IssuePluginEvent(GetRenderEventFunc(), viosoID); // this will initialize warper in Unity Graphic Library context
            int err1 = 0;
            GetError(viosoID, ref err1);
            ERROR err2;
            err2 = (ERROR)err1;
            if (err2==ERROR.NONE)
            {
                Debug.Log(string.Format("Initialization of VIOSO camera: {0}.", cam.name));
            }
            else
            {
                Debug.Log(string.Format("ERROR: VIOSO warper failed to initialize with error: {0}.", globalError));
            }

        }
        else
        {
            Debug.Log(string.Format("ERROR: VIOSO plugin failed to initialize with error type: {0}. \n Check the (.log) file for more details", globalError) );
        }

    }

    /// <summary>
    /// Calls the VIOSO dynamic view calculation before culling each frame
    /// </summary>
    private void OnPreCull()
    {
        if (-1 != viosoID && bDynamic && ERROR.NONE == globalError) 
        {
            ERROR err = Is3D(viosoID, ref b3D);
            if ( 1 == b3D && ERROR.NONE == err)
            {
                SetViosoViews();
            }

            if (staticEyepoint) { bDynamic=false; } //run the setViosoViews only once in static mode.
        }
    }


    /// <summary>
    /// VIOSO dynamic view calculation
    /// </summary>
    private void SetViosoViews()
    {
        // Tracker data for dynamic eyepoint. Defaults to origin defined in VIOSOWarpBlend.ini "Eye" & "Base" params.
        Vector3 trackerPos = new Vector3(0, 0, 0);
        Vector3 trackerRot = new Vector3(0, 0, 0);

        Matrix4x4 mV = Matrix4x4.identity;

        Matrix4x4 mP = new Matrix4x4();
        FrustumPlanes pl = new FrustumPlanes();
        if (ERROR.NONE == GetViewClip(viosoID, ref trackerPos, ref trackerRot, ref mV, ref pl))
        {
            mV = mV.transpose;
            Quaternion q = mV.rotation;
            Vector3 p = mV.GetColumn(3);
            cam.transform.localRotation = orig_rot * q;
            cam.transform.localPosition = orig_pos + p;

            mP = Matrix4x4.Frustum(pl);
            cam.projectionMatrix = mP;
        }
    }


    /// <summary>
    /// VIOSO shader call.
    /// </summary>
    private void OnRenderImage( RenderTexture source, RenderTexture destination )
    {
        RenderTexture.active = destination;
        if (-1 != viosoID)
        {
            IntPtr dst;
            if (!texMap.TryGetValue(source, out dst))
            {
                dst = source.GetNativeTexturePtr();
                texMap[source] = dst;
            }
            UpdateTex(viosoID, dst, IntPtr.Zero);
            SetTimeFromUnity(Time.timeSinceLevelLoad);
            GL.IssuePluginEvent(GetRenderEventFunc(), viosoID);
        }
    }

    private void OnDisable()
    {
        if (-1 != viosoID)
        {
            ERROR err = Destroy(viosoID);
            viosoID = -1;
        }
    }
}
