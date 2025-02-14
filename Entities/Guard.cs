using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Guard : Entity
{
    private GuardAnimationController animController;
    [SerializeField]private float damage;
    void Awake()
    {
       animController = GetComponent<GuardAnimationController>();
    }
    void Update()
    {

        if(Death())
        {
            animController.TriggerDeath();
            DisableComponents(GetComponents<Component>());
            DisableComponents(GetComponentsInChildren<Component>());
        }
    }
    public float Damage()
    {
        return damage;
    }
    private void DisableComponents(Component[] components)
    {
        foreach (var component in components)
        {
            if (component is MeshRenderer) continue; 
            if (component is Animator) animController.enabled = false;

            if (component is MonoBehaviour monoBehaviour)
            {
                monoBehaviour.enabled = false;
            }
            else if (component is Collider collider)
            {
                collider.enabled = false;
            }
            else if (component is NavMeshAgent navMeshAgent)
            {
                navMeshAgent.enabled = false;
            }
            else if (component is Light light)
            {
                light.enabled = false;
            }
        }
    }
}

