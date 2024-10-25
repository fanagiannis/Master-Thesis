using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Guard : Entity
{
    //private  zanimController;
    void Awake()
    {
       // zanimController = GetComponent<Animator>();
    }
    void Update()
    {

        if(Death())
        {
            //zanimController.TriggerDeath();
            //GetComponent<NavMeshAgent>().ResetPath();
           // GetComponent<ZombieBehavior>().Deactivate();
        }
    }
}
