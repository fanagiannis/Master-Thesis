using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    [SerializeField]private CharacterController Controller;
    [SerializeField]private Gravity gravity;
    [SerializeField]private PlayerInput playerInput;
    [SerializeField]private PlayerAnimationController animcontroller;
    private bool IsCrouching = false;
    void Start()
    {
        Controller=GetComponent<CharacterController>();
        playerInput=this.gameObject.GetComponent<PlayerInput>();
        gravity=this.gameObject.GetComponent<Gravity>();
        playerInput.actions.Enable();
        animcontroller=this.gameObject.GetComponent<PlayerAnimationController>();
        Cursor.visible=false;
        
    }
    public void Control()
    {    
        if(!GetComponent<PlayerShoot>().Aiming())
        {
            Movement();
            Crouch();
            
        }
        else if(GetComponent<PlayerShoot>().Aiming())
        {
            animcontroller.ResetAll();
        }
        Look();
        
        gravity.Apply(Controller);
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
    private void Crouch()
    {
        if(playerInput.actions["Crouch"].ReadValue<float>() > 0)
        {
            if(IsCrouching)
            {
                animcontroller.ResetCrouch();
                StartCoroutine(CrouchDelay(false));
            }
            else
            {
                animcontroller.Crouch();
                StartCoroutine(CrouchDelay(true));
            }
            
        }
         
        
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
    private IEnumerator CrouchDelay(bool value)
    {
        yield return new WaitForSeconds(0.2f);
        IsCrouching=value;
        yield break;
    }
    public PlayerInput PlayerInput(){return playerInput;}
    public bool HasJumped(){return !Controller.isGrounded; }
    public PlayerAnimationController GetAnimationController(){return animcontroller;} 
}
