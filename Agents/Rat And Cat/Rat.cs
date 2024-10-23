using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Behavior;
using Conditions;
using Actions;

public class Rat : Agent
{
    [SerializeField]private Transform safezone;
    [SerializeField]private List<Transform> patrolpoints;
    private bool Safe;
    public override void Start()
    {
        base.Start();
        BakeBehavior();
    }
    public override void Update()
    {
        BT.Process();
        foodOnSight=CheckTarget();
    }

    public override void BakeBehavior()
    {
        BT = new BehaviorTree("Rat Logic");
        
        // Action patrol = new Action("Patrol",new RandomPatrol(this,this.navigation));

        // Sequence dangerSequence = new Sequence("Danger Sequence");
        
        // Condition dangerCondition = new Condition("In Danger?", new ConditionLeaf(() => InDanger));
        // Action dangerAction = new Action("Danger", new GoTo(this,this.navigation,()=>safezone.position));
        // dangerSequence.AddChild(dangerCondition);
        // dangerSequence.AddChild(dangerAction);

        // Sequence foodSequence = new Sequence("Food Sequence");

        // Condition foodOnSightCondition = new Condition("Sees Food?", new ConditionLeaf(()=>foodOnSight));
        // Action goToFood = new Action("Approach Food", new GoTo(this,this.navigation,()=>FoodPosition()));
        // foodSequence.AddChild(foodOnSightCondition);
        // foodSequence.AddChild(goToFood);

        // Fallback fallbackSequence = new Fallback("Fallback Sequence");    
        // fallbackSequence.AddChild(dangerSequence);
        // fallbackSequence.AddChild(foodSequence);
        // fallbackSequence.AddChild(patrol);

        // BT.AddChild(fallbackSequence);
        BT.PrintTree();
    }
    
    public override bool CheckTarget()
    {
        Transform target = lineOfSight.GetVisibleTarget();
        if(target!=null)
        {
            if(target.CompareTag("Food")){
                foodOnSight=true;
                return true;
            }
            if(target.CompareTag("Threat")){
                SetDanger();
                return true;
            }    
        }
        return false;
        
    }
    public void SetSafe()=>Safe = true;
    public void SetDanger()
    {
        InDanger = true;
    }
    public void ResetDanger()=>InDanger = false;

    void BTDebug(BehaviorTree BT)
    {
        Sequence dangerSequence = new Sequence("Danger Sequence");
        
        Condition dangerCondition = new Condition("In Danger?", new ConditionLeaf(() => InDanger));
        Action dangerAction = new Action("Log Danger", new Test("DANGER!"));

        dangerSequence.AddChild(dangerCondition);
        dangerSequence.AddChild(dangerAction);

        Fallback fallbackSequence = new Fallback("Fallback Sequence");
        
        fallbackSequence.AddChild(dangerSequence);

        BT.AddChild(fallbackSequence);
    }
}
