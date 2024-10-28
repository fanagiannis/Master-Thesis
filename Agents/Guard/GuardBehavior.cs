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
    protected GuardAnimationController animator;
    [SerializeField]private Transform safezone;
    [SerializeField]protected float speed;
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
        if(!GetComponent<Guard>().Death())
        {
            BT.Process();
            playerInRange=TargetInRange(10f);
            //DEBUG!!!!!!!!!!!!
            if(!InDanger)
            {
                //navigation.speed = speed;
            }
            if(!playerAlive)
            {
                playerSpotted=false;
                animator.ResetAlert();
            }
            if(playerInRange)
            {
                navigation.ResetPath();
            }
            if(PlayerSpotted())
            {
                playerSpotted=true;
            }
            //DEBUG!!!!!!!!!!!!
        }
        else
        {
            navigation.ResetPath();
        }
        
        
    }
    public override void BakeBehavior()
    {
        BT = new BehaviorTree("Guard Logic");

        //PATROL

        Sequence guardPatrol = new Sequence("Patrol");
        Condition notspotPlayer = new Condition("PlayerSpotted?", new ConditionLeaf(() => !playerSpotted && !InDanger));
        Action patrol = new Action("Guard Patrol", new GuardRandomPatrol(this, this.navigation, this.animator));
        guardPatrol.AddChild(notspotPlayer);
        guardPatrol.AddChild(patrol);

        //HIDE

        Sequence hideSequence = new Sequence("Take Cover");
        Condition checkIfDanger = new Condition("Threatened?", new ConditionLeaf(() => InDanger));
        Action takeCover = new Action("Take Cover", new GuardGoTo(this.animator, this.navigation, () => safezone.position));
        Action crouchAction = new Action("Crouch", new Crouch(this.animator));
        Condition safe = new Condition("Safe?", new ConditionLeaf(() => !InDanger));
        Action standUp = new Action("Stand", new ActionReset(new Crouch(this.animator)));
        hideSequence.AddChild(checkIfDanger);
        hideSequence.AddChild(takeCover);
        hideSequence.AddChild(crouchAction);

        Fallback CoverFireFB = new Fallback("Cover Fire");
        Sequence coverFireSequence = new Sequence("Cover Fire Sequence");
        Condition inRange = new Condition("CanShootPlayer?", new ConditionLeaf(() => playerInRange));
        Action stand = new Action("Stand",new Stand(this.animator));

        coverFireSequence.AddChild(inRange);
        coverFireSequence.AddChild(stand);

        CoverFireFB.AddChild(coverFireSequence);

        hideSequence.AddChild(CoverFireFB);

        Sequence checkcoverSafety = new Sequence("Check Cover Safety");
        checkcoverSafety.AddChild(safe);
        checkcoverSafety.AddChild(standUp);

        hideSequence.AddChild(checkcoverSafety);

        //SPOT PLAYER

        Sequence playerSpot = new Sequence("Spot Player");
        Condition spotPlayer = new Condition("PlayerSpotted?", new ConditionLeaf(() => playerSpotted));
        playerSpot.AddChild(spotPlayer);

        //CHASE PLAYER

        Sequence chaseSequence = new Sequence("Chase Player Sequence");
        Condition cantShoot = new Condition("CantShootPlayer?", new ConditionLeaf(() => !playerInRange && playerAlive && !InDanger));
        Action chasePlayer = new Action("Chase Player", new GuardGoTo(this.animator, this.navigation, () => targetPosition.position));
        chaseSequence.AddChild(cantShoot);
        chaseSequence.AddChild(chasePlayer);

        //SHOOT PLAYER

        Sequence delayAndShootSequence = new Sequence("Delay and Debug Sequence");
        Condition canShoot = new Condition("CanShootPlayer?", new ConditionLeaf(() => playerInRange));
        WaitNode delay1 = new WaitNode("Delay", 3f);
        Action shootAction = new Action("ShootPlayer", new ShootAction(targetPosition, animator, Shoot));
        delayAndShootSequence.AddChild(canShoot);
        delayAndShootSequence.AddChild(delay1);
        delayAndShootSequence.AddChild(shootAction);

        RepeatNode repeat = new RepeatNode("Repeat Shoot", delayAndShootSequence, () => playerAlive);

        Sequence shootSequence = new Sequence("Shoot Player Sequence");
        shootSequence.AddChild(repeat);
        shootSequence.AddChild(delayAndShootSequence);

        Sequence chooseShootPlayerSequence = new Sequence("Choose Shoot Player Sequence");
        Action lookAt = new Action("LookAtPlayer", new LookAtTarget(this.navigation, this.animator, () => targetPosition));
        WaitNode delay = new WaitNode("Chase Delay", 1f);
        Action aim = new Action("Aim At Player", new Aim(this.animator));
        chooseShootPlayerSequence.AddChild(canShoot);
        chooseShootPlayerSequence.AddChild(lookAt);
        chooseShootPlayerSequence.AddChild(delay);
        chooseShootPlayerSequence.AddChild(aim);
        chooseShootPlayerSequence.AddChild(shootSequence);

        Fallback killPlayer = new Fallback("Kill Player");
        //killPlayer.AddChild(hideSequence);
        killPlayer.AddChild(chooseShootPlayerSequence);
        killPlayer.AddChild(chaseSequence);

        playerSpot.AddChild(killPlayer);

        Fallback rootFallback = new Fallback("Root");
        rootFallback.AddChild(hideSequence);

        Fallback roamFB = new Fallback("RoamFB");

        roamFB.AddChild(playerSpot);
        roamFB.AddChild(guardPatrol);

        rootFallback.AddChild(roamFB);

        BT.AddChild(rootFallback);
        BT.PrintTree();
    }

    public void SetPlayerSpotted()
    {
        playerSpotted = true;
    }

    public override bool TargetInRange(float range)
    {
        if(targetPosition!=null)
        {
            return Vector3.Distance(this.transform.position,targetPosition.position)<range && lineOfSight.ActiveVisiblePlayer() && playerAlive;
        }
        else
        {
            return false;
        }  
    }
}
    

