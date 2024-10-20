using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]private Weapon weaponData;
    public void Fire(Transform origin)
    {
        RaycastHit hit;
        Debug.Log("Boom");
        GetComponent<Animator>().SetTrigger("Fire");
        GetComponent<AudioSource>().PlayOneShot(weaponData.Sound);
        if(Physics.Raycast(origin.position,origin.forward,out hit)){
            Debug.Log($"Hit {hit.collider.gameObject.name}");
            if(hit.collider.tag == "Player"){
                hit.collider.GetComponent<Player>().Data.TakeDamage(weaponData.Damage);
                if(hit.collider.gameObject.GetComponent<Player>().Data.Dead())
                {
                    //this.GetComponent<Player>().Stats().GetKill();
                }
            }
        }
    }
    public Weapon Data()
    {
        return weaponData;
    }
}
