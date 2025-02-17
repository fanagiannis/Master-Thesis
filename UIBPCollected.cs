using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIBPCollected : MonoBehaviour
{
    public static UIBPCollected Instance;
    private TextMeshProUGUI UIBlueprintsCollected;
    void Awake()
    {
        Instance=this;
        UIBlueprintsCollected = GetComponent<TextMeshProUGUI>();
    }
    public void UpdateBPCount(int valueA,int valueB)
    {
        UIBlueprintsCollected.text="BLUEPRINTS "+valueA.ToString()+"/"+valueB.ToString();
    }
    public void UpdateBPCount()
    {
        UIBlueprintsCollected.text="ESCAPE THE AREA";
    }
}
