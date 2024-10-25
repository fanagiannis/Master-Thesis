using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : Entity
{   
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerShoot playerShoot;
    [SerializeField]private UnityEvent PlayerDead;
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
            PlayerDead.Invoke();
        }
    }
}
