using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Lists")]
    public List<Transform> spawnLocations;
    public List<Zombie> zombiesList;
    public List<Guard> guardsList;
    [Header("Prefabs")]
    public GameObject zombiePrefab;
    public GameObject guardPrefab;
    [Header("Timer")]
    public float timer;
    void Awake()
    {
        zombiesList=new List<Zombie>();
        guardsList=new List<Guard>();
        ResetTimer();
        SearchGuards();
    }
    void Update()
    {
        //ZombieSpawner();
        //GuardSpawner();
    }
    private void ResetTimer()
    {
        timer=Random.Range(2f,3f);
    }
    private void ZombieSpawner()
    {
        timer-=Time.deltaTime;
        if (timer < 0)
        {
            var obj = Instantiate(zombiePrefab,spawnLocations[Random.Range(0,spawnLocations.Count)]);
            zombiesList.Add(obj.gameObject.GetComponent<Zombie>());
            ResetTimer();
        }
    }

    private void GuardSpawner()
    {
        timer-=Time.deltaTime;
        if (timer < 0)
        {
            
            var obj = Instantiate(guardPrefab,spawnLocations[Random.Range(0,spawnLocations.Count)]);
            guardsList.Add(obj.gameObject.GetComponent<Guard>());
            ResetTimer();
        }
    }

    private void SearchGuards()
    {
        GameObject[] guardObjects = GameObject.FindGameObjectsWithTag("Guard");
        foreach (var guardObject in guardObjects)
        {
            Guard guard = guardObject.GetComponent<Guard>();
            if (guard != null)
            {
                guardsList.Add(guard);
            }
        }
    }

    public void PlayerDead()
    {
        foreach (Zombie zombie in zombiesList)
        {
            zombie.gameObject.GetComponent<ZombieBehavior>().SetPlayerDead();
        }
        foreach (Guard guard in guardsList)
        {
            guard.gameObject.GetComponent<GuardBehavior>().SetPlayerDead();
        }
    }
    public void ResetPlayerAlive()
    {
        SecurityManager.Instance.SetPlayerAlive(false);
    }
    public bool PlayerAlive()
    {
        return SecurityManager.Instance.PlayerAlive();
    }
}
