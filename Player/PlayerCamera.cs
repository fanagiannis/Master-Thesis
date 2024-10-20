using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]private GameObject Player;
    [SerializeField]public Vector3 Offset;
    void Update()
    {
        this.GetComponent<Camera>().transform.position=Player.transform.position+Offset;
    }
    public GameObject GetPlayer()
    {
        return Player;
    }
}
