using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Entity : MonoBehaviour
{
    
    public enum State{Alive,Dead}
    [Header("State")]
    [SerializeField]protected State currentstate;
    [Header("HP")]
    [SerializeField]protected int hp;
    [SerializeField]protected int MaxHP;
    [Header("Booleans")]
    [SerializeField]protected bool dead;

    public int HP()
    {
        return Math.Max(hp, 0);
    }
    public void Death()
    {
        if(hp>0)
        {
            currentstate=State.Alive;
        }
        else
        {
            currentstate=State.Dead;
            dead=true;
        }
    }
}
