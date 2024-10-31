using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Behavior;
using Actions;
using Actions.GuardActions;
using Conditions;
using UnityEngine.Events;

public class GuardBehavior : HostileAgent
{   
    protected GuardAnimationController animator;
    [SerializeField]protected float speed;
    [SerializeField]protected UnityEvent Shoot;
    

    public override void Start()
    {
        base.Start();
        animator = GetComponent<GuardAnimationController>();
        this.navigation.speed = speed;
    }

    public override bool TargetInRange(Transform target, float range)
    {
        if(target!=null)
        {
            return Vector3.Distance(this.transform.position,target.position)<range&&!Physics.Raycast(transform.position, (target.position - transform.position).normalized, Vector3.Distance(this.transform.position,target.position), lineOfSight.obstacleMask);
        }
        else
        {
            return false;
        }  
    }
}
    

