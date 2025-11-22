using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Levels;
using Scenes;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine.Events;
using UnityEngine.Serialization;


public class GameManager : MonoBehaviour
{
    public bool isDevMode;
    
    public static GameManager Instance
    {
        get; private set;
    }

    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
