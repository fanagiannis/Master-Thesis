using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityManager : MonoBehaviour
{
    public static SecurityManager Instance;
    public enum SecurityState{Alert,Cautious,Idle}
    [SerializeField]private EnemyManager enemyManager;
    [SerializeField]private SecurityState currentState;
    void Awake()
    {
        Instance = this;
        enemyManager = GetComponent<EnemyManager>();
    }
    public string State()
    {
        return currentState.ToString();
    }
    public EnemyManager GetEnemyManager()
    {
        return enemyManager;
    }
    public bool GetPlayerAlive()
    {
        return enemyManager.PlayerAlive();
    }
    public void ResetPlayerAlive()
    {
        enemyManager.ResetPlayerAlive();
    }
}
