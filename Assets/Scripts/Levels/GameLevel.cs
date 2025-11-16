using UnityEngine;
using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Levels;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine.Events;

namespace Levels
{
    public abstract class GameLevel : MonoBehaviour
    {

        public static GameLevel Instance { get; private set; }

        [SerializeField] private CameraBehavior sceneCamera;
        
        
        public delegate void OnLevelStartedDelegate();
        public delegate void OnLevelEndedDelegate();
    
        public OnLevelStartedDelegate OnLevelStarted;
        public OnLevelEndedDelegate OnLevelEnded;
        
        private float _totalTime = 20;
        public bool IsLevelRunning { get; private set; } = false;
        public bool IsLevelWon { get; private set; } = false;

        
        protected virtual void Awake()
        {
            Instance = this;
        }
        
        
        public void Start()
        {
            OnStart();
            sceneCamera.OnIntroAnimationCompleted += OnIntroFinished;

            StartCoroutine(StartGameCoroutine());
        }
        
        public void OnIntroFinished()
        {
            IsLevelRunning = true;
            OnLevelStarting();
        }

        public void Update()
        {
            OnUpdate();
            if (!IsLevelRunning) return;
            float v = _totalTime - Time.deltaTime;


            if (v <= 0)
            {
                EndLevel(false, EndingLevelStatus.TIMEOUT);
                return;
            }

            _totalTime = v > 0f ? v : 0f;

        }

        public void EndLevel(bool isWon, EndingLevelStatus cause)
        {
            IsLevelRunning = false;
            IsLevelWon = isWon;
        }
        
        
        private IEnumerator StartGameCoroutine()
        {
            yield return null; // Attend 1 frame pour que tous les objets soient initialisés
            print("GAMELEVEL : On LevelStarting" + OnLevelStarted);
            OnLevelStarted?.Invoke();

        }
        
        public void Win()
        {
            EndLevel(true, EndingLevelStatus.WON);   
        }

        public void Lose(EndingLevelStatus cause)
        {
            EndLevel(false, cause);
            OnLevelEnded?.Invoke();
        }
        
        public void OnStart(){}
        public void OnUpdate(){}
        public void OnLevelStarting(){}
    }
}
