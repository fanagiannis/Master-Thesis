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
    [SerializeField]protected float range; 
    [SerializeField]protected float speed;
    [SerializeField]protected UnityEvent Shoot;

    public override void Start()
    {
        base.Start();
        animator = GetComponent<GuardAnimationController>();
        entity = GetComponent<Guard>();
        BakeBehavior();
    }
    public override void Update()
    {
        if(!GetComponent<Guard>().Death())
        {
            BT.Process();
            targetInRange=TargetInRange(lineOfSight.GetVisibleTarget(),range);
            //DEBUG!!!!!!!!!!!!
            if(InDanger)
            {
                lineOfSight.viewAngle=270f;
            }
            else
            {
                lineOfSight.viewAngle=150f;
            }
            if(targetInRange)
            {
                navigation.ResetPath();
            }
            //DEBUG!!!!!!!!!!!!

            
        }
        else
        {
            navigation.ResetPath();
        }
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
    

