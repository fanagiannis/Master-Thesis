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
        //Idle();        
        //ResetTriggers();
    }
    public override void Idle()
    {
        ResetAnimation(run);
        ResetAnimation(walk);
        ResetAnimation(crouch);
    }
    public override void Walk()
    {
        ResetAnimation(run);
        SetAnimation(walk);
        ResetAnimation(crouch);
        //ResetAnimation(alert);
    }
    public override void Run()
    {
        SetAnimation(run);
        ResetAnimation(walk);
        ResetAnimation(crouch);
        //ResetAnimation(alert);
    }
    public void Alert()
    {
        SetAnimation(alert);
    }
    public void ResetAlert()
    {
        ResetAnimation(alert);
    }
    public void Crouch()
    {
        ResetAnimation(run);
        ResetAnimation(walk);
        SetAnimation(crouch);
        //ResetAnimation(alert);
    }
    public void CrouchWalk()
    {
        ResetAnimation(run);
        SetAnimation(walk);
        SetAnimation(crouch);
        //ResetAnimation(alert);
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