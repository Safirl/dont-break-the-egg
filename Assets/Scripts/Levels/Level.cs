using UnityEngine;
using System;
using System.Collections;
using DG.Tweening;
using Scenes;
using UnityEngine.Serialization;
using Zones;

namespace Levels
{
    public class Level : MonoBehaviour
    {
        public static Level Instance { get; private set; }

        [SerializeField] protected CameraBehavior sceneCamera;
        [SerializeField] protected StartZone startZone;
        [FormerlySerializedAs("endZone")] [SerializeField] protected TargetZone targetZone;
        [SerializeField] private AudioSource music;

        [SerializeField] private float killedAnimationDuration = 3f;
        public delegate void OnPlayerCompletedLevel();
        public OnPlayerCompletedLevel OnPlayerKilled;
        public OnPlayerCompletedLevel OnEndReached;
        
        
        public delegate void OnLevelStartedDelegate();
        public OnLevelStartedDelegate OnLevelStarted;
    
        public delegate void OnLevelEndedDelegate(Scenes.Scenes nextScene);
        public OnLevelEndedDelegate OnLevelEnded;
        
        public readonly float TotalTime = 20;
        public float TimeLeft { get; private set; } = 20f;
        public bool IsLevelInitialized { get; protected set; }
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
            if (music && !GameManager.Instance.isDevMode)
            {
                music.volume = 0f;
                music.Play();
                music.DOFade(.5f, 1f);
            }
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
        
        public virtual void OnPlayerReachedEnd()
        {
            targetZone.OnZoneEntered -= OnPlayerReachedEnd;
            IsPlayerRunning = false;
            OnEndReached?.Invoke();
            StartCoroutine(KillPlayerCoroutine(Scenes.Scenes.WIN));
        }

        public void Update()
        {
            if (!IsPlayerRunning) return;
            
            TimeLeft -= Time.deltaTime;
            
            if (TotalTime - TimeLeft > TotalTime + 6f)
            {
                KillPlayer();
            }
        }

        public void KillPlayer()
        {
            if (!IsLevelInitialized || !IsPlayerRunning) return;
            IsPlayerRunning = false;
            IsLevelInitialized = false;
            OnPlayerKilled?.Invoke();
            //@TODO Trigger the end animation (camera movement, player broken etc. whatever)
            // Instead of triggering the coroutine we could wait for a callback.
            StartCoroutine(KillPlayerCoroutine(Scenes.Scenes.LOSE));
        }

        IEnumerator KillPlayerCoroutine(Scenes.Scenes nextScene)
        {
            if (music)
            {
                music.DOFade(0f, killedAnimationDuration).OnComplete(() =>
                {
                    music.Stop();
                });
            }
            yield return new WaitForSeconds(killedAnimationDuration);
            OnLevelEnded?.Invoke(nextScene);
        }
    }
}
