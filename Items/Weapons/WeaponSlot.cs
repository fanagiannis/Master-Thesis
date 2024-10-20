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
    public void FireEquippedWeapon()
    {
        equippedweapon.Fire(GetComponentInParent<Camera>().transform);
    }
}
