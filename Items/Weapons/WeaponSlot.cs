using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlot : MonoBehaviour
{
    [SerializeField]private WeaponManager equippedweapon;
    private void Start()
    {
        SetWeapon();
    } 
    public void SetWeapon()
    {
        equippedweapon = GetComponentInChildren<WeaponManager>();
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
