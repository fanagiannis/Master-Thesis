using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UISecurityState : MonoBehaviour
{
    private TextMeshProUGUI UISecurityCurrentState;
    void Awake()
    {
        UISecurityCurrentState=this.GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        if(SecurityManager.Instance.State()==SecurityManager.SecurityState.Idle.ToString())
        {
            UISecurityCurrentState.text = "";
        }
        else
        {
            UISecurityCurrentState.text = SecurityManager.Instance.State();
        }
        
    }
}
