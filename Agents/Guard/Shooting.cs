using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCShooting : MonoBehaviour
{
    public Transform shootingPoint;        
    public Transform player;                
    public float bulletSpeed = 20f;        
    public float shootingRange = 30f;      
    public float shootCooldown = 1.5f;     
    public float aimSpeed = 5f;             
    private float nextTimeToShoot = 0f; 
    private bool activated=false;   

    void Update()
    {
        if(activated)
        {    
            AimAtPlayer();
            if (Time.time >= nextTimeToShoot)
            {
                Shoot();
                nextTimeToShoot = Time.time + shootCooldown;
            }
        }
        
    }
    void AimAtPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;  
        Quaternion lookRotation = Quaternion.LookRotation(direction);           
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * aimSpeed);  
    }

    void Shoot()
    {   
        PlayerDummy target=player.GetComponent<PlayerDummy>();
        this.gameObject.GetComponent<Animator>().SetTrigger("Shoot");
        if(target!=null)
        {
            int random = Random.Range(0, 3);
            Debug.Log(random);
            if(random >0)
            {
                
                target.TakeDamage(10f);
            }  
        }                         
    }

    public void SetActivate(bool value)
    {
        activated=value;
    }
}
