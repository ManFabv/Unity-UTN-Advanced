using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private float HorizontalSpeed = 5.0f;
    [SerializeField] private float VerticalSpeed = 6.0f;
    
    [SerializeField] private string HorizontalAxisName = "Horizontal";
    [SerializeField] private string VerticalAxisName = "Vertical";
#pragma warning restore 0649

    private float currentFrameHorizontalAxisValue = 0;
    private float currentFrameVerticalAxisValue = 0;
    
    private Vector3 movePosition = Vector3.zero;

    private CharacterController cachedLocalCharacterController;
    private Transform cachedLocalTransform;

    private void Awake()
    {
        cachedLocalCharacterController = this.GetComponent<CharacterController>();
        cachedLocalTransform = this.GetComponent<Transform>();
    }

    private void Update()
    {
        currentFrameHorizontalAxisValue = Input.GetAxis(HorizontalAxisName) * Time.deltaTime * HorizontalSpeed;
        currentFrameVerticalAxisValue = Input.GetAxis(VerticalAxisName) * Time.deltaTime * VerticalSpeed;

        movePosition = (cachedLocalTransform.right * currentFrameHorizontalAxisValue) + (cachedLocalTransform.forward * currentFrameVerticalAxisValue);

        cachedLocalCharacterController.Move(movePosition);
    }
}
