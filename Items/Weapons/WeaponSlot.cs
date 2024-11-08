using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSlot : MonoBehaviour
{
    [SerializeField]private PlayerInput input;
    [SerializeField]private WeaponManager equippedweapon;
    [SerializeField]private List<WeaponManager> weaponsList;
    [SerializeField]private int currentWeaponIndex = 0;
    private void Start()
    {
        var weapons = GetComponentsInChildren<WeaponManager>();
        foreach (var weapon in weapons)
        {
            if(weapon.gameObject.tag == "Weapon")
            {
                weaponsList.Add(weapon);
                weapon.gameObject.SetActive(false);
            }     
            if (weaponsList.Count > 0)
            {
                weaponsList[currentWeaponIndex].gameObject.SetActive(true); 
            }
        }
        SetWeapon();
    } 
    private void Update()
    {
        if(input.actions["WeaponChange"].ReadValue<Vector2>()!=Vector2.zero)
        {
            if(input.actions["WeaponChange"].ReadValue<Vector2>().y>0)
            {
                currentWeaponIndex+=1;
            }
            else if(input.actions["WeaponChange"].ReadValue<Vector2>().y<0)
            {
                currentWeaponIndex-=1;
            }
            if(currentWeaponIndex<0)
            {
                currentWeaponIndex=weaponsList.Count-1;
            }
            else if (currentWeaponIndex>weaponsList.Count-1)
            {
                currentWeaponIndex=0;
            }
            if (weaponsList.Count > 0)
            {
                weaponsList[currentWeaponIndex].gameObject.SetActive(true); 
            }
            foreach (var weapon in weaponsList)
        {
            if(weapon.gameObject.tag == "Weapon")
            {
                weapon.gameObject.SetActive(false);
            }     
            if (weaponsList.Count > 0)
            {
                weaponsList[currentWeaponIndex].gameObject.SetActive(true); 
            }
        }
            SetWeapon();
            Debug.Log(input.actions["WeaponChange"].ReadValue<Vector2>());
        }
        
    }
    public void SetWeapon()
    {
        equippedweapon = GetComponentInChildren<WeaponManager>();
    }
    public WeaponManager EquippedWeapon()
    {
        return equippedweapon;
    }
    public void FireEquippedWeapon(Transform origin)
    {
        equippedweapon.Fire(origin);
        equippedweapon.Data().DecAmmo(1);
    }
    public bool CanFire()
    {
        return equippedweapon.Data().CurrentAmmo>0;
    }
    public bool CanReload()
    {
        return equippedweapon.Data().CurrentAmmo<equippedweapon.Data().MagazineAmmo && equippedweapon.Data().MaxAmmo>0;
    }
    public void Reload()
    {
        equippedweapon.Data().Reload();
    }
}
