using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileAgent : Agent
{
    
    [SerializeField]protected bool playerInRange;
    [SerializeField]protected bool targetInRange;
    public void ResetPlayerAlive()
    {
        EnemyMaster.Instance.ResetPlayerAlive();
        InDanger=false;
    }
    public bool TargetSpotted()
    {
        return lineOfSight.GetVisibleTarget() != null  && lineOfSight.ActiveVisibleTarget();
    }
    public void SetPlayerDead()
    {
        EnemyMaster.Instance.ResetPlayerAlive();
    }
}
