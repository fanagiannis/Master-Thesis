using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    protected Animator animator;
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public virtual void Walk(){}
    public virtual void Run(){}
    public virtual void Idle(){}
    public virtual void Hit(){}
    public virtual void Scream(){}   
    public virtual void ResetTriggers(){}
}


