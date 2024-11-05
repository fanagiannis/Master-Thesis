using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Behavior;
using Conditions;
using Actions;
using Actions.GuardActions;

public class GuardHeavyBehavior : GuardBehavior
{
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

        //ACTIONS
        Action lookAt = new Action("Action Look At Target", new LookAtTarget(this.navigation, this.animator, () => sensors.GetVisibleTarget() ));
        Action randomSearch = new Action("Action Random Search", new GuardRandomSearch(this.transform, 0f, 80f));

        Sequence searchSequence = new Sequence("Sequence Search");
        searchSequence.AddChild(notspotTarget);
        searchSequence.AddChild(randomSearch);

        Sequence lookatSequence = new Sequence("Sequence LookAt");
        lookatSequence.AddChild(lookAt);
        //SHOOT

        Sequence spotSequence = new Sequence("Sequence Spot");
        spotSequence.AddChild(spotTarget);
        spotSequence.AddChild(lookatSequence);

        Fallback searchFallback = new Fallback("Fallback Search");
        searchFallback.AddChild(spotSequence);
        searchFallback.AddChild(searchFallback);

        Fallback rootFallback = new Fallback("Fallback Root");
        rootFallback.AddChild(searchFallback);

        BT.AddChild(rootFallback);
        BT.PrintTree();
    }
}
