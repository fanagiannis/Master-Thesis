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
            targetInRange=TargetInRange(lineOfSight.GetVisibleTarget(),10f);
            //DEBUG!!!!!!!!!!!!
            if(InDanger)
            {
                lineOfSight.viewAngle=270f;
            }
            else
            {
                lineOfSight.viewAngle=150f;
            }
            if(targetInRange)
            {
                navigation.ResetPath();
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
        Condition notspotTarget = new Condition("TargetSpotted?", new ConditionLeaf(() => !lineOfSight.GetVisibleTarget()  && !InDanger));
        Condition spotTarget = new Condition("TargetSpotted?", new ConditionLeaf(() => lineOfSight.GetVisibleTarget()  && !InDanger));
        
        //CHECK DANGER
        Condition checkIfDanger = new Condition("Threatened?", new ConditionLeaf(() => InDanger));
        Condition safe = new Condition("Safe?", new ConditionLeaf(() => !InDanger));

        //SHOOT CONDITIONS
        Condition cantShoot = new Condition("CantShootTarget?", new ConditionLeaf(() => !targetInRange && !InDanger));
        Condition canShoot = new Condition("CanShootTarget?", new ConditionLeaf(() => targetInRange));
        

        // ACTIONS
        Action patrol = new Action("Guard Patrol", new GuardRandomPatrol(this, this.navigation, this.animator));
        Action takeCover = new Action("Take Cover", new GuardGoTo(this.animator, this.navigation, () => safezone.position));
        Action crouchAction = new Action("Crouch", new Crouch(this.animator));
        Action standUp = new Action("Stand", new ActionReset(new Crouch(this.animator)));
        Action setDanger = new Action("Set Danger", new SetDanger(this, true));
        Action chaseTarget = new Action("Chase Target", new GuardGoTo(this.animator, this.navigation, () => lineOfSight.GetVisibleTarget().position));
        Action lookAt = new Action("Look At Target", new LookAtTarget(this.navigation, this.animator, () => lineOfSight.GetVisibleTarget()));
        Action aim = new Action("Aim At Target", new Aim(this.animator));
        Action stand = new Action("Stand", new Stand(this.animator));
        Action shootAction = new Action("Shoot Target", new ShootAction( animator, Shoot, ()=>lineOfSight.GetVisibleTarget()));

        WaitNode delay = new WaitNode("Chase Delay", 1f);
        WaitNode shootDelay = new WaitNode("Delay", 3f); //DELAY CONTROL FROM WEAPON FIRERATE

        // TREE STRUCTURE
        Sequence guardPatrol = new Sequence("Patrol Sequence");
        guardPatrol.AddChild(notspotTarget);
        guardPatrol.AddChild(patrol);

        Sequence checkcoverSafety = new Sequence("Check Cover Safety Sequence");
        checkcoverSafety.AddChild(safe);
        checkcoverSafety.AddChild(standUp);

        Sequence chaseSequence = new Sequence("Chase Player Sequence");
        chaseSequence.AddChild(cantShoot);
        chaseSequence.AddChild(chaseTarget);

        Sequence delayAndShootSequence = new Sequence("Delay and Shoot Sequence");
        delayAndShootSequence.AddChild(canShoot);
        delayAndShootSequence.AddChild(shootDelay);
        delayAndShootSequence.AddChild(shootAction);

        RepeatNode repeat = new RepeatNode("Repeat Shoot", delayAndShootSequence, () => EnemyMaster.Instance.PlayerAlive());

        Sequence shootSequence = new Sequence("Shoot Player Sequence");
        shootSequence.AddChild(repeat);
        shootSequence.AddChild(delayAndShootSequence);

        Sequence chooseShootTargetSequence = new Sequence("Choose Shoot Target Sequence");
        chooseShootTargetSequence.AddChild(canShoot);
        chooseShootTargetSequence.AddChild(lookAt);
        chooseShootTargetSequence.AddChild(delay);
        chooseShootTargetSequence.AddChild(aim);
        chooseShootTargetSequence.AddChild(shootSequence);

        Fallback killPlayer = new Fallback("Kill Player Fallback");
        killPlayer.AddChild(chooseShootTargetSequence);
        killPlayer.AddChild(chaseSequence);

        Sequence hideSequence = new Sequence("Take Cover Sequence");
        hideSequence.AddChild(checkIfDanger);
        hideSequence.AddChild(takeCover);
        hideSequence.AddChild(crouchAction);

        Fallback CoverFireFB = new Fallback("Cover Fire Fallback");
        Sequence coverFireSequence = new Sequence("Cover Fire Sequence");
        coverFireSequence.AddChild(canShoot);
        coverFireSequence.AddChild(stand);
        coverFireSequence.AddChild(chooseShootTargetSequence);

        CoverFireFB.AddChild(coverFireSequence);

        hideSequence.AddChild(CoverFireFB);
        hideSequence.AddChild(checkcoverSafety);

        Sequence targetSpotSequence = new Sequence("Spot Target Sequence");
        targetSpotSequence.AddChild(spotTarget);
        targetSpotSequence.AddChild(killPlayer);
        
        Fallback roamFB = new Fallback("Roam Fallback");
        roamFB.AddChild(targetSpotSequence);
        roamFB.AddChild(guardPatrol);
        
        Fallback rootFallback = new Fallback("Root Fallback");
        rootFallback.AddChild(hideSequence);
        rootFallback.AddChild(roamFB);

        BT.AddChild(rootFallback);
        BT.PrintTree();
    }

    public void TargetSpot()
    {
        
        
    }

    public override bool TargetInRange(Transform target, float range)
    {
        if(target!=null)
        {
            return Vector3.Distance(this.transform.position,target.position)<range && lineOfSight.ActiveVisibleTarget();
        }
        else
        {
            return false;
        }  
    }
}
    

