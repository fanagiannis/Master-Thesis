using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField]private WeaponSlot weaponSlot;
    [SerializeField]private UnityEvent Shoot;
    [SerializeField]private UnityEvent StopShooting; 
    [SerializeField] private float fireRate = 0.1f; 
    private PlayerInput playerInput;
    [SerializeField]private float nextFireTime;
    [SerializeField]private bool isAiming=false;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    public void Control()
    {
        if (playerInput.actions["Fire"].ReadValue<float>() > 0  && weaponSlot.CanFire())
        {  
            FireController();   
        }
        else if (playerInput.actions["Fire"].ReadValue<float>() <= 0)
        {
            
            StopShooting.Invoke();
            isAiming=false;
        }
        if (playerInput.actions["Reload"].ReadValue<float>()>0 && weaponSlot.CanReload())
        {
            weaponSlot.Reload();
        }
        if(playerInput.actions["Aim"].ReadValue<float>() > 0)
        {
            isAiming=true;
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
    
    public void debug()
    {
        Debug.Log("Shot");
    }
}
