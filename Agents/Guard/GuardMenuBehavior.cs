using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Behavior;
using Actions;
using Actions.GuardActions;
using Conditions;
using UnityEngine.Events;
using System.Linq.Expressions;

public class GuardPatrolMenuBehavior : GuardBehavior
{
    [Header("Patrol Variables")]
    [SerializeField]private List<Transform> patrolPoints;
    [SerializeField]private Transform alarm;
    [SerializeField]private Transform safezone;
    
    public override void Start()
    {
        base.Start();
        BakeBehavior();
    }
    public override void Update()
    {
        if(!GetComponent<Guard>().Death())
        {
            BT.Process();
            targetInRange=TargetInRange(sensors.GetVisibleTarget(),10f);
        }
    }
    public override void BakeBehavior()
    {
        BT = new BehaviorTree("Basic Guard Logic");

        // CONDITIONS
        
        // // ACTIONS
        Action patrol = new Action("Action Guard Patrol", new GuardPatrol(this, this.navigation, this.animator,patrolPoints));

        //DECORATORS
        WaitNode delay = new WaitNode("Delay", 1f);
    
        // TREE STRUCTURE
        Sequence guardPatrolSequence = new Sequence("Sequence Patrol");
        guardPatrolSequence.AddChild(delay);
        guardPatrolSequence.AddChild(patrol);
        
        Fallback roamFB = new Fallback("Fallback Roam");
        roamFB.AddChild(guardPatrolSequence);
        
        Fallback rootFallback = new Fallback("Fallback Root");
        rootFallback.AddChild(roamFB);

        BT.AddChild(rootFallback);
        BT.PrintTree();
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
