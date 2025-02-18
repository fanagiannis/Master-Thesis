using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Behavior;
using Actions;
using Actions.GuardActions;
using Conditions;
using UnityEngine.Events;
using System.Linq.Expressions;

public class GuardAlarmBehavior : GuardBehavior
{
    [Header("Patrol Variables")]
    [SerializeField]private List<Transform> patrolPoints;
    [SerializeField]private Transform alarm;
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
            //DEBUG!!!!!!!!!!!
            if(targetInRange)
            {
               navigation.ResetPath();
            }
            //DEBUG!!!!!!!!!!!!

            
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

        //CHECK SECURITY 
        Condition alarmnotSet = new Condition("Condition Alarm Not Set?",new ConditionLeaf(()=>SecurityManager.Instance.CurrentSecurityState()!=SecurityManager.SecurityState.Alert));
        Condition alarmSet = new Condition("Condition Alarm Set?",new ConditionLeaf(()=>SecurityManager.Instance.CurrentSecurityState()==SecurityManager.SecurityState.Alert));
        
        //CHECK DANGER
        Condition checkIfDanger = new Condition("Condition Threatened?", new ConditionLeaf(() => InDanger));
        Condition safe = new Condition("Condition Safe?", new ConditionLeaf(() => !InDanger));

        //SHOOT
        Condition cantShoot = new Condition("Condition CantShootTarget?", new ConditionLeaf(() => !targetInRange && !InDanger));
        Condition canShoot = new Condition("Condition CanShootTarget?", new ConditionLeaf(() => targetInRange));
        

        // ACTIONS
        Action randompatrol = new Action("Action Guard Random Patrol", new GuardRandomPatrol(this, this.navigation, this.animator));
        Action patrol = new Action("Action Guard Patrol", new GuardPatrol(this, this.navigation, this.animator,patrolPoints));
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
        Action stop = new Action("Action Stop",new Stop(this,this.navigation,this.animator));
        Action setAlarm = new Action("Action Set Alarm",new GuardSetAlarm(this.animator, this.navigation, () => alarm.position));

        WaitNode delay = new WaitNode("Delay Chase", 1f);
        WaitNode delay2 = new WaitNode("Delay Chase", 2f);
        WaitNode delay3 = new WaitNode("Delay Chase", 3f);
        WaitNode delay4 = new WaitNode("Delay Chase", 4f);
        WaitNode shootDelay = new WaitNode("Delay", 3f); 

        // TREE STRUCTURE
        Sequence guardPatrolSequence = new Sequence("Sequence Patrol");
        guardPatrolSequence.AddChild(notspotTarget);
        guardPatrolSequence.AddChild(delay);
        guardPatrolSequence.AddChild(patrol);

        Sequence checkcoverSafetySequence = new Sequence("Sequence Check Cover Safety");
        checkcoverSafetySequence.AddChild(safe);
        checkcoverSafetySequence.AddChild(standUp);

        Sequence chaseSequence = new Sequence("Sequence Chase Target");
        chaseSequence.AddChild(cantShoot);
        chaseSequence.AddChild(chaseTarget);

        Sequence delayAndShootSequence = new Sequence("Sequence Delay and Shoot");
        delayAndShootSequence.AddChild(canShoot);
        delayAndShootSequence.AddChild(shootDelay);
        delayAndShootSequence.AddChild(shootAction);

        RepeatNode repeat = new RepeatNode("Repeat Shoot", delayAndShootSequence, () => SecurityManager.Instance.GetEnemyManager().PlayerAlive() || sensors.GetVisibleTarget() );

        Sequence shootSequence = new Sequence("Sequence Shoot Target");
        shootSequence.AddChild(repeat);
        shootSequence.AddChild(delayAndShootSequence);

        Sequence chooseShootTargetSequence = new Sequence("Sequence Choose Shoot Target");
        chooseShootTargetSequence.AddChild(canShoot);
        chooseShootTargetSequence.AddChild(lookAt);
        chooseShootTargetSequence.AddChild(delay);
        chooseShootTargetSequence.AddChild(aim);
        chooseShootTargetSequence.AddChild(shootSequence);

        Sequence setAlarmSequence = new Sequence("Sequence Set Alarm");
        setAlarmSequence.AddChild(alarmnotSet);
        setAlarmSequence.AddChild(setAlarm);

        Fallback killPlayerFB = new Fallback("Fallback Kill Player");
        killPlayerFB.AddChild(setAlarmSequence);
        killPlayerFB.AddChild(chooseShootTargetSequence);
        killPlayerFB.AddChild(chaseSequence);
        
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
        hideSequence.AddChild(checkcoverSafetySequence);

        Sequence targetSpotSequence = new Sequence("Sequence Spot Target");
        targetSpotSequence.AddChild(spotTarget);
        targetSpotSequence.AddChild(killPlayerFB);

        Sequence InvestigateSequence = new Sequence("Sequence Investigate");
        InvestigateSequence.AddChild(lookAtSound);
        InvestigateSequence.AddChild(aim);
        InvestigateSequence.AddChild(delay2);
        InvestigateSequence.AddChild(inspectSource);
        InvestigateSequence.AddChild(delay);

        Sequence SoundAlertSequence = new Sequence("Sequence Sound Alert");
        SoundAlertSequence.AddChild(soundHeard);
        SoundAlertSequence.AddChild(InvestigateSequence);

        
        Fallback roamFB = new Fallback("Fallback Roam");
        roamFB.AddChild(targetSpotSequence);
        roamFB.AddChild(SoundAlertSequence);
        roamFB.AddChild(guardPatrolSequence);
        
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
