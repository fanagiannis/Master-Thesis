using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Behavior;
using Actions;
using Actions.ZombieActions;
using Conditions;
using UnityEngine.Assertions.Must;

public class ZombieBehavior : HostileAgent 
{
    [Header("Movement Speed")]
    [SerializeField]protected float walkspeed;
    [SerializeField]protected float runspeed;
    [SerializeField]protected float currentspeed;
    protected ZombieAnimationController animator;
    public override void Start()
    {
        base.Start();
        animator = GetComponent<ZombieAnimationController>();
        BakeBehavior();
        currentspeed=walkspeed;
        this.navigation.speed = currentspeed;
        animator.Idle();
    }
    public override void Update()
    {
        if(Active)
        {
            BT.Process();
            playerInRange=TargetInRange(EnemyMaster.Instance.Target(),1f);
            targetInRange=TargetInRange(lineOfSight.GetVisibleTarget(),1f);
        }
    }
    public override void BakeBehavior()
    {
        BT=new BehaviorTree("Zombie Logic");

        Sequence zombiePatrol = new Sequence("Zombie Patrol");
        Condition notspotPlayer = new Condition("PlayerSpotted?",new ConditionLeaf(()=>!GetVisibleTarget()));
        Action patrol = new Action("Roam",new ZombieRandomPatrol(this,this.navigation,this.animator));

        Sequence chaseTargetSequence = new Sequence("Spot Target Sequence");
        Condition spotGuard = new Condition("GuardSpotted?",new ConditionLeaf(()=>GetVisibleTarget() && !targetInRange));
        WaitNode delayt = new WaitNode("Chase Delay",1f);
        Action lookAtTarget = new Action("LookAtPlayer",new LookAtTarget(this.navigation,this.animator,()=>lineOfSight.GetVisibleTarget()));
        Action chaseTarget = new Action("Chase Player",new GoTo(this.animator,this.navigation,()=>lineOfSight.GetVisibleTarget().position));

        chaseTargetSequence.AddChild(spotGuard);
        chaseTargetSequence.AddChild(delayt);
        chaseTargetSequence.AddChild(lookAtTarget);
        chaseTargetSequence.AddChild(chaseTarget);

        Sequence hitTarget = new Sequence("Hit Target");
        Condition targetInRangeCond = new Condition("InRange?",new ConditionLeaf(()=>targetInRange && GetVisibleTarget()));
        Action hitTargetAction = new Action("Hit",new ZombieHit(this.animator,this.navigation,()=>lineOfSight.GetVisibleTarget()));

        hitTarget.AddChild(targetInRangeCond);
        hitTarget.AddChild(hitTargetAction);
        
        zombiePatrol.AddChild(notspotPlayer);
        zombiePatrol.AddChild(patrol);

        Fallback rootfallback = new Fallback("Root");

        rootfallback.AddChild(zombiePatrol);
        rootfallback.AddChild(chaseTargetSequence);
        rootfallback.AddChild(hitTarget);
    
        BT.AddChild(rootfallback);
        BT.PrintTree();
    }
    public bool GetVisibleTarget()
    {
        var target = lineOfSight.GetVisibleTarget();
        return target != null && (target.CompareTag("Guard")||(target.CompareTag("Player")&&PlayerSpotted()));
    }
    public void Deactivate()
    {
        Active=false;
    }
    
}
