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
    [SerializeField]private Transform safezone;
    [SerializeField]protected float speed;
    protected GuardAnimationController animator;
    [SerializeField]protected bool playerSpotted;
    [SerializeField]protected Transform playerPosition;
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
        BT.Process();

        //DEBUG!!!!!!!!!!!!
        if(!InDanger)
        {
            navigation.speed = speed;
        }
        if(!playerAlive)
        {
            playerSpotted=false;
        }
        //DEBUG!!!!!!!!!!!!
    }
    public override void BakeBehavior()
    {
        IAction crouch =new Crouch(this.animator); 

        BT=new BehaviorTree("Guard Logic");

        Sequence guardPatrol = new Sequence("Patrol");
        Condition notspotPlayer = new Condition("PlayerSpotted?",new ConditionLeaf(()=>!playerSpotted && !InDanger));
        Action patrol = new Action("Guard Patrol",new GuardRandomPatrol(this,this.navigation,this.animator));

        guardPatrol.AddChild(notspotPlayer);
        guardPatrol.AddChild(patrol);

        Sequence hideSequence = new Sequence("Take Cover");
        Condition checkIfDanger = new Condition("Threatened?",new ConditionLeaf(()=>InDanger));
        Action takeCover = new Action("Take Cover",new GuardGoTo(this.animator,this.navigation,()=>safezone.position));
        Action crouchAction = new Action("Crouch",crouch);

        Condition safe = new Condition("Safe?",new ConditionLeaf(()=>!InDanger));
        Action standUp = new Action("Stand",new ActionReset(crouch));


        Sequence PlayerSpot = new Sequence("Spot Player");
        Condition spotPlayer = new Condition("PlayerSpotted?",new ConditionLeaf(()=>playerSpotted && playerAlive));
        Condition checkPlayer = new Condition("PlayerAlive?",new ConditionLeaf(()=>playerAlive));

        
        Action lookAt = new Action("LookAtPlayer",new LookAtTarget(this.navigation,this.animator,()=>playerPosition));

        Sequence shootSequence = new Sequence("Shoot Player Sequence");
       
        Action shootAction = new Action("ShootPlayer",new ShootAction(playerPosition, animator , Shoot));

        Action test = new Action("Debug",new Test("debug"));
        WaitNode delay = new WaitNode("Delay",3f);
        Sequence delayAndShootSequence = new Sequence("Delay and Debug Sequence");
        delayAndShootSequence.AddChild(delay);   
        delayAndShootSequence.AddChild(shootAction);    
        RepeatNode repeat = new RepeatNode("Repeat Shoot", delayAndShootSequence, () => playerAlive);

        shootSequence.AddChild(repeat);

        PlayerSpot.AddChild(spotPlayer);
        PlayerSpot.AddChild(lookAt);
        PlayerSpot.AddChild(repeat);

        hideSequence.AddChild(checkIfDanger);
        hideSequence.AddChild(takeCover);
        hideSequence.AddChild(crouchAction);
        hideSequence.AddChild(safe);
        hideSequence.AddChild(standUp);
        
        
        Fallback rootfallback = new Fallback("Root");

        rootfallback.AddChild(hideSequence);
        rootfallback.AddChild(PlayerSpot);
        rootfallback.AddChild(guardPatrol);
        

        BT.AddChild(rootfallback);
        BT.PrintTree();
    }

    public void SetPlayerSpotted()
    {
        playerSpotted = true;
    }
}
    

