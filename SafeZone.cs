using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SafeZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        
        if(other.tag=="Agent")
        {
            var obj=other.GetComponent<Rat>();
            if(obj!=null)
            {
                StartCoroutine(Timer(obj));
                
            }
        }
    }

    private IEnumerator Timer(Rat obj)
    {
        yield return new WaitForSeconds(5f);   
        obj.ResetBT();
        obj.ResetDanger();
        yield break;
        
    }
}
