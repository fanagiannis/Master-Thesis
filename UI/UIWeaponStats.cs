using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIWeaponStats : MonoBehaviour
{
    [SerializeField]private WeaponSlot weaponSlot;
    private TextMeshProUGUI WeaponStats;
    void Awake()
    {
        WeaponStats=this.GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        WeaponStats.text=$"{weaponSlot.EquippedWeapon().Data().Name}\n{weaponSlot.EquippedWeapon().CurrentAmmo} / {weaponSlot.EquippedWeapon().MaxAmmo}";
    }
}
