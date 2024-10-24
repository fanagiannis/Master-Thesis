using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]private Weapon weaponData;
    [SerializeField]private GameObject hitFX,enemyHitFX;
    public void Fire(Transform origin)
    {
        RaycastHit hit;
        GetComponent<AudioSource>().PlayOneShot(weaponData.Sound);
        if(Physics.Raycast(origin.position+new Vector3(0,1,0),origin.forward,out hit)){
            Debug.Log($"Hit {hit.collider.gameObject.name}");
            if(hit.collider.gameObject.CompareTag("Enemy"))
            {
                GameObject impact = Instantiate(enemyHitFX, hit.point+new Vector3(0,Random.Range(0.2f,0.7f),0), Quaternion.LookRotation(-hit.normal));
                Destroy(impact, 1f);
            }

            else if(hit.collider.gameObject.CompareTag("Wall"))
            {
                GameObject impact = Instantiate(hitFX, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impact, 2f);
            }
            
            
            
        }
    }
    public Weapon Data()
    {
        return weaponData;
    }
}
