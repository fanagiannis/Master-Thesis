using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Stats", menuName = "New ScriptableObject/SplitScreen/Stats")]
public class Stats : ScriptableObject
{
    [SerializeField]private float kills=0;
    [SerializeField]private float deaths=0;
    [SerializeField]private int score=0;
    private float KDratio;
    public float Kills
    {
        get { return kills; }
        private set{}
    }
    public float Deaths
    {
        get { return deaths; }
        private set{}
    }
    public int Score
    {
        get { return score; }
        private set{}
    }
    public void GetKill()
    {
        kills+=1;
        score+=100;
    }
    public void GetDeath()
    {
        deaths+=1;
        score-=10;
    }
    public float KD()
    {
        if(deaths!=0)
        {
            return kills/deaths;
        }
        else
        {
            return kills;
        }
    }
}
