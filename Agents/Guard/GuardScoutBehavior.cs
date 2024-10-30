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

        Fallback rootFallback = new Fallback("Fallback Root");

        BT.AddChild(rootFallback);
        BT.PrintTree();
    }
}
