using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGuardHPbar : MonoBehaviour
{
    [SerializeField]private float hp; 
    void Update()
    {
        hp=GetComponentInParent<Guard>().HP();
        GetComponent<Image>().fillAmount=hp/100;
    }
}
