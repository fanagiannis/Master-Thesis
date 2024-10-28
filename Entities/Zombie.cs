using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : Entity
{
    private ZombieAnimationController zanimController;
    void Awake()
    {
        zanimController = GetComponent<ZombieAnimationController>();
    }
    void Update()
    {

        if(Death())
        {
            zanimController.TriggerDeath();
            GetComponent<NavMeshAgent>().ResetPath();
            GetComponent<ZombieBehavior>().Deactivate();
            GetComponent<CapsuleCollider>().enabled = false;
        }
    }
}
