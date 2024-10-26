using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Behavior;
using Actions;
using Actions.GuardActions;
using Conditions;
using System.ComponentModel;
using UnityEngine.UIElements;
using UnityEngine.Events;

public class GuardBehavior : HostileAgent
{   
    [SerializeField]private Transform safezone;
    [SerializeField]protected float speed;
    protected GuardAnimationController animator;
    [SerializeField]protected bool playerSpotted;
    [SerializeField]protected UnityEvent Shoot;
    public override void Start()
    {
        base.Start();
        animator = GetComponent<GuardAnimationController>();
        
        this.navigation.speed = speed;
        BakeBehavior();
    }
    public override void Update()
    {
        BT.Process();
        playerInRange=Vector3.Distance(this.transform.position,targetPosition.position)<15f && lineOfSight.ActiveVisiblePlayer();
        //DEBUG!!!!!!!!!!!!
        if(!InDanger)
        {
            navigation.speed = speed;
        }
        if(!playerAlive)
        {
            playerSpotted=false;
        }
        //DEBUG!!!!!!!!!!!!
    }
    public override void BakeBehavior()
    {
        BT = new BehaviorTree("Guard Logic");

        Sequence guardPatrol = new Sequence("Patrol");
        Condition notspotPlayer = new Condition("PlayerSpotted?", new ConditionLeaf(() => !playerSpotted && !InDanger));
        Action patrol = new Action("Guard Patrol", new GuardRandomPatrol(this, this.navigation, this.animator));

        guardPatrol.AddChild(notspotPlayer);
        guardPatrol.AddChild(patrol);

        Sequence hideSequence = new Sequence("Take Cover");
        Condition checkIfDanger = new Condition("Threatened?", new ConditionLeaf(() => InDanger));
        Action takeCover = new Action("Take Cover", new GuardGoTo(this.animator, this.navigation, () => safezone.position));
        Action crouchAction = new Action("Crouch", new Crouch(this.animator));
        Condition safe = new Condition("Safe?", new ConditionLeaf(() => !InDanger));
        Action standUp = new Action("Stand", new ActionReset(new Crouch(this.animator)));

        Sequence playerSpot = new Sequence("Spot Player");
        Condition spotPlayer = new Condition("PlayerSpotted?", new ConditionLeaf(() => PlayerSpotted()));
        Action lookAt = new Action("LookAtPlayer", new LookAtTarget(this.navigation, this.animator, () => targetPosition));
        WaitNode delay = new WaitNode("Chase Delay", 1f);
        Action aim = new Action("Aim At Player", new Aim(this.animator));

        Sequence shootSequence = new Sequence("Shoot Player Sequence");
        Condition canShoot = new Condition("CanShootPlayer?", new ConditionLeaf(() => playerInRange));
        Action shootAction = new Action("ShootPlayer", new ShootAction(targetPosition, animator, Shoot));
        WaitNode delay1 = new WaitNode("Delay", 3f);

        Sequence chaseSequence = new Sequence("Chase Player Sequence");
        Condition cantShoot = new Condition("CantShootPlayer?", new ConditionLeaf(() => !playerInRange));
        Action chasePlayer = new Action("Chase Player",new GoTo(this.animator,this.navigation,()=>targetPosition.position));

        chaseSequence.AddChild(cantShoot);
        chaseSequence.AddChild(chasePlayer);

        Sequence delayAndShootSequence = new Sequence("Delay and Debug Sequence");
        delayAndShootSequence.AddChild(delay1);
        delayAndShootSequence.AddChild(shootAction);

        RepeatNode repeat = new RepeatNode("Repeat Shoot", delayAndShootSequence, () => playerAlive);
        shootSequence.AddChild(canShoot);
        shootSequence.AddChild(repeat);
        shootSequence.AddChild(delayAndShootSequence);

        playerSpot.AddChild(spotPlayer);
        playerSpot.AddChild(lookAt);
        playerSpot.AddChild(delay);
        playerSpot.AddChild(aim);

        Fallback KillPlayer = new Fallback("Kill Player");
        KillPlayer.AddChild(chaseSequence);
        KillPlayer.AddChild(shootSequence);

        playerSpot.AddChild(KillPlayer);
        
        hideSequence.AddChild(checkIfDanger);
        hideSequence.AddChild(takeCover);
        hideSequence.AddChild(crouchAction);
        hideSequence.AddChild(safe);
        hideSequence.AddChild(standUp);

        Fallback rootFallback = new Fallback("Root");
        rootFallback.AddChild(hideSequence);
        rootFallback.AddChild(playerSpot);
        rootFallback.AddChild(guardPatrol);

        BT.AddChild(rootFallback);
        BT.PrintTree();
    }

    public void SetPlayerSpotted()
    {
        playerSpotted = true;
    }
}
    

