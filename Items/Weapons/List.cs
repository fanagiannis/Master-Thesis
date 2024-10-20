using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
[CreateAssetMenu (fileName = "List", menuName = "New ScriptableObject/List")]
public class List : ScriptableObject
{
    [SerializeField]public List<GameObject> prefabList;
    public GameObject Item(int index)
    {
        return prefabList[index];
    }
}

