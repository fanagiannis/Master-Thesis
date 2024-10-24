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
        Animations();
    }
    
    private void Movement()
    {
        float Move_X=playerInput.actions["Movement"].ReadValue<Vector2>().x;
        float Move_Z=playerInput.actions["Movement"].ReadValue<Vector2>().y;
        Vector3 move = transform.right * Move_X + transform.forward * Move_Z;

        Controller.Move(move*moveSpeed*Time.deltaTime);
    }
    private void Look()
    {
        Vector2 mouseInput = playerInput.actions["Look"].ReadValue<Vector2>();
        float mouseX = mouseInput.x * 10f * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);
    }
    private void Animations()
    {
        Debug.Log(Controller.velocity);
     
        Vector3 velocity = Controller.velocity;
        float speed = velocity.magnitude; 
        if (speed > 0.1f) 
        {
           
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            if (localVelocity.z > 0) 
            {
                animcontroller.WalkForward();
            }
            else if (localVelocity.z < 0) 
            {
                animcontroller.WalkBackwards();
            }

            if (localVelocity.x > 0) 
            {
                animcontroller.WalkRight();
            }
            else if (localVelocity.x < 0) 
            {
                animcontroller.WalkLeft();
            }
        }
        else
        {
            animcontroller.ResetAll();
        }
    
    }
    public PlayerInput PlayerInput(){return playerInput;}
    public bool HasJumped(){return !Controller.isGrounded; }
}
