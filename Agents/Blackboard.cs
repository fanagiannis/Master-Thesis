using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Blackboard : MonoBehaviour 
{
    [SerializeField]private Dictionary<string, object> data = new Dictionary<string, object>();

    void Awake()
    {
        string value = "";
        foreach (var key in data.Keys)
        {
            value+=key + ":" + data[key]+"\n";
        }
        Debug.Log(value);
    }

    public void SetValue<T>(string key, T value)
    {
        data[key] = value;
    }

    public T GetValue<T>(string key)
    {
        if (data.TryGetValue(key, out var value) && value is T typedValue)
        {
            return typedValue;
        }
        return default;
    }

    public bool HasKey(string key)
    {
        return data.ContainsKey(key);
    }

    public void RemoveKey(string key)
    {
        data.Remove(key);
    }

}
