using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    [SerializeField]private Player player;
    [SerializeField]private float staminaregen,staminadec;
    void SprintDec(){if(player.Data.GetIsSprinting()){player.Data.AddStamina(-staminadec);};}
    void StaminaRegen(){if(player.Data.Stamina()<player.Data.MaxStamina()){player.Data.AddStamina(staminaregen);}}
    public bool Exhausted(){if(player.Data.Stamina()<=0){return true;}else {return false;}}
    void Update(){SprintDec();StaminaRegen();}
}
