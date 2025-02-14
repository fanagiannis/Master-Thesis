using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Behavior;
using Actions;
using Actions.GuardActions;
using Conditions;
using UnityEngine.Events;
using System.Linq.Expressions;

public class GuardNoBrain : GuardBehavior
{
    [Header("Patrol Variables")]
    [SerializeField]private List<Transform> patrolPoints;
    [SerializeField]private Transform alarm;
    [SerializeField]private Transform safezone;
    
    public override void Start()
    {
        base.Start();
        BakeBehavior();
        //TestBehavior();
    }
    public override void Update()
    {
        if(!GetComponent<Guard>().Death())
        {
            BT.Process();
            targetInRange=TargetInRange(sensors.GetVisibleTarget(),10f);
            //DEBUG!!!!!!!!!!!
            // if(targetInRange)
            // {
            //     navigation.ResetPath();
            // }
            //DEBUG!!!!!!!!!!!!

            
        }
    }
    public override void BakeBehavior()
    {

        BT = new BehaviorTree("Basic Guard Logic");
        
    }

    public override bool TargetInRange(Transform target, float range)
    {
        if(target!=null)
        {
            return Vector3.Distance(this.transform.position,target.position)<range&&!Physics.Raycast(transform.position, (target.position - transform.position).normalized, Vector3.Distance(this.transform.position,target.position), sensors.obstacleMask);
        }
        else
        {
            return false;
        }  
    }
}
