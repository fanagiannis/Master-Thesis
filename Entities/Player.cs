using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{   
    void Update()
    {
        Death();
        switch(currentstate)
        {
            case State.Alive:
                break;
            case State.Dead:
                this.gameObject.SetActive(false);
                break;
        }
    }
}
