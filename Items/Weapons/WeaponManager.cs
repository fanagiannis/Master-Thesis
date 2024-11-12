using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    [Header("Weapon Data")]
    [SerializeField]private Weapon weaponData;
    [Header("Prefabs")]
    [SerializeField]private GameObject hitFX;
    [SerializeField]private GameObject enemyHitFX;
    [SerializeField]private GameObject muzzleFlash;
    [SerializeField]private GameObject soundSourcePrefab;
    [Header("Aim Laser")]
    [SerializeField]private GameObject aimLine;
    [Header("Weapon Stats")]
    [SerializeField]private int damage;
    [SerializeField]private int maxAmmo;
    [SerializeField]private int currentAmmo;
    [SerializeField]private int magazineAmmo;
    [SerializeField]private float volume;
    [SerializeField]private float resetdelay=1f;
    [SerializeField]private float delay;
    private void Start()
    {
        damage=weaponData.Damage;
        maxAmmo=weaponData.MaxAmmo;
        magazineAmmo=weaponData.MagazineAmmo;
        currentAmmo=magazineAmmo;
        delay=resetdelay;
    }
    public void Fire(Transform origin)
    {
        muzzleFlash.SetActive(true);
        var sound = Instantiate(soundSourcePrefab, transform.position,transform.rotation);
        sound.GetComponent<SoundSource>().SetSourceVolume(volume);
        RaycastHit hit;
        float pitch = 0f;
        if(weaponData.Silenced)
        {   
            pitch = 3f;
        }
        else
        {
            pitch = Random.Range(0.85f, 1.15f);
        }
        GetComponent<AudioSource>().pitch = pitch;
        GetComponent<AudioSource>().PlayOneShot(weaponData.Sound);
        if (Physics.Raycast(origin.position + new Vector3(0, 1, 0), origin.forward, out hit))
        {
            Debug.Log($"Hit {hit.collider.gameObject.name}");
            if (hit.collider.gameObject.CompareTag("Guard") || hit.collider.gameObject.CompareTag("Zombie"))
            {
                GameObject impact = Instantiate(enemyHitFX, hit.point + new Vector3(0, Random.Range(0.2f, 0.7f), 0), Quaternion.LookRotation(hit.normal), parent: hit.collider.gameObject.transform);
                hit.collider.gameObject.GetComponent<Entity>().TakeDamage(weaponData.Damage);
                Destroy(impact, 0.5f);
            }
            else if (hit.collider.gameObject.CompareTag("Wall"))
            {
                GameObject impact = Instantiate(hitFX, hit.point + new Vector3(0, Random.Range(0.2f, 0.7f), 0), Quaternion.LookRotation(hit.normal, origin.position - hit.point));
                Destroy(impact, 2f);
            }
        }
    }
    public void Aim(bool value)
    {
        aimLine.SetActive(value);
    }
    public void StopShooting()
    {
        muzzleFlash.SetActive(false);
    }
    public Weapon Data()
    {
        return weaponData;
    }
    public void AddAmmo(int value)
    {
        maxAmmo+=value;
    }
    public void DecAmmo(int value)
    {
        currentAmmo -= value;
    }
    public void Reload()
    {
        currentAmmo = magazineAmmo;
        maxAmmo -= magazineAmmo;
    }
    public int Damage{get {return damage;}}
    public int MaxAmmo{get {return maxAmmo;}}
    public int CurrentAmmo{get {return currentAmmo;}}
    public int MagazineAmmo{get {return magazineAmmo;}}
}
