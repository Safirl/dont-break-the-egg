using UnityEngine;
using System;
using System.Collections;
using Scenes;
using UnityEngine.Serialization;
using Zones;

namespace Levels
{
    public class Level : MonoBehaviour
    {
        public static Level Instance { get; private set; }

        [SerializeField] protected CameraBehavior sceneCamera;
        [SerializeField] private StartZone startZone;
        [FormerlySerializedAs("endZone")] [SerializeField] private TargetZone targetZone;
    
        public delegate void OnLevelStartedDelegate();
        public OnLevelStartedDelegate OnLevelStarted;
    
        public delegate void OnLevelEndedDelegate(Scenes.Scenes nextScene);
        public OnLevelEndedDelegate OnLevelEnded;
        
        public readonly float TotalTime = 20;
        public float TimeLeft { get; private set; } = 20f;
        public bool IsLevelInitialized { get; private set; }
        public bool IsPlayerRunning { get; private set; }
        
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
            if (!sceneCamera || !startZone || !targetZone)
            {
                sceneCamera = FindAnyObjectByType<CameraBehavior>();
                startZone = FindAnyObjectByType<StartZone>();
                targetZone = FindAnyObjectByType<TargetZone>();
                
                if (!sceneCamera || !startZone || !targetZone)
                {
                    Debug.LogError("One of the required object is not set");
                    return;
                }
            }
            sceneCamera.OnIntroAnimationCompleted += OnIntroFinished;
            startZone.OnZoneExited += OnPlayerMoved;
            targetZone.OnZoneEntered += OnPlayerReachedEnd;
            StartCoroutine(StartLevelCoroutine());
        }
        
        private IEnumerator StartLevelCoroutine()
        {
            yield return null; // Attend 1 frame pour que tous les objets soient initialisés
            OnLevelStarted?.Invoke();
        }
        
        public void OnIntroFinished()
        {
            sceneCamera.OnIntroAnimationCompleted -= OnIntroFinished;
            IsLevelInitialized = true;
        }

        private void OnPlayerMoved()
        {
            startZone.OnZoneExited -= OnPlayerMoved;
            IsPlayerRunning = true;
        }
        
        private void OnPlayerReachedEnd()
        {
            targetZone.OnZoneEntered -= OnPlayerReachedEnd;
            IsLevelInitialized = false;
            OnLevelEnded?.Invoke(Scenes.Scenes.WIN);
        }

        public void Update()
        {
            if (!IsPlayerRunning) return;
            
            TimeLeft -= Time.deltaTime;
        }
    }
}
