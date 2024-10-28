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
        Condition notspotPlayer = new Condition("PlayerSpotted?",new ConditionLeaf(()=>!PlayerSpotted() || !GetVisibleGuard()));
        Action patrol = new Action("Roam",new ZombieRandomPatrol(this,this.navigation,this.animator));

        Sequence chaseTargetSequence = new Sequence("Spot Target Sequence");
        Condition spotGuard = new Condition("GuardSpotted?",new ConditionLeaf(()=>GetVisibleGuard() && !targetInRange));
        WaitNode delayt = new WaitNode("Chase Delay",1f);
        Action lookAtTarget = new Action("LookAtPlayer",new LookAtTarget(this.navigation,this.animator,()=>lineOfSight.GetVisibleTarget()));
        Action chaseTarget = new Action("Chase Player",new GoTo(this.animator,this.navigation,()=>lineOfSight.GetVisibleTarget().position));

        chaseTargetSequence.AddChild(spotGuard);
        chaseTargetSequence.AddChild(delayt);
        chaseTargetSequence.AddChild(lookAtTarget);
        chaseTargetSequence.AddChild(chaseTarget);

        Sequence hitTarget = new Sequence("Hit Target");
        Condition targetInRangeCond = new Condition("InRange?",new ConditionLeaf(()=>targetInRange && GetVisibleGuard()));
        Action hitTargetAction = new Action("Hit",new ZombieHit(this.animator,this.navigation,()=>lineOfSight.GetVisibleTarget()));

        hitTarget.AddChild(targetInRangeCond);
        hitTarget.AddChild(hitTargetAction);

        Sequence chasePlayerSequence = new Sequence("Spot Sequence");
        Condition spotPlayer = new Condition("PlayerSpotted?",new ConditionLeaf(()=>PlayerSpotted() && !playerInRange));
        WaitNode delay = new WaitNode("Chase Delay",1f);
        Action lookAt = new Action("LookAtPlayer",new LookAtTarget(this.navigation,this.animator,()=>EnemyMaster.Instance.Target()));
        Action chasePlayer = new Action("Chase Player",new GoTo(this.animator,this.navigation,()=>EnemyMaster.Instance.Target().position));

        Sequence hitPlayer = new Sequence("Hit Player");
        Condition InRange = new Condition("InRange?",new ConditionLeaf(()=>playerInRange && PlayerSpotted()));
        Action hitAction = new Action("Hit",new ZombieHit(this.animator,this.navigation,()=>EnemyMaster.Instance.Target()));

        hitPlayer.AddChild(InRange);
        hitPlayer.AddChild(hitAction);

        chasePlayerSequence.AddChild(spotPlayer);
        chasePlayerSequence.AddChild(delay);
        chasePlayerSequence.AddChild(lookAt);
        //chasePlayerSequence.AddChild(delay2);
        chasePlayerSequence.AddChild(chasePlayer);
        //chasePlayerSequence.AddChild(hitPlayer);

        zombiePatrol.AddChild(notspotPlayer);
        zombiePatrol.AddChild(patrol);

        Fallback rootfallback = new Fallback("Root");

        rootfallback.AddChild(zombiePatrol);
        rootfallback.AddChild(chaseTargetSequence);
        rootfallback.AddChild(hitTarget);
        rootfallback.AddChild(chasePlayerSequence);
        rootfallback.AddChild(hitPlayer);
        BT.AddChild(rootfallback);
        BT.PrintTree();
    }
    public bool GetVisibleGuard()
    {
        var target = lineOfSight.GetVisibleTarget();
        return target != null && target.CompareTag("Guard");
    }
    public void Deactivate()
    {
        Active=false;
    }
    
}
