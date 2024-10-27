using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.UIElements;

public class GamemodeManager : MonoBehaviour
{
    public static GamemodeManager Instance;
    public List<Transform> spawnLocations;
    public List<Zombie> zombiesList;
    public List<Guard> guardsList;
    public GameObject zombiePrefab,guardPrefab;
    public float timer;
    void Start()
    {
        Instance=this;
        zombiesList=new List<Zombie>();
        guardsList=new List<Guard>();
        ResetTimer();
    }
    void Update()
    {
        //ZombieSpawner();
        //GuardSpawner();
    }

    void ResetTimer()
    {
        timer=Random.Range(0.5f,3f);
    }
    void ZombieSpawner()
    {
        timer-=Time.deltaTime;
        if (timer < 0)
        {
            var obj = Instantiate(zombiePrefab,spawnLocations[Random.Range(0,spawnLocations.Count)]);
            zombiesList.Add(obj.gameObject.GetComponent<Zombie>());
            ResetTimer();
        }
    }

    void GuardSpawner()
    {
        timer-=Time.deltaTime;
        if (timer < 0)
        {
            
            var obj = Instantiate(guardPrefab,spawnLocations[Random.Range(0,spawnLocations.Count)]);
            guardsList.Add(obj.gameObject.GetComponent<Guard>());
            ResetTimer();
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
}
