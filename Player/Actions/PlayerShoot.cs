using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerShoot : MonoBehaviour
{
    private Player player;
    public UnityEvent Fire;
    void Start()
    {
        //Fire.AddListener(Debugger);
        player = GetComponent<Player>();
    }
    void Update()=>Shoot();
    private void Shoot()
    {
        if (player.PlayerInput().actions["Fire"].triggered)
        {
            RaycastHit hit;
             if (Physics.Raycast (player.GetCamera().transform.position, player.GetCamera().transform.forward, out hit))
            {
                Debug.Log(hit);
                // Set the end position for our laser line 
            //Fire.Invoke();
            }
        }
    }
    private void Debugger()
    {
        Debug.Log("Boom");    
    }
}
