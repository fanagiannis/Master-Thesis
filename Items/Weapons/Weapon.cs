using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

[CreateAssetMenu (fileName = "Weapon", menuName = "New ScriptableObject/Weapons/Weapon")]
public class Weapon : ScriptableObject
{
    public enum FireType{Automatic,Single,Burst}
    [SerializeField]private GameObject prefab;
    [SerializeField]private AudioClip sound;
    [SerializeField]private Sprite icon;
    [SerializeField]private FireType type;
    [SerializeField]private string weaponName;
    [SerializeField]private int maxAmmo;
    [SerializeField]private int currentAmmo;
    [SerializeField]private int magazineAmmo;
    [SerializeField]private int damage;
    [SerializeField]private float fireRate;
    [SerializeField]private bool silenced;
    public string Name
    {
        get { return weaponName; }
    }
    public int Damage
    {
        get { return damage; }
    }
    public float FireRate
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
    public bool Silenced
    {
        get { return silenced; }
    }
    public FireType Type
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
