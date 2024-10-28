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
    public bool PlayerSpotted()
    {
        return EnemyMaster.Instance.Target() != null && EnemyMaster.Instance.PlayerAlive() && lineOfSight.ActiveVisiblePlayer();
    }
    public void SetPlayerDead()
    {
        EnemyMaster.Instance.ResetPlayerAlive();
    }
}
