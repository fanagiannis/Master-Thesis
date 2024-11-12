using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SecurityManager : MonoBehaviour
{
    public static SecurityManager Instance;
    public enum SecurityState{Alert,Cautious,Idle}
    [Header("Enemy Manager")]
    [SerializeField]private EnemyManager enemyManager;
    [Header("Security State")]
    [SerializeField]private SecurityState currentState;
    [Header("Target")]
    [SerializeField]private Transform targetPosition;
    [SerializeField]private bool playerAlive=true;
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
    public void Alert()
    {
        currentState=SecurityState.Alert;
    }
    public Transform Target()
    {
        return targetPosition;
    }
    public SecurityState CurrentSecurityState()
    {
        return currentState;
    }
    public bool PlayerAlive()
    {
        return playerAlive;
    }
    public void SetPlayerAlive(bool set)
    {
        playerAlive=set;
    }
}
