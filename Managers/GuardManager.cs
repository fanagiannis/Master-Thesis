using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<Transform> spawnLocations;
    public List<Zombie> zombiesList;
    public List<Guard> guardsList;
    public GameObject zombiePrefab,guardPrefab;
    [SerializeField]private bool playerAlive=true;
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
        playerAlive=false;
    }
    public bool PlayerAlive()
    {
        return playerAlive;
    }
}
