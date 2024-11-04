using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player" )
        {
            other.gameObject.GetComponent<Entity>().TakeDamage(150);
            //GetComponent<SoundSource>().Activate();
        }
    }   
}
