using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    private enum PlayerState{Alive,Dead};
    [SerializeField]private GameObject cameraPrefab;
    [SerializeField]private GameObject FollowCamera;
    public EntityData Data;
    [SerializeField]private CharacterController Controller;
    [SerializeField]private PlayerInput playerInput;
    [SerializeField]private PlayerState currentState=PlayerState.Alive;
    public UnityEvent IsWalking,IsIdle;
    void Start()
    {
        Data.Start();
        Controller=GetComponent<CharacterController>();
        playerInput=this.gameObject.GetComponent<PlayerInput>();
        playerInput.actions.Enable();
    }
    void Update()
    {
        if (Data.Dead())
        {
            currentState=PlayerState.Dead;
        }
        switch (currentState)
        {
            case PlayerState.Alive:
                Movement();
                this.GetComponent<Gravity>().gravity(Controller);
                this.GetComponent<PlayerJump>().Jump(Controller);
                if(playerInput.actions["Movement"].ReadValue<Vector2>().magnitude > 0)
                {
                    IsWalking.Invoke();
                }
                else
                {

                    IsIdle.Invoke();
                }
                break;
            case PlayerState.Dead:
                gameObject.SetActive(false);
                break;
        }
    }
    private void Movement()
    {
        float Move_X=playerInput.actions["Movement"].ReadValue<Vector2>().x;//Input.GetAxis("Horizontal");
        float Move_Z=playerInput.actions["Movement"].ReadValue<Vector2>().y;//Input.GetAxis("Vertical");
        Data.Velocity(this.gameObject.transform.right*Move_X+this.gameObject.transform.forward*Move_Z);
        Controller.Move(Data.Velocity()*Data.Speed()*Time.deltaTime);
    }
    public GameObject GetCamera(){return FollowCamera;}
    public PlayerInput PlayerInput(){return playerInput;}
    public bool HasJumped(){return !Controller.isGrounded; }
}

