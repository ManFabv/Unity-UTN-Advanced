using UnityEngine;

[RequireComponent(typeof(PlayerAnimatorController))]
public class PlayerShoot : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private string FirePrimaryButtonName = "Fire1";
    [SerializeField] private string AimButtonName = "Fire2";
#pragma warning restore 0649

    private PlayerAnimatorController cachedAnimatorController;
    
    private bool isFiring = false;
    private bool isAiming = false;

    private void Awake()
    {
        cachedAnimatorController = this.GetComponent<PlayerAnimatorController>();
    }

    private void Update()
    {
        isAiming = Input.GetButton(AimButtonName);
        isFiring = Input.GetButton(FirePrimaryButtonName);
        
        cachedAnimatorController.SetAimAnimation(isAiming);
        cachedAnimatorController.SetFireAnimation(isFiring);
    }
}
