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
        GetComponent<AudioSource>().pitch = Random.Range(0.85f, 1.15f);
        GetComponent<AudioSource>().PlayOneShot(weaponData.Sound);
        if(Physics.Raycast(origin.position+new Vector3(0,1,0),origin.forward,out hit)){
            Debug.Log($"Hit {hit.collider.gameObject.name}");
            if(hit.collider.gameObject.CompareTag("Guard")||hit.collider.gameObject.CompareTag("Zombie"))
            {
                GameObject impact = Instantiate(enemyHitFX, hit.point+new Vector3(0,Random.Range(0.2f,0.7f),Random.Range(0.2f,0.7f)), Quaternion.LookRotation(-hit.normal),parent:hit.collider.gameObject.transform);
                hit.collider.gameObject.GetComponent<Entity>().TakeDamage(10);
                Destroy(impact, 0.5f);
            }

            else if(hit.collider.gameObject.CompareTag("Wall"))
            {
                GameObject impact = Instantiate(hitFX, hit.point+new Vector3(0,Random.Range(0.2f,0.7f),Random.Range(0.2f,0.7f)), Quaternion.LookRotation(hit.normal));
                Destroy(impact, 2f);
            } 
        }
    }
    public Weapon Data()
    {
        return weaponData;
    }
}
