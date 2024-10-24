using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]private Weapon weaponData;
    [SerializeField]private GameObject hitFX;
    public void Fire(Transform origin)
    {
        RaycastHit hit;
        GetComponent<AudioSource>().PlayOneShot(weaponData.Sound);
        Debug.DrawRay(origin.position, origin.forward, Color.red);
        if(Physics.Raycast(origin.position,origin.forward,out hit)){
            Debug.Log($"Hit {hit.collider.gameObject.name}");
            GameObject impact = Instantiate(hitFX, hit.point+new Vector3(0,1.5f,0), Quaternion.LookRotation(hit.normal));
            Destroy(impact, 2f);
            
        }
    }
    public Weapon Data()
    {
        return weaponData;
    }
}
