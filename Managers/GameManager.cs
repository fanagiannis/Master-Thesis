using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class GamemodeManager : MonoBehaviour
{
    public static GamemodeManager Instance;
    public PlayerSpawner playerSpawner;
    public List weaponsList;
    public int listIndex;
    public GameObject activeweapon;
    void Start()
    {
        Instance=this;
        //activeweapon=RandomWeapon();
        // if(playerSpawner!=null)
        // {
        //     playerSpawner.SpawnPlayers();
        // } 
    }
    public GameObject RandomWeapon()
    {
        return weaponsList.Item(Random.Range(0, weaponsList.prefabList.Count));
    }
}
