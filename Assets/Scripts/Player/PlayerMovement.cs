using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private float HorizontalSpeed = 5.0f;
    [SerializeField] private float VerticalSpeed = 6.0f;
    [SerializeField] private float GravityAcceleration = -15.0f;
    [SerializeField] private float JumpSpeed = 2.0f;
    [SerializeField] private float JumpAttenuationFactor = 0.0027f;
    [SerializeField] private float GroundDistanceCheck = 1.1f;
    [SerializeField] private float CeilingDistanceCheck = 1.1f;
    
    [SerializeField] private string JumpButtonName = "Jump";
    [SerializeField] private string HorizontalAxisName = "Horizontal";
    [SerializeField] private string VerticalAxisName = "Vertical";
#pragma warning restore 0649

    private float currentFrameHorizontalAxisValue = 0;
    private float currentFrameVerticalAxisValue = 0;
    
    private Vector3 movePosition = Vector3.zero;
    private Vector3 verticalMovePosition = Vector3.zero;

    private CharacterController cachedLocalCharacterController;
    private Transform cachedLocalTransform;

    private bool isTouchingGround = true;
    private bool isTouchingCeiling = false;
    private bool pressedJumpButton = false;

    private void Awake()
    {
        cachedLocalCharacterController = this.GetComponent<CharacterController>();
        cachedLocalTransform = this.GetComponent<Transform>();
    }

    private void Update()
    {
        CheckIfGrounded();
        CheckIfTouchingCeiling();
        CheckVerticalVelocity();
        
        ProcessFloorMovement();
        cachedLocalCharacterController.Move(movePosition);
        
        ProcessJump();
        ProcessGravity();
        cachedLocalCharacterController.Move(verticalMovePosition);
    }

    private void ProcessFloorMovement()
    {
        currentFrameHorizontalAxisValue = Input.GetAxis(HorizontalAxisName) * Time.deltaTime * HorizontalSpeed;
        currentFrameVerticalAxisValue = Input.GetAxis(VerticalAxisName) * Time.deltaTime * VerticalSpeed;

        movePosition = (cachedLocalTransform.right * currentFrameHorizontalAxisValue) + (cachedLocalTransform.forward * currentFrameVerticalAxisValue);
    }
    
    private void ProcessJump()
    {
        pressedJumpButton = Input.GetButtonDown(JumpButtonName);

        if (pressedJumpButton && isTouchingGround && !isTouchingCeiling)
            verticalMovePosition.y = -1.0f * JumpAttenuationFactor * JumpSpeed * GravityAcceleration;
    }
    
    private void ProcessGravity()
    {
        if(!isTouchingGround)
            verticalMovePosition.y += GravityAcceleration * Time.deltaTime * Time.deltaTime;
    }

    private void CheckIfGrounded()
    {
        isTouchingGround =  Physics.Raycast(cachedLocalTransform.position, Vector3.down, GroundDistanceCheck);
    }
    
    private void CheckIfTouchingCeiling()
    {
        isTouchingCeiling = Physics.Raycast(cachedLocalTransform.position, Vector3.up, CeilingDistanceCheck);
    }
    
    private void CheckVerticalVelocity()
    {
        if (isTouchingGround)
        {
            movePosition.y = 0;
            if(verticalMovePosition.y < 0)
            {
                verticalMovePosition.y = 0;
            }
        }
    }
}
