using UnityEngine;
using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Levels;
using Scenes;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine.Events;

namespace Levels
{
    public abstract class GameLevel : MonoBehaviour
    {

        public static GameLevel Instance { get; private set; }

        [SerializeField] protected CameraBehavior sceneCamera;
        
        
        public delegate void OnLevelStartedDelegate();
        public delegate void OnLevelEndedDelegate();
    
        public OnLevelStartedDelegate OnLevelStarted;
        public OnLevelEndedDelegate OnLevelEnded;
        
        private float _totalTime = 20;
        private bool _isRacing = false;
        public bool IsLevelRunning { get; private set; } = false;
        public bool IsLevelWon { get; private set; } = false;
        public SceneManager SceneManager;
        
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
        
        private IEnumerator StartGameCoroutine()
        {
            yield return null; // Attend 1 frame pour que tous les objets soient initialisés
            OnLevelStarted?.Invoke();

        }
        
        public void OnIntroFinished()
        {
            IsLevelRunning = true;
            OnLevelStarting();
        }

        public void Update()
        {
            if (!IsLevelRunning) return;
            
            //OnBeforeUpdate method could be called here if we need in the future
            if (_isRacing)
            {
                float v = _totalTime - Time.deltaTime;


                if (v <= 0)
                {
                    EndLevel(false, EndingLevelStatus.TIMEOUT);
                    return;
                }   

                _totalTime = v > 0f ? v : 0f;
                
            }
            OnUpdate();
        }

        public void EndLevel(bool isWon, EndingLevelStatus cause)
        {
            _isRacing = false;
            IsLevelRunning = false;
            IsLevelWon = isWon;
            print("is won ? " + isWon + cause);

            switch (cause)
            {
                case  EndingLevelStatus.BROKEN:
                case  EndingLevelStatus.CATCHED:
                case  EndingLevelStatus.TIMEOUT:
                    SceneManager.LoadNextLevel(RequestScene.LOSE);
                    break;
                
                case  EndingLevelStatus.WON:
                    SceneManager.LoadNextLevel(RequestScene.WIN);
                    break;
                
                
                case  EndingLevelStatus.NEXT:
                    SceneManager.LoadNextLevel(RequestScene.NEXT_SCENE);
                    break;
                
                case  EndingLevelStatus.RESUME:
                    SceneManager.LoadNextLevel(RequestScene.SAME_SCENE);
                    break;
                    
                    
                default:
                    throw new Exception("ERROR: the end cause is not handled" +  cause);

            }
        }

        public void StartLevel()
        {
            _isRacing = true;
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
        
        public abstract void OnStart();
        public abstract void OnUpdate();
        public abstract void OnLevelStarting();
    }
}
