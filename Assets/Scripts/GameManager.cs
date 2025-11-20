using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Levels;
using Scenes;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine.Events;



public class GameManager : MonoBehaviour
{
    
    public ScenesOrder currentLevel = ScenesOrder.LEVEL_1;
    public GameLevel Level;
    public ScenesOrder currentScene = ScenesOrder.LEVEL_1;
    
    public float _totalTime { get; private set; } = 20;
    public bool IsGameRunning { get; private set; } = false;

    // public UnityEvent onLevelStarted = new UnityEvent();
    // public UnityEvent onLevelEnded = new UnityEvent();

    public float timeLeft { get; private set; }

    [SerializeField] private CameraBehavior sceneCamera;

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
        
        Level = FindFirstObjectByType<GameLevel>();
        timeLeft = _totalTime;

        if (!Level)
            throw new Exception("No Level found");
        
        Level.Start();
    }
    
    private IEnumerator StartGameCoroutine()
    {
        yield return null; // Attend 1 frame pour que tous les objets soient initialisés
        // StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsGameRunning) return;
        timeLeft -= Time.deltaTime;
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
