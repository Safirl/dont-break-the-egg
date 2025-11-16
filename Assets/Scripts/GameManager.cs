using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Levels;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine.Events;



public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject pan;
    [SerializeField] GameObject player;
    
    
    public int currentLevel = 0;
    public GameLevel Level;
    // public delegate void OnLevelStartedDelegate();
    // public delegate void OnLevelEndedDelegate();
    //
    // public OnLevelStartedDelegate OnLevelStarted;
    // public OnLevelEndedDelegate OnLevelEnded;
    
    

    public float _totalTime { get; private set; } = 20;
    public bool IsGameRunning { get; private set; } = false;

    // [SerializeField] private CameraBehavior sceneCamera;

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

    private void Start()
    {
        
        Level = FindObjectOfType<GameLevel>();

        if (!(Level is GameLevel))
            throw new Exception("No Level found");

        
        Level.Start();
    }
    
    public void OnIntroFinished()
    {
        IsGameRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsGameRunning) return;
        _totalTime -= Time.deltaTime;
        // print (_totalTime);
    }
    


    void Win()
    {
        if(!(Level is GameLevel)) throw new Exception("level doesn't exist, level : " + currentLevel, null);
        
        Level.Win();
    }

    void Lose(EndingLevelStatus cause)
    {
        if(!(Level is GameLevel)) throw new Exception("level doesn't exist, level : " + currentLevel, null);
        
        Level.Lose(cause);
        
    }
    

}
