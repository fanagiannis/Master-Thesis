using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{   
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerShoot playerShoot;
    void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerShoot = GetComponent<PlayerShoot>();
    }
    void Update()
    {
        if(!Death())
        {
            playerController.Control();
            playerShoot.Control();
        }
        else
        {
            playerController.GetAnimationController().TriggerDeath();
        }
    }
}
