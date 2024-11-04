using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Behavior;
using Conditions;
using Actions.GuardActions;
using UnityEngine.UIElements;

public class GuardBomberBehavior : GuardBehavior
{
    [Header("Explosion")]
    [SerializeField]private GameObject explosionPrefab;
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
        BT = new BehaviorTree("Bomber Guard Logic");

        //CONDITIONS

        Condition securityAlert = new Condition("Condition Security Alert?",new ConditionLeaf(()=>SecurityManager.Instance.CurrentSecurityState() == SecurityManager.SecurityState.Alert));
        Condition inRange = new Condition("Condition Target In?", new ConditionLeaf(() => targetInRange));

        //ACTIONS
        Action chaseTarget = new Action("Action Chase Target",new GuardGoTo(animator,navigation,()=>SecurityManager.Instance.Target().position));

        //DEBUG
        Action explode = new Action("Action Explode",new Explode(this.gameObject,explosionPrefab,transform));
        //DEBUG

        Sequence explodeSequence = new Sequence("Sequence Explode");
        explodeSequence.AddChild(inRange);
        explodeSequence.AddChild(explode);

        Sequence chaseTargetSequence = new Sequence("Sequence Chase Target");
        chaseTargetSequence.AddChild(chaseTarget);
        chaseTargetSequence.AddChild(explodeSequence);

        Sequence activateSequence = new Sequence("Sequence Activate");
        activateSequence.AddChild(securityAlert);
        activateSequence.AddChild(chaseTargetSequence);


        Fallback rootFallback = new Fallback("Fallback Root");
        rootFallback.AddChild(activateSequence);

        BT.AddChild(rootFallback);
        BT.PrintTree();
    }
}
