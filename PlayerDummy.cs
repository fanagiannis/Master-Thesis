using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDummy : MonoBehaviour
{
    public float HP;
    public GameObject hpBar;
    public Image image;
    void Awake()
    {
        image = hpBar.GetComponent<Image>();
    }

    void Update()
    {
        image.fillAmount = HP/100;
        Death();
    }
    public void TakeDamage(float value)
    {
        HP-=value;
    }
    private void Death()
    {
        if(HP<=0){this.gameObject.SetActive(false);}
    }
}
