using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Entity", menuName = "New ScriptableObject/Entity")]
public class EntityData : ScriptableObject
{
    [SerializeField]private string playerName;
    [Header("HP")]
    [SerializeField]private float maxHp=100;  
    [SerializeField]private float hp;   
    [Header("Stamina")]
    [SerializeField] private float stamina;     
    [SerializeField] private float maxstamina=100;   
    [Header("Movement")]
    [SerializeField]private float speed;
    [SerializeField]private float walkingspeed;
    [SerializeField]private float minspeed;
    [SerializeField]private float maxspeed;
    private bool IsSprinting;
    private Vector3 velocity;

    public void Start()
    {
        hp=maxHp;
        stamina=maxstamina;
        ResetSpeed();
        minspeed=speed/3;
        maxspeed=speed*2;
    }
    
    public bool Dead()
    {
        if(hp<=0){return true;}
        else{return false;}
    }
    public string Name(){return playerName;}
    public float HP(){return Mathf.Max(hp,0);}
    public float Stamina(){return Mathf.Max(hp,0);}
    public float MaxHp(){return maxHp;}
    public float MaxStamina(){return maxstamina;}
    public void Heal(float value){hp+=value;}
    public void TakeDamage(float value){hp-=value;}
    public void AddStamina(float value){stamina+=value;}
    public void ResetStamina(){stamina=maxstamina;}
    public bool IsMoving(){if(velocity!=Vector3.zero){return true;}else{return false;}}
    public void ResetSpeed(){speed=walkingspeed;}
    public void DecSpeed(){speed=minspeed;}
    public void IncSpeed(){speed=maxspeed;}
    public void SetIsSprinting(bool set){IsSprinting=set;}
    public bool GetIsSprinting(){return IsSprinting;}
    public float Speed(){return speed;}
    public float WalkingSpeed(){return walkingspeed;}
    public void Velocity(Vector3 value){velocity=value;}
    public Vector3 Velocity(){return velocity;}
}
