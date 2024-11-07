using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

[CreateAssetMenu (fileName = "Weapon", menuName = "New ScriptableObject/Weapons/Weapon")]
public class Weapon : ScriptableObject
{
    public enum WeaponType{Melee,Pistol,Rifle,AutomaticRifle,SMG}
    [SerializeField]private GameObject prefab;
    [SerializeField]private AudioClip sound;
    [SerializeField]private Sprite icon;
    [SerializeField]private WeaponType type;
    [SerializeField]private string weaponName;
    [SerializeField]private int maxAmmo;
    [SerializeField]private int currentAmmo;
    [SerializeField]private int magazineAmmo;
    [SerializeField]private int damage;
    [SerializeField]private int fireRate;
    public void DecAmmo(int value)
    {
        currentAmmo -= value;
    }
    public void Reload()
    {
        currentAmmo = magazineAmmo;
        maxAmmo -= magazineAmmo;
    }
    public string Name
    {
        get { return weaponName; }
    }
    public int Damage
    {
        get { return damage; }
    }
    public int FireRate
    {
        get { return fireRate; }
    }
    public int MagazineAmmo
    {
        get { return magazineAmmo;}
    }
    public int MaxAmmo
    {
        get { return maxAmmo;}
    }
    public int CurrentAmmo
    {
        get { return currentAmmo;}
    }
    public WeaponType Type
    {
        get { return type; }
    }
    public AudioClip Sound
    {
        get { return sound; }
    }
    public GameObject Prefab
    {
        get { return prefab; }
    }
}
