using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Behavior;
using Actions;
using UnityEngine.Events;
using Actions.GuardActions;
using Conditions;

public class GuardScoutBehavior : GuardBehavior
{
    public override void Start()
    {
        base.Start();
        animator.Alert();
    }
    public override void Update()
    {
        base.Update();
    }
    public override void BakeBehavior()
    {
        BT = new BehaviorTree("Scout Guard Logic");

        //CONDITIONS

        Condition notspotTarget = new Condition("Condition Not Target Spotted?", new ConditionLeaf(() => !lineOfSight.GetVisibleTarget()  && !InDanger));
        Condition spotTarget = new Condition("Condition Target Spotted?", new ConditionLeaf(() => lineOfSight.GetVisibleTarget()   && !InDanger));

        //SHOOT
        Condition cantShoot = new Condition("Condition CantShootTarget?", new ConditionLeaf(() => !targetInRange && !InDanger));
        Condition canShoot = new Condition("Condition CanShootTarget?", new ConditionLeaf(() => targetInRange));

        //ACTIONS
        Action searchAction = new Action("Action Search",new GuardSearch(this.gameObject.transform,5f,0f,100f));
        Action setDanger = new Action("Action Set Danger", new SetDanger(this, true));
        Action lookAt = new Action("Action Look At Target", new LookAtTarget(this.navigation, this.animator, () => lineOfSight.GetVisibleTarget() ));
        Action stand = new Action("Action Stand", new Stand(this.animator));
        Action shootAction = new Action("Action Shoot Target", new ShootAction( animator, Shoot, ()=>lineOfSight.GetVisibleTarget() ));

        WaitNode shootDelay = new WaitNode("Delay", 5f); //DELAY CONTROL FROM WEAPON FIRERATE

        Sequence delayAndShootSequence = new Sequence("Sequence Delay and Shoot");
        delayAndShootSequence.AddChild(canShoot);
        delayAndShootSequence.AddChild(shootDelay);
        delayAndShootSequence.AddChild(shootAction);

        RepeatNode repeat = new RepeatNode("Repeat Shoot", delayAndShootSequence, () => EnemyMaster.Instance.PlayerAlive() || lineOfSight.GetVisibleTarget() );

        Sequence shootSequence = new Sequence("Sequence Shoot Target");
        shootSequence.AddChild(repeat);
        shootSequence.AddChild(delayAndShootSequence);

        Sequence shootTargetSequence = new Sequence("Sequence Target Shoot");
        //shootTargetSequence.AddChild(canShoot);
        shootTargetSequence.AddChild(lookAt);
        shootTargetSequence.AddChild(shootSequence);

        Sequence spotSequence = new Sequence("Sequence Spot");
        spotSequence.AddChild(spotTarget);
        spotSequence.AddChild(shootTargetSequence);
        

        Sequence searchSequence = new Sequence("Sequence Search");
        searchSequence.AddChild(notspotTarget);
        searchSequence.AddChild(searchAction);

        Fallback searchFallback = new Fallback("Fallback Search");
        searchFallback.AddChild(spotSequence);
        searchFallback.AddChild(searchSequence);

        Fallback rootFallback = new Fallback("Fallback Root");
        rootFallback.AddChild(searchFallback);

        BT.AddChild(rootFallback);
        BT.PrintTree();
    }
}
