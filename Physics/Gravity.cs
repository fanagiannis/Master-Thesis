using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    [SerializeField]public float GravityScale=-9f;
    public Vector3 velocity;
    public void gravity(CharacterController controller)
    {
        if(controller.isGrounded&&velocity.y<0){velocity.y=-2f;}
        velocity.y+=GravityScale*Time.deltaTime;
        controller.Move(velocity*Time.deltaTime);
    }

}
