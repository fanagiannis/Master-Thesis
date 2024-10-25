using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardAnimationController : AnimationController
{
    private string walk,run,crouch,alert,death;
    protected override void Awake()
    {
        base.Awake();
        walk="IsWalking";
        crouch="IsCrouching";
        run="IsRunning";
        alert="Alert";
        death="Dead";
        Idle();        
        ResetTriggers();
    }
    public override void Idle()
    {
        ResetAll();
    }
    public override void Walk()
    {
        ResetAnimation(run);
        SetAnimation(walk);
        ResetAnimation(crouch);
        ResetAnimation(alert);
    }
    public override void Run()
    {
        SetAnimation(run);
        ResetAnimation(walk);
        ResetAnimation(crouch);
        ResetAnimation(alert);
    }
    public void Alert()
    {
        ResetAnimation(run);
        ResetAnimation(walk);
        ResetAnimation(crouch);
        ResetAnimation(alert);
    }
    public void Crouch()
    {
        ResetAnimation(run);
        ResetAnimation(walk);
        SetAnimation(crouch);
        ResetAnimation(alert);
    }
    public void CrouchWalk()
    {
        ResetAnimation(run);
        SetAnimation(walk);
        SetAnimation(crouch);
        ResetAnimation(alert);
    }
    public void AlertWalk()
    {
        ResetAnimation(run);
        SetAnimation(walk);
        ResetAnimation(crouch);
        SetAnimation(alert);
    }
    public void AlertRun()
    {
        SetAnimation(run);
        ResetAnimation(walk);
        ResetAnimation(crouch);
        SetAnimation(alert);
    }

    public void TriggerDeath()
    {
        ResetAll();
        Trigger(death);
    }
    public void ResetAll()
    {
        ResetAnimation(run);
        ResetAnimation(walk);
        ResetAnimation(crouch);
        ResetAnimation(alert);
    }
    public override void ResetTriggers()
    {
        ResetTrigger(death);
    } 
}