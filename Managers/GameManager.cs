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
    public GameObject zombiePrefab;
    public float timer;
    void Start()
    {
        Instance=this;
        zombiesList=new List<Zombie>();
        ResetTimer();
    }
    void Update()
    {
        timer-=Time.deltaTime;
        if (timer < 0)
        {
            var obj = Instantiate(zombiePrefab,spawnLocations[Random.Range(0,spawnLocations.Count)]);
            zombiesList.Add(obj.gameObject.GetComponent<Zombie>());
            ResetTimer();
        }
    }

    void ResetTimer()
    {
        timer=Random.Range(0.5f,3f);
    }

    public void PlayerDead()
    {
        foreach (Zombie zombie in zombiesList)
        {
            zombie.gameObject.GetComponent<ZombieBehavior>().SetPlayerDead();
        }
    }
}
