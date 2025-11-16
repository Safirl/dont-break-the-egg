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
  
    
    public float _totalTime { get; private set; } = 20;
    public bool IsGameRunning { get; private set; } = false;
    
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
        IsGameRunning = true;
        
        
        Level = FindObjectOfType<GameLevel>();

        if (!(Level is GameLevel))
            throw new Exception("No Level found");

        
        Level.Start();
    }
    

    // Update is called once per frame
    void Update()
    {
        if (!IsGameRunning) return;
        _totalTime -= Time.deltaTime;
        // print (_totalTime);
    }



    public void StartLevel()
    {
        Level.StartLevel();
    }
    public void WinLevel()
    {
        if(!(Level is GameLevel)) throw new Exception("level doesn't exist, level : " + currentLevel, null);
        
        Level.Win();
    }

    public  void LoseLevel(EndingLevelStatus cause)
    {
        if(!(Level is GameLevel)) throw new Exception("level doesn't exist, level : " + currentLevel, null);
        
        Level.Lose(cause);
        
    }
    

}
