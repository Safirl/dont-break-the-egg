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
        }
    }
    
    private IEnumerator StartGameCoroutine()
    {
        yield return null; // Attend 1 frame pour que tous les objets soient initialisés
        // StartGame();
    }
    //
    //
    // public void StartLevel()
    // {
    //     if (!levelManager) return;
    //     levelManager.StartLevel();
    // }
    // public void WinLevel()
    // {
    //     if (!levelManager) return;
    //     levelManager.Win();
    // }
    //
    // public  void LoseLevel()
    // {
    //     if (!levelManager) return;
    //     levelManager.Lose();
    // }
}
