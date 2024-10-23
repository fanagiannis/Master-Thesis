using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Behavior;
using Actions;
using Conditions;
using UnityEngine.Assertions.Must;

public class Zombie : Agent
{
    [SerializeField]protected float walkspeed,runspeed,currentspeed;
    protected AnimationController animator;
    [SerializeField]protected bool playerSpotted,playerInRange;
    [SerializeField]protected bool playerAlive;
    [SerializeField]protected Transform playerPosition;
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
        Condition notspotPlayer = new Condition("PlayerSpotted?",new ConditionLeaf(()=>!playerSpotted));
        Action patrol = new Action("Roam",new GuardRandomPatrol(this,this.navigation,this.animator));

        Sequence chasePlayerSequence = new Sequence("Spot Sequence");
        Condition spotPlayer = new Condition("PlayerSpotted?",new ConditionLeaf(()=>playerSpotted && !playerInRange));
        Action lookAt = new Action("LookAtPlayer",new ZombieLookAtTarget(this.navigation,this.animator,playerPosition));
        WaitNode delay = new WaitNode("Chase Delay",5f);
        Action chasePlayer = new Action("Chase Player",new GoTo(this.animator,this.navigation,()=>playerPosition.position));

        Sequence hitPlayer = new Sequence("Hit Player");
        Condition InRange = new Condition("InRange?",new ConditionLeaf(()=>playerInRange && playerSpotted));
        Action hitAction = new Action("Hit",new ZombieHit(playerPosition,this.animator,this.navigation));

        hitPlayer.AddChild(InRange);
        hitPlayer.AddChild(hitAction);

        chasePlayerSequence.AddChild(spotPlayer);
        chasePlayerSequence.AddChild(lookAt);
        chasePlayerSequence.AddChild(delay);
        chasePlayerSequence.AddChild(chasePlayer);
        //chasePlayerSequence.AddChild(hitPlayer);

        zombiePatrol.AddChild(notspotPlayer);
        zombiePatrol.AddChild(patrol);

        Fallback rootfallback = new Fallback("Root");

        rootfallback.AddChild(zombiePatrol);
        rootfallback.AddChild(chasePlayerSequence);
        rootfallback.AddChild(hitPlayer);
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
        playerSpotted = false;
    }
    public bool TargetInRange()
    {
        float range = 2f; 
        return Vector3.Distance(transform.position, playerPosition.position) <= range;
    }
}
