using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

[CreateAssetMenu (fileName = "Weapon", menuName = "New ScriptableObject/Weapons/Weapon")]
public class Weapon : ScriptableObject
{
    [SerializeField]private GameObject prefab;
    [SerializeField]private AudioClip sound;
    [SerializeField]private Sprite icon;
    [SerializeField]private int MaxAmmo;
    [SerializeField]private int MagazineAmmo;
    [SerializeField]private string weaponName;
    [SerializeField]private int damage;
    [SerializeField]private int fireRate;
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
    public AudioClip Sound
    {
        get { return sound; }
    }
    public GameObject Prefab
    {
        get { return prefab; }
    }
}
