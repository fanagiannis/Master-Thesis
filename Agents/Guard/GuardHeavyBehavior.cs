using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Behavior;
using Conditions;
using Actions;
using Actions.GuardActions;

public class GuardHeavyBehavior : GuardBehavior
{
    [Header("Defend Zone")]
    [SerializeField]private Transform defendZone;
    public override void Start()
    {
        base.Start();
    }
    public override void Update()
    {
        base.Update();
    }
    public override void BakeBehavior()
    {
        BT = new BehaviorTree("Heavy Guard Logic");

        //CONDITIONS
        Condition spotTarget = new Condition("Condition Target Spotted?", new ConditionLeaf(() => sensors.GetVisibleTarget()   && !InDanger));
        Condition notspotTarget = new Condition("Condition Target Spotted?", new ConditionLeaf(() => !sensors.GetVisibleTarget()   && !InDanger));
        Condition canShoot = new Condition("Condition CanShootTarget?", new ConditionLeaf(() => targetInRange));


        //ACTIONS
        Action lookAt = new Action("Action Look At Target", new LookAtTarget(this.navigation, this.animator, () => sensors.GetVisibleTarget() ));
        Action defendingPatrol = new Action("Action Defending Patrol",new GuardPatrolAroundPoint(this,this.navigation,this.animator,defendZone));
        Action gotoDefendPosition = new Action("Action Take Cover", new GuardGoTo(this.animator, this.navigation, () => defendZone.position));
        Action crouch = new Action("Action Crouch", new Crouch(this.animator));
        Action stand = new Action("Action Stand", new Stand(this.animator));
        Action aim = new Action("Action Aim At Target", new Aim(this.animator));
        Action shoot = new Action("Action Shoot Target", new ShootAction( animator, Shoot , entity.Damage() , ()=>sensors.GetVisibleTarget() ));

        //DELAY
        WaitNode delay = new WaitNode("Delay Chase", 1f);

        Sequence delayAndShootSequence = new Sequence("Sequence Delay and Shoot");
        delayAndShootSequence.AddChild(canShoot);
        delayAndShootSequence.AddChild(delay);
        delayAndShootSequence.AddChild(shoot);

        Sequence shootTargetSequence = new Sequence("Sequence Shoot Target");
        RepeatNode repeat = new RepeatNode("Repeat Shoot", delayAndShootSequence, () => SecurityManager.Instance.GetEnemyManager().PlayerAlive() || sensors.GetVisibleTarget() );
        shootTargetSequence.AddChild(repeat);
        shootTargetSequence.AddChild(delayAndShootSequence);

        Sequence aimAtTargetSequence = new Sequence("Sequence Aim At Target");
        aimAtTargetSequence.AddChild(canShoot);
        aimAtTargetSequence.AddChild(lookAt);
        aimAtTargetSequence.AddChild(delay);
        aimAtTargetSequence.AddChild(aim);
        aimAtTargetSequence.AddChild(shootTargetSequence);

        Sequence coverFireSequence = new Sequence("Sequence Cover Fire");
        coverFireSequence.AddChild(canShoot);
        coverFireSequence.AddChild(stand);
        coverFireSequence.AddChild(aimAtTargetSequence);

        Fallback coverFireFallback = new Fallback("Fallback Cover Fire");
        coverFireFallback.AddChild(coverFireSequence);

        Sequence checkCoverSequence = new Sequence("Sequence Check Cover");
        //safe?
        //stand

        Sequence defendPositionSequence = new Sequence("Sequence Defend Position");
        defendPositionSequence.AddChild(spotTarget);
        defendPositionSequence.AddChild(gotoDefendPosition);
        defendPositionSequence.AddChild(crouch);
        defendPositionSequence.AddChild(coverFireFallback);
        //check cover safety sequence

        Sequence patrolSequence = new Sequence("Sequence Patrol");
        patrolSequence.AddChild(notspotTarget);
        patrolSequence.AddChild(defendingPatrol);

        Fallback roamFallback = new Fallback("Fallback Search");
        roamFallback.AddChild(defendPositionSequence);
        roamFallback.AddChild(patrolSequence);

        Fallback rootFallback = new Fallback("Fallback Root");
        rootFallback.AddChild(roamFallback);

        BT.AddChild(rootFallback);
        BT.PrintTree();
    }
}
