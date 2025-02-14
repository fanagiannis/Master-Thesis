using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlueprintPickup : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="Player"){
            GamemodeManager.Instance.AddBlueprint();       //WHEN THE PLAYER TRIGGERS THE BLUEPRINT'S COLLIDER, A BLUEPRINT IS ADDED
            this.gameObject.SetActive(false);          //DISABLE CURRENT GAME OBJECT
        }
    }
}
