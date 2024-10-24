using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private UnityEvent Shoot;
    [SerializeField]private UnityEvent StopShooting; 
    [SerializeField] private float fireRate = 0.1f; 
    private PlayerInput playerInput;
    private float nextFireTime;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (playerInput.actions["Fire"].ReadValue<float>() > 0)
        {
            if (Time.time >= nextFireTime)
            {
                Shoot.Invoke();  
                nextFireTime = Time.time + fireRate;  
            }
        }
        else
        {
            StopShooting.Invoke();
        }
    }
    public void debug()
    {
        Debug.Log("Shot");
    }
}
