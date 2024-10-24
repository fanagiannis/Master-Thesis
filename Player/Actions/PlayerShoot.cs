using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField]private UnityEvent Shoot;
    private PlayerInput playerInput;
    void Awake()=>playerInput = GetComponent<PlayerInput>();
    private void Update()
    {
        if (playerInput.actions["Fire"].triggered)
        {
            Shoot.Invoke();
        }
    }
    public void debug()
    {
        Debug.Log("Shot");
    }
}
