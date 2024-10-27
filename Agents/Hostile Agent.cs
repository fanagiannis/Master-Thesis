using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileAgent : Agent
{
    
    [SerializeField]protected bool playerInRange;
    [SerializeField]protected bool playerAlive;
    public void ResetPlayerAlive()
    {
        playerAlive = false;
        InDanger=false;
    }
    public bool PlayerSpotted()
    {
        return targetPosition != null && playerAlive && lineOfSight.ActiveVisiblePlayer();
    }
}
