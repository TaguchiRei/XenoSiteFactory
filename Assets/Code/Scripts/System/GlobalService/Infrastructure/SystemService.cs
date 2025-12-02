using System;
using UnityEngine;

public class SystemService : MonoBehaviour
{
    public static SystemService Instance;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    
}