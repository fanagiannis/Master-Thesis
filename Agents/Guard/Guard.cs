using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Behavior;
using Actions;
using Conditions;
using System.ComponentModel;

public class Guard : Agent
{   
    [SerializeField]private Transform safezone;
    [SerializeField]protected float speed;
    protected Animator animator;
    protected bool playerSpotted;
    [SerializeField]protected Transform playerPosition;
    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        BakeBehavior();
        this.navigation.speed = speed;
    }
    public override void Update()
    {
        BT.Process();
        if(!InDanger)
        {
            animator.SetBool("IsCrouching",false);//DEBUG!!!!!!!!!!!!
            navigation.speed = speed;
        }
    }
    public override void BakeBehavior()
    {
        IAction crouch =new Crouch(this.animator); 

        BT=new BehaviorTree("Guard Logic");

        Sequence guardPatrol = new Sequence("Patrol");
        Condition notspotPlayer = new Condition("PlayerSpotted?",new ConditionLeaf(()=>!playerSpotted));
        Action patrol = new Action("Guard Patrol",new GuardRandomPatrol(this,this.navigation,this.animator));

        guardPatrol.AddChild(notspotPlayer);
        guardPatrol.AddChild(patrol);

        Sequence hideSequence = new Sequence("Take Cover");
        Condition checkIfDanger = new Condition("Threatened?",new ConditionLeaf(()=>InDanger));
        Action takeCover = new Action("Take Cover",new GoTo(this.animator,this.navigation,()=>safezone.position));
        Action crouchAction = new Action("Crouch",crouch);

        Condition safe = new Condition("Safe?",new ConditionLeaf(()=>!InDanger));
        Action standUp = new Action("Stand",new ActionReset(crouch));


        Sequence PlayerSpot = new Sequence("Spot Player");
        Condition spotPlayer = new Condition("PlayerSpotted?",new ConditionLeaf(()=>playerSpotted));
        Action lookAt = new Action("LookAtPlayer",new LookAtTarget(this.navigation,this.animator,playerPosition));
        Action shootAction = new Action("ShootPlayer",new ShootAction(playerPosition, 5000f, animator));

        PlayerSpot.AddChild(spotPlayer);
        PlayerSpot.AddChild(lookAt);
        PlayerSpot.AddChild(shootAction);


        hideSequence.AddChild(checkIfDanger);
        hideSequence.AddChild(takeCover);
        hideSequence.AddChild(crouchAction);
        hideSequence.AddChild(safe);
        hideSequence.AddChild(standUp);
        
        
        Fallback rootfallback = new Fallback("Root");

        rootfallback.AddChild(PlayerSpot);
        rootfallback.AddChild(hideSequence);
        rootfallback.AddChild(guardPatrol);
        

        BT.AddChild(rootfallback);
        BT.PrintTree();
    }

    public void SetPlayerSpotted()
    {
        playerSpotted = true;
    }
}
