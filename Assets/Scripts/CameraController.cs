using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CameraController : MonoBehaviour
{
    public bool translation=false;
    public bool rotation = false;
    public float moveSpeed = 5.00f;


    public float speedRotation = 1.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;


    void Update()

    {   
        // Translation : Arrow keys + WASD
        if (translation)
        {transform.Translate(moveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime, 0f, moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime); }
        
        // Rotation: Mouse Control
        if (rotation)
        {
            yaw += speedRotation * Input.GetAxis("Mouse X");
            pitch -= speedRotation * Input.GetAxis("Mouse Y");

            transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
            
        }

    }

    public void ToggleNavigation()
    {
        translation=!translation;
        rotation=!rotation;
    }
}
