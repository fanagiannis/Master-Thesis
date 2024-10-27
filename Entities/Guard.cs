using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Guard : Entity
{
    private GuardAnimationController animController;
    void Awake()
    {
       animController = GetComponent<GuardAnimationController>();
    }
    void Update()
    {

        if(Death())
        {
            animController.TriggerDeath();
            GetComponent<CapsuleCollider>().enabled = false;
            //GetComponent<NavMeshAgent>().ResetPath();
           // GetComponent<ZombieBehavior>().Deactivate();
        }
    }
}
