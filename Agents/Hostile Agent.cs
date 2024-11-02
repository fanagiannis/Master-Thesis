using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileAgent : Agent
{
    [SerializeField]protected bool targetInRange;
    [Header("Entity")]
    [SerializeField]protected Guard entity;
    public void ResetPlayerAlive()
    {
        SecurityManager.Instance.ResetPlayerAlive();
        InDanger=false;
    }
    public bool TargetSpotted()
    {
        return sensors.GetVisibleTarget() != null  && sensors.ActiveVisibleTarget();
    }
    public void SetPlayerDead()
    {
        SecurityManager.Instance.ResetPlayerAlive();
    }
}
