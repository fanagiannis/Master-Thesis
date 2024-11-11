using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField]private WeaponSlot weaponSlot;
    [SerializeField]private PlayerAnimationController animController;
    [SerializeField]private UnityEvent Shoot;
    [SerializeField]private UnityEvent StopShooting; 
    [SerializeField] private float fireRate = 0.1f; 
    private PlayerInput playerInput;
    [SerializeField]private float nextFireTime;
    [SerializeField]private bool isAiming=false;
    private bool hasFired=false;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        animController = GetComponent<PlayerAnimationController>();
    }

    public void Control()
    {
        if (playerInput.actions["Fire"].ReadValue<float>() > 0  && weaponSlot.CanFire())
        {  
            FireController();   
        }
        else if (playerInput.actions["Fire"].ReadValue<float>() <= 0 || !weaponSlot.CanFire())
        {     
            StopShooting.Invoke();
            isAiming=false;
            hasFired=false;
        }
        if (playerInput.actions["Reload"].ReadValue<float>()>0 && weaponSlot.CanReload())
        {
            weaponSlot.Reload();
        }
        if(playerInput.actions["Aim"].ReadValue<float>() > 0)
        {
            isAiming=true;
            animController.Aim();
        }   
        else if(playerInput.actions["Aim"].ReadValue<float>() <= 0)
        {
            isAiming=false;
            animController.ResetAim();
        }
        AimController();
    }
    public void FireController()
    {
        switch (weaponSlot.EquippedWeapon().Data().Type)
        {
            case Weapon.FireType.Automatic:
                FireAutomatic();
                break;
            case Weapon.FireType.Single:
                FireSingle();
                break;
        }   
    }
    public void AimController()
    {
        weaponSlot.EquippedWeapon().Aim(isAiming);
    }
    private void FireAutomatic()
    {
        if (Time.time >= nextFireTime && isAiming)
        {
            Shoot.Invoke();  
            nextFireTime = Time.time + weaponSlot.EquippedWeapon().Data().FireRate;  
        }
    }

    private void FireSingle()
    {
        if (Time.time >= nextFireTime && isAiming )
        {
            if(!hasFired)
            {
                Shoot.Invoke();  
                nextFireTime = Time.time + weaponSlot.EquippedWeapon().Data().FireRate;  
                hasFired=true;
                StartCoroutine(StopShootingCR());
            }    
        }
    }

    private IEnumerator StopShootingCR()
    {
        yield return new WaitForSeconds(0.2f);
        StopShooting.Invoke();
    }
    public bool Aiming()
    {
        return isAiming;
    }
    public void debug()
    {
        Debug.Log("Shot");
    }
}
