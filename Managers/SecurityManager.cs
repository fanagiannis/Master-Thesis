using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityManager : MonoBehaviour
{
    public static SecurityManager Instance;
    private EnemyManager enemyManager;

    void Awake()
    {
        Instance = this;
        enemyManager = GetComponent<EnemyManager>();
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
