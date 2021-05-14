using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Transform PlayerLookCameraTransform;
    [SerializeField] private float HorizontalLookSensitivity = 100.0f;
    [SerializeField] private float VerticalLookSensitivity = 100.0f;
    [SerializeField] private float TopMaxVerticalLookAngles = 90.0f;
    [SerializeField] private float BottomMinVerticalLookAngles = -70.0f;
    [SerializeField] private bool InvertVerticalLook = false;
    
    [SerializeField] private string HorizontalAxisName = "Mouse X";
    [SerializeField] private string VerticalAxisName = "Mouse Y";
    
    private float currentFrameHorizontalAxisValue = 0;
    private float currentFrameVerticalAxisValue = 0;
    private Vector3 verticalRotation = Vector3.zero;
    
    private Transform localCachedTransform;
    
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        localCachedTransform = this.GetComponent<Transform>();
    }

    void Update()
    {
        currentFrameHorizontalAxisValue = Input.GetAxis(HorizontalAxisName) * HorizontalLookSensitivity * Time.deltaTime;
        currentFrameVerticalAxisValue = Input.GetAxis(VerticalAxisName) * VerticalLookSensitivity * Time.deltaTime;

        if (InvertVerticalLook)
            verticalRotation.x += currentFrameVerticalAxisValue;
        else
            verticalRotation.x -= currentFrameVerticalAxisValue;
        
        verticalRotation.x = Mathf.Clamp(verticalRotation.x, BottomMinVerticalLookAngles, TopMaxVerticalLookAngles);
        
        PlayerLookCameraTransform.localRotation = Quaternion.Euler(verticalRotation);
        localCachedTransform.Rotate(Vector3.up * currentFrameHorizontalAxisValue);
    }
}
