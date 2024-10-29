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
            if(InDanger)
            {
                lineOfSight.viewAngle=270f;
            }
            else
            {
                lineOfSight.viewAngle=150f;
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
        // // CONDITIONS

        // //SPOT
        // Condition notspotTarget = new Condition("TargetSpotted?", new ConditionLeaf(() => !lineOfSight.ActiveVisibleTarget() && !InDanger));
        // Condition spotTarget = new Condition("TargetSpotted?", new ConditionLeaf(() => lineOfSight.ActiveVisibleTarget() ));
        // Condition spotPlayer = new Condition("PlayerSpotted?", new ConditionLeaf(() => playerSpotted));
        // Condition spotZombie = new Condition("ZombieSpotted?", new ConditionLeaf(() => lineOfSight.GetVisibleTarget() != null && lineOfSight.GetVisibleTarget().CompareTag("Zombie")));
        
        // //CHECK DANGER
        // Condition checkIfDanger = new Condition("Threatened?", new ConditionLeaf(() => InDanger));
        // Condition safe = new Condition("Safe?", new ConditionLeaf(() => !InDanger));

        // //SHOOT CONDITIONS
        // Condition inRange = new Condition("CanShootPlayer?", new ConditionLeaf(() => playerInRange));
        // Condition cantShoot = new Condition("CantShootPlayer?", new ConditionLeaf(() => !playerInRange && EnemyMaster.Instance.PlayerAlive() && !InDanger));
        // Condition canShoot = new Condition("CanShootPlayer?", new ConditionLeaf(() => TargetInRange(lineOfSight.GetVisibleTarget(),10f)));
        

        // // ACTIONS
        // Action patrol = new Action("Guard Patrol", new GuardRandomPatrol(this, this.navigation, this.animator));
        // Action takeCover = new Action("Take Cover", new GuardGoTo(this.animator, this.navigation, () => safezone.position));
        // Action crouchAction = new Action("Crouch", new Crouch(this.animator));
        // Action standUp = new Action("Stand", new ActionReset(new Crouch(this.animator)));
        // Action setDanger = new Action("Set Danger", new SetDanger(this, true));
        // Action chaseTarget = new Action("Chase Target", new GuardGoTo(this.animator, this.navigation, () => lineOfSight.GetVisibleTarget().position));
        // Action lookAtTarget = new Action("LookAtTarget", new LookAtTarget(this.navigation, this.animator, () => lineOfSight.GetVisibleTarget()));
        // Action aim = new Action("Aim At Target", new Aim(this.animator));
        // Action stand = new Action("Stand", new Stand(this.animator));
        // Action shootAction = new Action("ShootTarget", new ShootAction(animator, Shoot, () => lineOfSight.GetVisibleTarget()));

        // WaitNode delay = new WaitNode("Chase Delay", 1f);
        // WaitNode shootDelay = new WaitNode("Delay", 3f); //DELAY CONTROL FROM WEAPON FIRERATE

        // // TREE STRUCTURE

        // //GUARD PATROL
        // Sequence guardPatrol = new Sequence("Patrol Sequence");
        // guardPatrol.AddChild(notspotTarget);
        // guardPatrol.AddChild(patrol);

        // //SPOT TARGET
        
        

        // Sequence ShootTargetSequence = new Sequence("Shoot Target Sequence");
        // ShootTargetSequence.AddChild(canShoot);
        // ShootTargetSequence.AddChild(shootDelay);
        // ShootTargetSequence.AddChild(shootAction);

        // RepeatNode repeat = new RepeatNode("Repeat Shoot", ShootTargetSequence, () => EnemyMaster.Instance.PlayerAlive());

        // Sequence ShootTargetRepeatSequence = new Sequence("Shoot Target Repeat Sequence");
        // ShootTargetRepeatSequence.AddChild(repeat);
        // ShootTargetRepeatSequence.AddChild(ShootTargetSequence);

        // Sequence AimTargetSequence = new Sequence("Aim Target Sequence");
        // AimTargetSequence.AddChild(lookAtTarget);
        // AimTargetSequence.AddChild(delay);
        // AimTargetSequence.AddChild(aim);
        // AimTargetSequence.AddChild(ShootTargetRepeatSequence);

       
        // Sequence chaseTargetSequence = new Sequence("Chase Target Sequence");
        // chaseTargetSequence.AddChild(cantShoot);
        // chaseTargetSequence.AddChild(chaseTarget);

        // Sequence killTargetSequence = new Sequence("Kill Target Sequence");
        // killTargetSequence.AddChild(AimTargetSequence);
        // killTargetSequence.AddChild(chaseTargetSequence);

        // Sequence SpotTargetSequence = new Sequence("Spot Target Sequence");
        // SpotTargetSequence.AddChild(spotTarget);
        // SpotTargetSequence.AddChild(killTargetSequence);


        // Fallback RoamFB = new Fallback("Roam Fallback");
        // RoamFB.AddChild(SpotTargetSequence); 
        // RoamFB.AddChild(guardPatrol);
        
        // Fallback rootFallback = new Fallback("Root Fallback");
        // rootFallback.AddChild(RoamFB);

        // BT.AddChild(rootFallback);
        // BT.PrintTree();

        // CONDITIONS

        //SPOT
        Condition notspotTarget = new Condition("TargetSpotted?", new ConditionLeaf(() => !lineOfSight.GetVisibleTarget()  && !InDanger));
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
        Action shootAction = new Action("ShootPlayer", new ShootAction( animator, Shoot, ()=>EnemyMaster.Instance.Target()));

        WaitNode delay = new WaitNode("Chase Delay", 1f);
        WaitNode shootDelay = new WaitNode("Delay", 3f); //DELAY CONTROL FROM WEAPON FIRERATE

        // TREE STRUCTURE
        Sequence guardPatrol = new Sequence("Patrol");
        guardPatrol.AddChild(notspotTarget);
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

        //zombieSpot.AddChild(setDanger);

        Sequence chaseSequence = new Sequence("Chase Player Sequence");
        chaseSequence.AddChild(cantShoot);
        chaseSequence.AddChild(chasePlayer);

        Sequence delayAndShootSequence = new Sequence("Delay and Shoot Sequence");
        delayAndShootSequence.AddChild(canShoot);
        delayAndShootSequence.AddChild(shootDelay);
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
        
        Fallback roamFB = new Fallback("RoamFB");
        roamFB.AddChild(playerSpot);
        roamFB.AddChild(zombieSpot);
        roamFB.AddChild(guardPatrol);
        
        Fallback rootFallback = new Fallback("Root");
        rootFallback.AddChild(hideSequence);
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
            return Vector3.Distance(this.transform.position,target.position)<range && lineOfSight.ActiveVisibleTarget();
        }
        else
        {
            return false;
        }  
    }
}
    

