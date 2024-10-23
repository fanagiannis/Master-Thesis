using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Behavior;
using Actions;
using Conditions;
using UnityEngine.Assertions.Must;

public class Zombie : HostileAgent 
{
    [SerializeField]protected float walkspeed,runspeed,currentspeed;
    protected AnimationController animator;
    public override void Start()
    {
        base.Start();
        animator = GetComponent<AnimationController>();
        BakeBehavior();
        currentspeed=walkspeed;
        this.navigation.speed = currentspeed;
    }
    public override void Update()
    {
        BT.Process();
        playerInRange=TargetInRange();
    }
    public override void BakeBehavior()
    {
        BT=new BehaviorTree("Guard Logic");

        Sequence zombiePatrol = new Sequence("Zombie Patrol");
        Condition notspotPlayer = new Condition("PlayerSpotted?",new ConditionLeaf(()=>!PlayerSpotted()));
        Action patrol = new Action("Roam",new GuardRandomPatrol(this,this.navigation,this.animator));

        Sequence chasePlayerSequence = new Sequence("Spot Sequence");
        Condition spotPlayer = new Condition("PlayerSpotted?",new ConditionLeaf(()=>PlayerSpotted() && !playerInRange));
        WaitNode delay = new WaitNode("Chase Delay",1f);
        Action lookAt = new Action("LookAtPlayer",new ZombieLookAtTarget(this.navigation,this.animator,()=>targetPosition));
        WaitNode delay2 = new WaitNode("Chase Delay",5f);
        Action chasePlayer = new Action("Chase Player",new GoTo(this.animator,this.navigation,()=>targetPosition.position));

        Sequence hitPlayer = new Sequence("Hit Player");
        Condition InRange = new Condition("InRange?",new ConditionLeaf(()=>playerInRange && PlayerSpotted()));
        Action hitAction = new Action("Hit",new ZombieHit(this.animator,this.navigation,()=>targetPosition));

        hitPlayer.AddChild(InRange);
        hitPlayer.AddChild(hitAction);

        chasePlayerSequence.AddChild(spotPlayer);
        chasePlayerSequence.AddChild(delay);
        chasePlayerSequence.AddChild(lookAt);
        chasePlayerSequence.AddChild(delay2);
        chasePlayerSequence.AddChild(chasePlayer);

        zombiePatrol.AddChild(notspotPlayer);
        zombiePatrol.AddChild(patrol);

        Fallback rootfallback = new Fallback("Root");

        rootfallback.AddChild(zombiePatrol);
        rootfallback.AddChild(chasePlayerSequence);
        rootfallback.AddChild(hitPlayer);
        BT.AddChild(rootfallback);
        BT.PrintTree();
    }
}
