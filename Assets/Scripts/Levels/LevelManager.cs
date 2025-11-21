using UnityEngine;
using System;
using System.Collections;
using Scenes;

namespace Levels
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }

        [SerializeField] protected CameraBehavior sceneCamera;
        [SerializeField] private GameObject startZone;
        [SerializeField] private GameObject endZone;
    
        public delegate void OnLevelStartedDelegate();
        public OnLevelStartedDelegate OnLevelStarted;
    
        public delegate void OnLevelEndedDelegate();
        public OnLevelEndedDelegate OnLevelEnded;
        
        public readonly float TotalTime = 20;
        public float TimeLeft { get; private set; }
        public bool IsLevelRunning { get; private set; }
        
        protected virtual void Awake()
        {
            if (Instance && Instance != this) 
            { 
                Destroy(this);
                Debug.LogWarning("Another levelManager was found");
            } 
            else
            { 
                Instance = this; 
            }
        }
        
        public void Start()
        {
            // OnStart();
            sceneCamera.OnIntroAnimationCompleted += OnIntroFinished;
            StartCoroutine(StartLevelCoroutine());
        }
        
        private IEnumerator StartLevelCoroutine()
        {
            yield return null; // Attend 1 frame pour que tous les objets soient initialisés
            OnLevelStarted?.Invoke();
        }
        
        public void OnIntroFinished()
        {
            IsLevelRunning = true;
            // OnLevelStarting();
        }

        public void Update()
        {
            if (!IsLevelRunning) return;
            
            TimeLeft -= Time.deltaTime;
            float v = TotalTime - Time.deltaTime;

            // TimeLeft = v > 0f ? v : 0f;
        }

        public void EndLevel(bool isWon)
        {
            IsLevelRunning = false;
            print("is won ? " + isWon);
            SceneManager.Instance.LoadLevel(isWon ? Scenes.Scenes.WIN : Scenes.Scenes.LOSE);
        }

        public void StartLevel()
        {
            IsLevelRunning = true;
        }

        public void Lose()
        {
            EndLevel(false);
            OnLevelEnded?.Invoke();
        }
    }
}
