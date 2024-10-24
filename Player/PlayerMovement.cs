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
    void Start()
    {
        Controller=GetComponent<CharacterController>();
        playerInput=this.gameObject.GetComponent<PlayerInput>();
        gravity=this.gameObject.GetComponent<Gravity>();
        playerInput.actions.Enable();
    }
    void Update()
    {   
        Movement();
        Look();
        gravity.Apply(Controller);
    }
    
    private void Movement()
    {
        float Move_X=playerInput.actions["Movement"].ReadValue<Vector2>().x;//Input.GetAxis("Horizontal");
        float Move_Z=playerInput.actions["Movement"].ReadValue<Vector2>().y;//Input.GetAxis("Vertical");
        Vector3 move = transform.right * Move_X + transform.forward * Move_Z;

        Controller.Move(move*moveSpeed*Time.deltaTime);
    }
    private void Look()
    {
         Vector2 mouseInput = playerInput.actions["Look"].ReadValue<Vector2>();
        float mouseX = mouseInput.x * 10f * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);
    }
    public PlayerInput PlayerInput(){return playerInput;}
    public bool HasJumped(){return !Controller.isGrounded; }
}
