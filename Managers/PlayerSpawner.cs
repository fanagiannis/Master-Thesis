using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner: MonoBehaviour
{
    [SerializeField]private GameObject playerPrefab;
    [SerializeField]private Transform Spawns;
    [SerializeField]private List<Transform> spawnPoints;
    [SerializeField]private int numOfPlayers=1;
    private List<GameObject> players= new List<GameObject>();
    public void SpawnPlayers()
    {
        SetSpawnPoints();
        for (int num = 0; num < numOfPlayers; num++)
        {
            if (spawnPoints.Count > num) 
            {
                GameObject newPlayer = Instantiate(playerPrefab, spawnPoints[num].position, spawnPoints[num].rotation);
                players.Add(newPlayer); 
            }
        }
    }
    void SetSpawnPoints()
    {
        foreach (Transform spawnPoint in Spawns.GetComponentsInChildren<Transform>())
        {
            if (spawnPoint != Spawns) 
            {
                spawnPoints.Add(spawnPoint);
            }
        }
    }
}