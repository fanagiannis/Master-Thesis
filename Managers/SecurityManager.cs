using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityManager : MonoBehaviour
{
    public static SecurityManager Instance;
    public enum SecurityState{Alert,Cautious,Idle}
    [SerializeField]private EnemyManager enemyManager;
    [SerializeField]private SecurityState currentState;
    [SerializeField]private Transform targetPosition;
    void Awake()
    {
        Instance = this;
        enemyManager = GetComponent<EnemyManager>();
    }
    void Update()
    {
        switch (currentState)
        {
            case SecurityState.Alert:
            foreach (Guard guard in enemyManager.guardsList)
            {
                guard.gameObject.GetComponent<AISensors>().SpotTarget(targetPosition);
            }
            break;
            case SecurityState.Cautious:
            break;
            default:
            break;
        }
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
    public Transform Target()
    {
        return targetPosition;
    }
}
