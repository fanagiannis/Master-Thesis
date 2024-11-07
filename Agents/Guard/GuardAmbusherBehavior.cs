using System.Collections;
using System.Collections.Generic;
using Actions;
using Actions.GuardActions;
using Behavior;
using Conditions;
using UnityEngine;

public class GuardAmbusherBehavior : GuardBehavior
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
        BT = new BehaviorTree("Ambusher Guard Logic");
        
        //CONDITIONS
        Condition notspotTarget = new Condition("Condition Not Target Spotted?", new ConditionLeaf(() => !sensors.GetVisibleTarget()  && !InDanger));
        Condition spotTarget = new Condition("Condition Target Spotted?", new ConditionLeaf(() => sensors.GetVisibleTarget()   && !InDanger));
        Condition cantShoot = new Condition("Condition CantShootTarget?", new ConditionLeaf(() => !targetInRange && !InDanger));
        Condition canShoot = new Condition("Condition CanShootTarget?", new ConditionLeaf(() => targetInRange));
        Condition playerInRange = new Condition("Condition PlayerInRange?",new ConditionLeaf(()=>sensors.ActiveVisibleTarget()));
        Condition notplayerInRange = new Condition("Condition not PlayerInRange?",new ConditionLeaf(()=>!sensors.ActiveVisibleTarget()));

        //ACTIONS
        Action crouch = new Action("Action Crouch",new Crouch(this.animator));
        Action lookAt = new Action("Action Look At Target", new LookAtTarget(this.navigation,this.animator, () => SecurityManager.Instance.Target()));
        Action setDanger = new Action("Action Set Danger", new SetDanger(this, true));
        Action stand = new Action("Action Stand", new Stand(this.animator));
        Action aim = new Action("Action Aim At Target", new Aim(this.animator));
        Action shootAction = new Action("Action Shoot Target", new ShootAction( animator, Shoot, entity.Damage(), ()=>sensors.GetVisibleTarget() ));
        

        Sequence crouchSequence = new Sequence("Sequence Crouch");
        crouchSequence.AddChild(notplayerInRange);
        crouchSequence.AddChild(crouch);

        Sequence spotSequence = new Sequence("Sequence Spot Player");
        spotSequence.AddChild(playerInRange);
        spotSequence.AddChild(lookAt);
        spotSequence.AddChild(stand);
        spotSequence.AddChild(aim);


        Fallback rootFallback = new Fallback("Fallback Root");
        rootFallback.AddChild(crouchSequence);
        rootFallback.AddChild(spotSequence);
        

        BT.AddChild(rootFallback);
        BT.PrintTree();
    }
}
