using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("HP")]
    [SerializeField]protected int hp;
    [SerializeField]protected int MaxHP;
    [Header("Booleans")]
    [SerializeField]protected bool dead;

    public int HP()
    {
        return Math.Max(hp, 0);
    }
    public void TakeDamage(int value)
    {
        hp-=value;
    }
    public bool Death()
    {
        if(hp>0)
        {
            
            return false;
        }
        else
        {
            this.gameObject.tag = "DeadBody";
            this.gameObject.layer = 30;
            return true;
        }
    }
}
