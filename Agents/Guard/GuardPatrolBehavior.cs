using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Behavior;
using Actions;
using Actions.GuardActions;
using Conditions;
using UnityEngine.Events;
using System.Linq.Expressions;

public class GuardPatrolBehavior : GuardBehavior
{
    [Header("Patrol Variables")]
    [SerializeField]private Transform safezone;
    
    public override void Start()
    {
        base.Start();
        BakeBehavior();
        //TestBehavior();
    }
    public override void Update()
    {
        if(!GetComponent<Guard>().Death())
        {
            BT.Process();
            targetInRange=TargetInRange(sensors.GetVisibleTarget(),10f);
            //DEBUG!!!!!!!!!!!!
            // if(InDanger)
            // {
            //     sensors.viewAngle=270f;
            // }
            // else
            // {
            //     sensors.viewAngle=150f;
            // }
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

        BT = new BehaviorTree("Basic Guard Logic");

        // CONDITIONS

        //SPOT
        Condition notspotTarget = new Condition("Condition Not Target Spotted?", new ConditionLeaf(() => !sensors.GetVisibleTarget()  && !InDanger));
        Condition spotTarget = new Condition("Condition Target Spotted?", new ConditionLeaf(() => sensors.GetVisibleTarget()   && !InDanger));
        Condition soundHeard = new Condition("Condition Heard Sound",new ConditionLeaf(()=>sensors.Heard()));
        
        //CHECK DANGER
        Condition checkIfDanger = new Condition("Condition Threatened?", new ConditionLeaf(() => InDanger));
        Condition safe = new Condition("Condition Safe?", new ConditionLeaf(() => !InDanger));

        //SHOOT
        Condition cantShoot = new Condition("Condition CantShootTarget?", new ConditionLeaf(() => !targetInRange && !InDanger));
        Condition canShoot = new Condition("Condition CanShootTarget?", new ConditionLeaf(() => targetInRange));
        

        // ACTIONS
        Action patrol = new Action("Action Guard Patrol", new GuardRandomPatrol(this, this.navigation, this.animator));
        Action takeCover = new Action("Action Take Cover", new GuardGoTo(this.animator, this.navigation, () => safezone.position));
        Action crouchAction = new Action("Action Crouch", new Crouch(this.animator));
        Action standUp = new Action("Action Stand", new ActionReset(new Crouch(this.animator)));
        Action setDanger = new Action("Action Set Danger", new SetDanger(this, true));
        Action chaseTarget = new Action("Action Chase Target", new GuardGoTo(this.animator, this.navigation, () => sensors.GetVisibleTarget() .position));
        Action lookAt = new Action("Action Look At Target", new LookAtTarget(this.navigation, this.animator, () => sensors.GetVisibleTarget() ));
        Action lookAtSound = new Action("Action Look At Sound Source", new LookAtTarget(this.navigation, this.animator, () => sensors.FindSoundSource() ));
        Action inspectSource = new Action("Action Go To Sound Source", new Inspect(this.animator, this.navigation, this.sensors,() => sensors.InspectingSource.position));
        Action aim = new Action("Action Aim At Target", new Aim(this.animator));
        Action stand = new Action("Action Stand", new Stand(this.animator));
        Action shootAction = new Action("Action Shoot Target", new ShootAction( animator, Shoot , entity.Damage() , ()=>sensors.GetVisibleTarget() ));

        WaitNode delay = new WaitNode("Delay Chase", 1f);
        WaitNode delay2 = new WaitNode("Delay Chase", 2f);
        WaitNode delay3 = new WaitNode("Delay Chase", 3f);
        WaitNode delay4 = new WaitNode("Delay Chase", 4f);
        WaitNode shootDelay = new WaitNode("Delay", 3f); //DELAY CONTROL FROM WEAPON FIRERATE

        // TREE STRUCTURE
        Sequence guardPatrol = new Sequence("Sequence Patrol");
        guardPatrol.AddChild(notspotTarget);
        guardPatrol.AddChild(patrol);

        Sequence checkcoverSafety = new Sequence("Sequence Check Cover Safety");
        checkcoverSafety.AddChild(safe);
        checkcoverSafety.AddChild(standUp);

        Sequence chaseSequence = new Sequence("Sequence Chase Target");

        chaseSequence.AddChild(cantShoot);
        chaseSequence.AddChild(chaseTarget);

        Sequence delayAndShootSequence = new Sequence("Sequence Delay and Shoot");
        delayAndShootSequence.AddChild(canShoot);
        delayAndShootSequence.AddChild(shootDelay);
        delayAndShootSequence.AddChild(shootAction);

        RepeatNode repeat = new RepeatNode("Repeat Shoot", delayAndShootSequence, () => EnemyMaster.Instance.PlayerAlive() || sensors.GetVisibleTarget() );

        Sequence shootSequence = new Sequence("Sequence Shoot Target");
        shootSequence.AddChild(repeat);
        shootSequence.AddChild(delayAndShootSequence);

        Sequence chooseShootTargetSequence = new Sequence("Sequence Choose Shoot Target");
        chooseShootTargetSequence.AddChild(canShoot);
        chooseShootTargetSequence.AddChild(lookAt);
        chooseShootTargetSequence.AddChild(delay);
        chooseShootTargetSequence.AddChild(aim);
        chooseShootTargetSequence.AddChild(shootSequence);

        Fallback killPlayer = new Fallback("Fallback Kill Player");
        killPlayer.AddChild(chooseShootTargetSequence);
        killPlayer.AddChild(chaseSequence);

        Sequence hideSequence = new Sequence("Sequence Take Cover");
        hideSequence.AddChild(checkIfDanger);
        hideSequence.AddChild(takeCover);
        hideSequence.AddChild(crouchAction);

        Fallback CoverFireFB = new Fallback("Fallback Cover Fire");
        Sequence coverFireSequence = new Sequence("Sequence Cover Fire");
        coverFireSequence.AddChild(canShoot);
        coverFireSequence.AddChild(stand);
        coverFireSequence.AddChild(chooseShootTargetSequence);

        CoverFireFB.AddChild(coverFireSequence);

        hideSequence.AddChild(CoverFireFB);
        hideSequence.AddChild(checkcoverSafety);

        Sequence targetSpotSequence = new Sequence("Sequence Spot Target");
        targetSpotSequence.AddChild(spotTarget);
        targetSpotSequence.AddChild(killPlayer);

        Sequence InvestigateSequence = new Sequence("Sequence Investigate");
        InvestigateSequence.AddChild(lookAtSound);
        InvestigateSequence.AddChild(delay3);
        InvestigateSequence.AddChild(aim);
        InvestigateSequence.AddChild(delay2);
        InvestigateSequence.AddChild(inspectSource);
        InvestigateSequence.AddChild(delay4);

        Sequence SoundAlertSequence = new Sequence("Sequence Sound Alert");
        SoundAlertSequence.AddChild(soundHeard);
        SoundAlertSequence.AddChild(InvestigateSequence);

        
        Fallback roamFB = new Fallback("Fallback Roam");
        roamFB.AddChild(targetSpotSequence);
        roamFB.AddChild(SoundAlertSequence);
        roamFB.AddChild(guardPatrol);
        
        Fallback rootFallback = new Fallback("Fallback Root");
        rootFallback.AddChild(hideSequence);
        rootFallback.AddChild(roamFB);

        BT.AddChild(rootFallback);
        BT.PrintTree();
    }

    public override bool TargetInRange(Transform target, float range)
    {
        if(target!=null)
        {
            return Vector3.Distance(this.transform.position,target.position)<range&&!Physics.Raycast(transform.position, (target.position - transform.position).normalized, Vector3.Distance(this.transform.position,target.position), sensors.obstacleMask);
        }
        else
        {
            return false;
        }  
    }
}
