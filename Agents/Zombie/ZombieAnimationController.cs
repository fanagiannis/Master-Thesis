using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAnimationController : AnimationController
{
    protected override void Awake()
    {
        base.Awake();
        Idle();        ResetTriggers();
    }
    public override void Walk()
    {
        animator.SetBool("IsRunning",false);
        animator.SetBool("IsWalking",true);
    }
    public override void Run()
    {
        animator.SetBool("IsRunning",true);
        animator.SetBool("IsWalking",false);
    }
    public override void Idle()
    {
        animator.SetBool("IsRunning",false);
        animator.SetBool("IsWalking",false);
    }
    public override void Hit()
    {
        Idle();
        animator.SetTrigger("Hit");
    }
    public override void Scream()
    {
        Idle();
        animator.SetTrigger("Scream");
    }  
    public override void ResetTriggers()
    {
        animator.ResetTrigger("Scream");
        animator.ResetTrigger("Hit");
    } 
}
