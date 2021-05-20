using UnityEngine;

[RequireComponent(typeof(PlayerAnimatorController))]
public class PlayerShoot : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private Camera LookCamera;
    [SerializeField] private float CurrentWeaponRange = 100.0f;
    [SerializeField] private float CurrentWeaponFireRate = 1.0f;
    [SerializeField] private int CurrentWeaponDamage = 10;
    [SerializeField] private LayerMask ShootableLayerMasks;
    
    [SerializeField] private string FirePrimaryButtonName = "Fire1";
    [SerializeField] private string AimButtonName = "Fire2";
#pragma warning restore 0649

    private PlayerAnimatorController cachedAnimatorController;
    private Transform cachedCameraTransform;
    private float weaponFireRate = 0;
    
    private bool isFiring = false;
    private bool isAiming = false;

    private void Awake()
    {
        cachedAnimatorController = this.GetComponent<PlayerAnimatorController>();
        cachedCameraTransform = LookCamera.GetComponent<Transform>();
    }

    private void Update()
    {
        isAiming = Input.GetButton(AimButtonName);
        isFiring = Input.GetButton(FirePrimaryButtonName);
        
        cachedAnimatorController.SetAimAnimation(isAiming);
        cachedAnimatorController.SetFireAnimation(isFiring);
        
        ProcessShoot();
    }

    private void ProcessShoot()
    {
        weaponFireRate -= Time.deltaTime;
        
        if (weaponFireRate < 0 && isFiring)
        {
            weaponFireRate = CurrentWeaponFireRate;
            
            if(Physics.Raycast(cachedCameraTransform.position, cachedCameraTransform.forward, out RaycastHit hit, CurrentWeaponRange, ShootableLayerMasks))
            {
                Vida otherObjectVida = hit.transform.GetComponent<Vida>();
                otherObjectVida?.Dañar(CurrentWeaponDamage);
            }
        }
    }
}
