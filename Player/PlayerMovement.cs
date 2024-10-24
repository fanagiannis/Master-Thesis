using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed; 
    [SerializeField]private CharacterController Controller;
    [SerializeField]private Gravity gravity;
    [SerializeField]private PlayerInput playerInput;
    [SerializeField]private PlayerAnimationController animcontroller;
    void Start()
    {
        Controller=GetComponent<CharacterController>();
        playerInput=this.gameObject.GetComponent<PlayerInput>();
        gravity=this.gameObject.GetComponent<Gravity>();
        playerInput.actions.Enable();
        animcontroller=this.gameObject.GetComponent<PlayerAnimationController>();
    }
    void Update()
    {   
        Movement();
        Look();
        //gravity.Apply(Controller);
    }
    
    private void Movement()
    {
        float Move_X=playerInput.actions["Movement"].ReadValue<Vector2>().x;
        float Move_Z=playerInput.actions["Movement"].ReadValue<Vector2>().y;
        Vector3 move = transform.right * Move_X + transform.forward * Move_Z;

        Controller.Move(move*moveSpeed*Time.deltaTime);
        Animations(Move_X, Move_Z);
    }
    private void Look()
    {
        Vector2 mouseInput = playerInput.actions["Look"].ReadValue<Vector2>();
        float mouseX = mouseInput.x * 10f * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);
    }
    private void Animations(float moveX, float moveZ)
    {
        animcontroller.ResetAll();
        if (Mathf.Abs(moveX) > 0.1f || Mathf.Abs(moveZ) > 0.1f) 
        {
            if (moveZ > 0.1f) 
            {
                animcontroller.WalkForward();
            }
            else if (moveZ < -0.1f) 
            {
                animcontroller.WalkBackwards();
            }
            if (moveX > 0.1f) 
            {
                animcontroller.WalkRight();
            }
            else if (moveX < -0.1f) 
            {
                animcontroller.WalkLeft();
            }
        }
    
    }
    public PlayerInput PlayerInput(){return playerInput;}
    public bool HasJumped(){return !Controller.isGrounded; }
}
