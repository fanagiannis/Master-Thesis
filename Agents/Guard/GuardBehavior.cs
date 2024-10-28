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
            playerInRange=TargetInRange(EnemyMaster.Instance.Target(),10f);
            //DEBUG!!!!!!!!!!!!
            if(!InDanger)
            {
                //navigation.speed = speed;
            }
            if(!EnemyMaster.Instance.PlayerAlive())
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

        // CONDITIONS

        //SPOT
        Condition notspotPlayer = new Condition("PlayerSpotted?", new ConditionLeaf(() => !playerSpotted && !InDanger));
        Condition spotPlayer = new Condition("PlayerSpotted?", new ConditionLeaf(() => playerSpotted));
        Condition spotZombie = new Condition("ZombieSpotted?", new ConditionLeaf(() => lineOfSight.GetVisibleTarget() != null && lineOfSight.GetVisibleTarget().CompareTag("Zombie")));
        
        //CHECK DANGER
        Condition checkIfDanger = new Condition("Threatened?", new ConditionLeaf(() => InDanger));
        Condition safe = new Condition("Safe?", new ConditionLeaf(() => !InDanger));

        //SHOOT CONDITIONS
        Condition inRange = new Condition("CanShootPlayer?", new ConditionLeaf(() => playerInRange));
        Condition cantShoot = new Condition("CantShootPlayer?", new ConditionLeaf(() => !playerInRange && EnemyMaster.Instance.PlayerAlive() && !InDanger));
        Condition canShoot = new Condition("CanShootPlayer?", new ConditionLeaf(() => playerInRange));
        

        // ACTIONS
        Action patrol = new Action("Guard Patrol", new GuardRandomPatrol(this, this.navigation, this.animator));
        Action takeCover = new Action("Take Cover", new GuardGoTo(this.animator, this.navigation, () => safezone.position));
        Action crouchAction = new Action("Crouch", new Crouch(this.animator));
        Action standUp = new Action("Stand", new ActionReset(new Crouch(this.animator)));
        Action setDanger = new Action("Set Danger", new SetDanger(this, true));
        Action chasePlayer = new Action("Chase Player", new GuardGoTo(this.animator, this.navigation, () => EnemyMaster.Instance.Target().position));
        Action lookAt = new Action("LookAtPlayer", new LookAtTarget(this.navigation, this.animator, () => EnemyMaster.Instance.Target()));
        Action aim = new Action("Aim At Player", new Aim(this.animator));
        Action stand = new Action("Stand", new Stand(this.animator));
        Action shootAction = new Action("ShootPlayer", new ShootAction(EnemyMaster.Instance.Target(), animator, Shoot));

        WaitNode delay = new WaitNode("Chase Delay", 1f);
        WaitNode delay1 = new WaitNode("Delay", 3f);

        // TREE STRUCTURE
        Sequence guardPatrol = new Sequence("Patrol");
        guardPatrol.AddChild(notspotPlayer);
        guardPatrol.AddChild(patrol);

        Sequence hideSequence = new Sequence("Take Cover");
        hideSequence.AddChild(checkIfDanger);
        hideSequence.AddChild(takeCover);
        hideSequence.AddChild(crouchAction);

        Fallback CoverFireFB = new Fallback("Cover Fire");
        Sequence coverFireSequence = new Sequence("Cover Fire Sequence");
        coverFireSequence.AddChild(inRange);
        coverFireSequence.AddChild(stand);
        CoverFireFB.AddChild(coverFireSequence);
        hideSequence.AddChild(CoverFireFB);

        Sequence checkcoverSafety = new Sequence("Check Cover Safety");
        checkcoverSafety.AddChild(safe);
        checkcoverSafety.AddChild(standUp);
        hideSequence.AddChild(checkcoverSafety);

        Sequence playerSpot = new Sequence("Spot Player");
        playerSpot.AddChild(spotPlayer);

        Sequence zombieSpot = new Sequence("Spot Zombie");
        zombieSpot.AddChild(spotZombie);
        zombieSpot.AddChild(setDanger);

        Sequence chaseSequence = new Sequence("Chase Player Sequence");
        chaseSequence.AddChild(cantShoot);
        chaseSequence.AddChild(chasePlayer);

        Sequence delayAndShootSequence = new Sequence("Delay and Shoot Sequence");
        delayAndShootSequence.AddChild(canShoot);
        delayAndShootSequence.AddChild(delay1);
        delayAndShootSequence.AddChild(shootAction);

        RepeatNode repeat = new RepeatNode("Repeat Shoot", delayAndShootSequence, () => EnemyMaster.Instance.PlayerAlive());

        Sequence shootSequence = new Sequence("Shoot Player Sequence");
        shootSequence.AddChild(repeat);
        shootSequence.AddChild(delayAndShootSequence);

        Sequence chooseShootPlayerSequence = new Sequence("Choose Shoot Player Sequence");
        chooseShootPlayerSequence.AddChild(canShoot);
        chooseShootPlayerSequence.AddChild(lookAt);
        chooseShootPlayerSequence.AddChild(delay);
        chooseShootPlayerSequence.AddChild(aim);
        chooseShootPlayerSequence.AddChild(shootSequence);

        Fallback killPlayer = new Fallback("Kill Player");
        killPlayer.AddChild(chooseShootPlayerSequence);
        killPlayer.AddChild(chaseSequence);

        playerSpot.AddChild(killPlayer);

        Fallback rootFallback = new Fallback("Root");
        rootFallback.AddChild(hideSequence);

        Fallback roamFB = new Fallback("RoamFB");
        roamFB.AddChild(zombieSpot);
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

    public override bool TargetInRange(Transform target, float range)
    {
        if(EnemyMaster.Instance.Target()!=null)
        {
            return Vector3.Distance(this.transform.position,EnemyMaster.Instance.Target().position)<range && lineOfSight.ActiveVisiblePlayer() && EnemyMaster.Instance.PlayerAlive();
        }
        else
        {
            return false;
        }  
    }
}
    

