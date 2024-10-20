using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Destroy : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        Debug.Log($"{name} destroyed");
        gameObject.SetActive(false);
        collider.GetComponent<NavMeshAgent>().ResetPath();
    }
}
