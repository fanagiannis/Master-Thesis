using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField]private LayerMask InteractableLayerMask;
    [SerializeField] private GameObject Crosshair;
    [SerializeField] private float DepthOfInteraction;
    private Player player;
    void Awake(){player=this.GetComponent<Player>();}
    void Interaction()
    {
        RaycastHit hit;
        if(Physics.Raycast(player.transform.position, player.transform.forward,out hit,DepthOfInteraction,InteractableLayerMask)){
            Crosshair.GetComponent<RawImage>().color = Color.red;
        }
        else{
            Crosshair.GetComponent<RawImage>().color = Color.gray; 
        }
    }
    void Update(){Interaction();}
}
