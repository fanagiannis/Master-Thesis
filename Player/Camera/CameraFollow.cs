using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]public Transform player;
    [SerializeField]private Vector3 offset;
    void Update()
    {
        this.GetComponent<Camera>().transform.position=player.position+offset;  
    }
    public void SetPlayer(Transform player)=>this.player=player.transform;
}
