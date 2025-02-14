using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class GamemodeManager : MonoBehaviour
{
    public static GamemodeManager Instance;
    [Header("Objective")]
    [SerializeField]private bool ObjectiveCompleted=false; 
    [Header("Blueprints")]
    [SerializeField]private List<GameObject> Blueprints=new List<GameObject>();             //NUMBER OF BLUEPRINTS
    [SerializeField]private int BlueprintsCollected=0; 
    [SerializeField]private AudioClip pickupsound;    
    [Header("References")]
    private GameObject playercamera;                                  //NUMBER OF BLUEPRINTS COLLECTED

    void Start()
    {
        Instance = this;
        //Blueprints=GameObject.FindGameObjectsWithTag("Blueprint").ToList();                     
        playercamera=GameObject.FindGameObjectWithTag("MainCamera");               
    }

    public void AddBlueprint()    //INC BLUEPRINT COLLECTED
    {
        BlueprintsCollected++; 
        AudioSource.PlayClipAtPoint(pickupsound,playercamera.transform.position,0.5f);
        if (BlueprintsCollected>=Blueprints.Count){ObjectiveCompleted=true;}
        else{return;}
    }            
    public void CompleteObjective()=>ObjectiveCompleted = true;                             //COMPLETE OBJECTIVE FUNCTION
     //GETTERS
    public int GetBlueprintsCollected(){return BlueprintsCollected;}                       
    public int GetAllBlueprints(){return Blueprints.Count;}
    public bool GetCompleteObjective(){return ObjectiveCompleted;}
}
