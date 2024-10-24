using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAnimationController : AnimationController
{
    private string walk,run,scream,hit,death;
    protected override void Awake()
    {
        base.Awake();
        walk="IsWalking";
        run="IsRunning";
        scream="Scream";
        hit="Hit";
        death="Death";
        Idle();        
        ResetTriggers();
    }
    public override void Walk()
    {
        ResetAnimation(run);
        SetAnimation(walk);
    }
    public override void Run()
    {
        SetAnimation(run);
        ResetAnimation(walk);
    }
    public override void Idle()
    {
        ResetAnimation(run);
        ResetAnimation(walk);
    }
    public override void Hit()
    {
        Idle();
        Trigger(hit);
    }
    public override void Scream()
    {
        Idle();
        Trigger(scream);
    }  
    public void TriggerDeath()
    {
        ResetAll();
        Trigger(death);
    }
    public void ResetAll()
    {
        ResetAnimation(walk);
        ResetAnimation(run);
    }
    public override void ResetTriggers()
    {
        ResetTrigger(scream);
        ResetTrigger(hit);
    } 
}
