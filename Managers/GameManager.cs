using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.UIElements;

public class GamemodeManager : MonoBehaviour
{
    public static GamemodeManager Instance;

    void Start()
    {
        Instance = this;
    }
}
