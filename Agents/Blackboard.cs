using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Blackboard : MonoBehaviour
{
    public bool InDanger = false;
    public bool playerSpotted = false;
    public bool playerAlive = true;

    public Blackboard()
    {
        
    }

    public bool SetPlayerSpotted()
    {
        return playerSpotted = true;
    }

    public bool SetInDanger()
    {
        return InDanger = true;
    }

    public bool ResetPlayerAlive()
    {
        return playerAlive = false;
    }
}
