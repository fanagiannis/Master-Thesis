using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]private Player player;
    private Image image;
    void Awake()
    {
        image = GetComponentInChildren<Image>();
    }
    void Update()=>image.fillAmount = player.HP()/100f;
}
