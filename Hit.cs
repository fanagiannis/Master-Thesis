using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            other.GetComponent<Player>().TakeDamage(10);
        }
        if(other.tag=="Guard")
        {
            other.GetComponent<Guard>().TakeDamage(10);
        }
    }
}
