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
        
        public delegate void OnLevelStartedDelegate();
        public delegate void OnLevelEndedDelegate();
    
        public OnLevelStartedDelegate OnLevelStarted;
        public OnLevelEndedDelegate OnLevelEnded;
        
        private readonly float _totalTime = 20;
        public float TimeLeft { get; private set; }
        public bool IsLevelRunning { get; private set; }
        public SceneManager SceneManager;
        
        protected virtual void Awake()
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
            float v = _totalTime - Time.deltaTime;


            // if (v <= 0)
            // {
            //     EndLevel(false);
            //     return;
            // }   

            TimeLeft = v > 0f ? v : 0f;
        }

        public void EndLevel(bool isWon)
        {
            IsLevelRunning = false;
            print("is won ? " + isWon);
            

            switch (cause)
            {
                case  EndingLevelStatus.BROKEN:
                case  EndingLevelStatus.CATCHED:
                case  EndingLevelStatus.TIMEOUT:
                    SceneManager.LoadNextLevel(Scenes.Scenes.LOSE);
                    break;
                
                case  EndingLevelStatus.WON:
                    SceneManager.LoadNextLevel(Scenes.Scenes.WIN);
                    break;
                
                
                case  EndingLevelStatus.NEXT:
                    SceneManager.LoadNextLevel(Scenes.Scenes.NEXT_LEVEL);
                    break;
                
                case  EndingLevelStatus.RESUME:
                    SceneManager.LoadNextLevel(Scenes.Scenes.SAME_LEVEL);
                    break;
                    
                    
                default:
                    throw new Exception("ERROR: the end cause is not handled" +  cause);

            }
        }

        public void StartLevel()
        {
            IsLevelRunning = true;
        }

        
        public void Win()
        {
            EndLevel(true);
        }

        public void Lose()
        {
            EndLevel(false);
            OnLevelEnded?.Invoke();
        }
    }
}
