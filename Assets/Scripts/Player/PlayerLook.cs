using UnityEngine;

public class PlayerLook : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private Transform PlayerLookCameraTransform;
    [SerializeField] private float HorizontalLookSensitivity = 150.0f;
    [SerializeField] private float VerticalLookSensitivity = 120.0f;
    [SerializeField] private float TopMaxVerticalLookAngles = 90.0f;
    [SerializeField] private float BottomMinVerticalLookAngles = -80.0f;
    [SerializeField] private bool InvertVerticalLook = false;
    
    [SerializeField] private string HorizontalAxisName = "Mouse X";
    [SerializeField] private string VerticalAxisName = "Mouse Y";
#pragma warning restore 0649

    private float currentFrameHorizontalAxisValue = 0;
    private float currentFrameVerticalAxisValue = 0;
    private Vector3 verticalRotation = Vector3.zero;
    
    private Transform localCachedTransform;
    
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        localCachedTransform = this.GetComponent<Transform>();
    }

    private void Update()
    {
        currentFrameHorizontalAxisValue = Input.GetAxis(HorizontalAxisName) * HorizontalLookSensitivity * Time.deltaTime;
        currentFrameVerticalAxisValue = Input.GetAxis(VerticalAxisName) * VerticalLookSensitivity * Time.deltaTime;

        AdjustVerticalAxisValueIfInvertedAxis();
        
        verticalRotation.x -= currentFrameVerticalAxisValue;
        verticalRotation.x = Mathf.Clamp(verticalRotation.x, BottomMinVerticalLookAngles, TopMaxVerticalLookAngles);
        
        PlayerLookCameraTransform.localRotation = Quaternion.Euler(verticalRotation);
        localCachedTransform.Rotate(Vector3.up * currentFrameHorizontalAxisValue);
    }

    private void AdjustVerticalAxisValueIfInvertedAxis()
    {
        if (InvertVerticalLook) currentFrameVerticalAxisValue *= -1;
    }
}
