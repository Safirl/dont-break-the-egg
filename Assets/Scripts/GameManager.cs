using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject pan;
    [SerializeField] GameObject player;
    public delegate void OnLevelStartedDelegate();
    public delegate void OnLevelEndedDelegate();
    
    public OnLevelStartedDelegate OnLevelStarted;
    public OnLevelEndedDelegate OnLevelEnded;
    
    // public UnityEvent onLevelStarted = new UnityEvent();
    // public UnityEvent onLevelEnded = new UnityEvent();
    
    private int currentLevel = 0;

    private float _totalTime = 20;
    public bool HasLevelStarted { get; private set; } = false;

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
        sceneCamera.OnIntroAnimationCompleted += OnIntroFinished;
        StartCoroutine(StartGameCoroutine());
    }
    
    private IEnumerator StartGameCoroutine()
    {
        yield return null; // Attend 1 frame pour que tous les objets soient initialisés
        StartGame();
    }
    

    public void OnIntroFinished()
    {
        HasLevelStarted = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!HasLevelStarted) return;
        _totalTime -= Time.deltaTime;
        print (_totalTime);
    }

    void StartGame()
    {
        StartLevel();
    }

    void StartLevel()
    {
        OnLevelStarted?.Invoke();
    }

    void EndLevel()
    {
        OnLevelEnded?.Invoke();
    }
}
