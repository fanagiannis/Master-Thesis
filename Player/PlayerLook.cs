using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UIElements.Experimental;

public class PlayerLook : MonoBehaviour
{
    [SerializeField]private Transform Player;
    public float Sensitivity;
    private float RotationY,RotationX,Pitch,Yaw;
    void Start()
    {
        SetUpFollowCamera();
        Cursor.visible = false;
    }
    void Update()
    {
        Look();
    }
    private void SetUpFollowCamera()
    {
        Player PlayerGO = GetComponent<Player>();
        Player=PlayerGO.transform;
    }
    void Look()
    {
        Vector2 lookInput=Vector2.zero;
        var inputDevice = Player.gameObject.GetComponent<Player>().PlayerInput().currentControlScheme;
        Sensitivity= inputDevice=="Keyboard"? 10f: 100f;
        
        lookInput=Player.gameObject.GetComponent<Player>().PlayerInput().actions["Look"].ReadValue<Vector2>();
        RotationX = lookInput.x*Sensitivity*Time.deltaTime;
        RotationY = lookInput.y*Sensitivity*Time.deltaTime;
        Pitch-=RotationY;
        Yaw+=RotationX;
        Pitch= Mathf.Clamp(Pitch, -80,80);
        Player.Rotate(Vector3.up,RotationX);
        Player.GetComponent<Player>().GetCamera().transform.localRotation=Quaternion.Euler(Pitch,Yaw,0);   
    }
}
