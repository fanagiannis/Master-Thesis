using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Behavior;
using Actions;
using Conditions;

public class Zombie : Agent
{
    [SerializeField]protected float walkspeed,runspeed,currentspeed;
    protected Animator animator;
    [SerializeField]protected bool playerSpotted;
    [SerializeField]protected bool playerAlive;
    [SerializeField]protected Transform playerPosition;
    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        BakeBehavior();
        currentspeed=walkspeed;
        this.navigation.speed = currentspeed;
    }
    public override void Update()
    {
        BT.Process();
    }
    public override void BakeBehavior()
    {
        BT=new BehaviorTree("Guard Logic");

        Sequence zombiePatrol = new Sequence("Zombie Patrol");
        Condition notspotPlayer = new Condition("PlayerSpotted?",new ConditionLeaf(()=>!playerSpotted));
        Action patrol = new Action("Roam",new GuardRandomPatrol(this,this.navigation,this.animator));

        Sequence chasePlayerSequence = new Sequence("Spot Sequence");
        Condition spotPlayer = new Condition("PlayerSpotted?",new ConditionLeaf(()=>playerSpotted));
        Action lookAt = new Action("LookAtPlayer",new ZombieLookAtTarget(this.navigation,this.animator,playerPosition));
        WaitNode delay = new WaitNode("Chase Delay",5f);
        Action chasePlayer = new Action("Chase Player",new GoTo(this.animator,this.navigation,()=>playerPosition.position));

        chasePlayerSequence.AddChild(spotPlayer);
        chasePlayerSequence.AddChild(lookAt);
        chasePlayerSequence.AddChild(delay);
        chasePlayerSequence.AddChild(chasePlayer);

        zombiePatrol.AddChild(notspotPlayer);
        zombiePatrol.AddChild(patrol);

        Fallback rootfallback = new Fallback("Root");

        rootfallback.AddChild(zombiePatrol);
        rootfallback.AddChild(chasePlayerSequence);
        BT.AddChild(rootfallback);
        BT.PrintTree();
    }

    public void SetPlayerSpotted()
    {
        playerSpotted = true;
    }

    public void ResetPlayerAlive()
    {
        playerAlive = false;
    }
}
