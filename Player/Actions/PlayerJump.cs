using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerJump : MonoBehaviour
{
    [Header("Jump Properties")]
    [SerializeField] private float JumpHeight=2.4f;
    [SerializeField] private float JumpScale=-0.5f;
    [SerializeField] private float JumpStaminaCost;
    public void Jump(CharacterController controller)
    {
        if(this.GetComponent<Player>().PlayerInput().actions["Jump"].triggered&&controller.isGrounded&&this.GetComponent<Player>().Data.Stamina()>JumpStaminaCost){
            this.GetComponent<Player>().Data.AddStamina(-JumpStaminaCost);
            this.GetComponent<Gravity>().velocity.y = Mathf.Sqrt(JumpHeight*JumpScale*this.GetComponent<Gravity>().GravityScale);
        }
    }
}
