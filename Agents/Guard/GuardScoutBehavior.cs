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
    public override void BakeBehavior()
    {
        BT = new BehaviorTree("Scout Guard Logic");

        //CONDITIONS

        Condition notspotTarget = new Condition("Condition Not Target Spotted?", new ConditionLeaf(() => !lineOfSight.GetVisibleTarget()  && !InDanger));
        Condition spotTarget = new Condition("Condition Target Spotted?", new ConditionLeaf(() => lineOfSight.GetVisibleTarget()   && !InDanger));

        //SHOOT
        Condition cantShoot = new Condition("Condition CantShootTarget?", new ConditionLeaf(() => !targetInRange && !InDanger));
        Condition canShoot = new Condition("Condition CanShootTarget?", new ConditionLeaf(() => targetInRange));

        Fallback rootFallback = new Fallback("Fallback Root");

        BT.AddChild(rootFallback);
        BT.PrintTree();
    }
}
