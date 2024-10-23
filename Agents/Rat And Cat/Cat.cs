using System.Collections;
using System.Collections.Generic;
using Behavior;
using Actions;
using UnityEngine;
using Conditions;

public class Cat : Agent
{
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
        BT = new BehaviorTree("Cat Logic");

        // Action patrol = new Action("Patrol",new RandomPatrol(this,this.navigation));

        // Sequence chaseSequence = new Sequence("Chase Sequence");
        
        // Condition spotCondition = new Condition("Spot Rat?", new ConditionLeaf(() => foodOnSight));
        // Action chaseAction = new Action("Chase Rat", new GoTo(this,this.navigation,()=>FoodPosition()));
        // chaseSequence.AddChild(spotCondition);
        // chaseSequence.AddChild(chaseAction);
        
        // Fallback fallbackSequence = new Fallback("Fallback");    

        // fallbackSequence.AddChild(chaseSequence);
        // fallbackSequence.AddChild(patrol);

        // BT.AddChild(fallbackSequence);
        BT.PrintTree();
    }
    public override bool CheckTarget()
    {
        Transform target = lineOfSight.GetVisibleTarget();
        if(target!=null)
        {
            if(target.CompareTag("Rat")){
                foodOnSight=true;
                return true;
            }  
        }
        return false;
    }
}
