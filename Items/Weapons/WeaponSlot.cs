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
        SetWeapon(currentWeaponIndex);
    } 
    private void Update()
    {
        WeaponChange();
    }
    public void WeaponChange()
    {
        if(input.actions["WeaponChange"].ReadValue<Vector2>()!=Vector2.zero)
        {
            WeaponIndexControl();
            WeaponIndexFlow();
            WeaponListManipulation();
            SetWeapon(currentWeaponIndex);
        }
    }
    public void SetWeapon(int index)
    {
        
        if(index>0 && index < weaponsList.Count-1)
        {
            weaponsList[index].gameObject.SetActive(true);
        }
        equippedweapon = GetComponentInChildren<WeaponManager>();
    }
    public void WeaponIndexFlow()
    {
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
        
    }
    public void WeaponIndexControl()
    {
        if(input.actions["WeaponChange"].ReadValue<Vector2>().y>0)
        {
            currentWeaponIndex+=1;
        }
        else if(input.actions["WeaponChange"].ReadValue<Vector2>().y<0)
        {
            currentWeaponIndex-=1;
        }
    }
    public void WeaponListManipulation()
    {
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
    }
    public WeaponManager EquippedWeapon()
    {
        return equippedweapon;
    }
    public void FireEquippedWeapon(Transform origin)
    {
        equippedweapon.Fire(origin);
        equippedweapon.DecAmmo(1);
    }
    public bool CanFire()
    {
        return equippedweapon.CurrentAmmo>0;
    }
    public bool CanReload()
    {
        return equippedweapon.CurrentAmmo<equippedweapon.MagazineAmmo && equippedweapon.MaxAmmo>0;
    }
    public void Reload()
    {
        equippedweapon.Reload();
    }
}
