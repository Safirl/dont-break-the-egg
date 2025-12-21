using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Levels;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Scenes
{
    public class LevelManager: MonoBehaviour
    {

        public string CurrentLevelName { get; private set; } = "";
        [SerializeField] private List<string> levelNames;
        [SerializeField] private string winSceneName;
        [SerializeField] private string endSceneName;
        [SerializeField] private string loseSceneName;

        public Scenes CurrentScene { get; private set; } = Scenes.NONE;
        private string _nextSceneName;
        [SerializeField] private Animator transition;
        [SerializeField] private TransitionAnimation transitionAnimation;

        public static LevelManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance && Instance != this) 
            { 
                Destroy(this);
                return;
            } 
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (!transitionAnimation) transitionAnimation = GetComponentInChildren<TransitionAnimation>();
            if (!transition) transition = GetComponentInChildren<Animator>();
            SceneManager.sceneLoaded += (scene, mode) =>
            {
                if (!transition)
                {
                    Debug.LogError("transition animation not set");
                    return;
                }
                transition.ResetTrigger("fadeOut");
                transition.SetTrigger("fadeIn");
                BindToLevelDelegates();
            };
        }

        private void Start()
        {
            //If we are directly in a level we want to assign it
            if (!Level.Instance) return;
            BindToLevelDelegates();
            var currentScene = SceneManager.GetActiveScene();
            CurrentLevelName = levelNames.Find(x => x == currentScene.name) ?? "";
        }

        public void BindToLevelDelegates()
        {
            if (!Level.Instance) return;
            Level.Instance.OnLevelEnded += OnLevelEnded;
        }

        public void OnLevelEnded(Scenes nextScene)
        {
            Level.Instance.OnLevelEnded -= OnLevelEnded;
            LoadLevel(nextScene);
        }


        public void LoadLevel(Scenes requestedScene)
        {
            if (requestedScene == CurrentScene)
            {
                Debug.LogWarning("Scene " + CurrentScene + " is already loaded");
                return;
            }

            var currentLevelIndex = levelNames.FindIndex(x => x == CurrentLevelName);
            switch (requestedScene)
            {
                case Scenes.NEXT_LEVEL:
                {
                    //First level
                    if (CurrentLevelName == "")
                    {
                        CurrentLevelName = levelNames[0];
                        TransitionScene(CurrentLevelName);
                    }
                    else
                    {
                        CurrentLevelName = levelNames[currentLevelIndex + 1];
                        TransitionScene(levelNames[currentLevelIndex + 1]);
                    }

                    break;
                }
                case Scenes.SAME_LEVEL:
                    //First level
                    if (CurrentLevelName == "")
                    {
                        CurrentLevelName = levelNames[0];
                        TransitionScene(CurrentLevelName);
                    }
                    else
                    {
                        TransitionScene(CurrentLevelName);
                    }
                    break;
                case Scenes.LOSE:
                    TransitionScene(loseSceneName);
                    break;
                case Scenes.WIN:
                    //Last level
                    if (currentLevelIndex == levelNames.Count - 1)
                    {
                        CurrentLevelName = levelNames[1];
                        TransitionScene(endSceneName);
                        break;
                    }
                    TransitionScene(winSceneName);
                    break;
            }

            CurrentScene = requestedScene;
        }

        private void TransitionScene(string sceneName)
        {
            if (!transition || !transitionAnimation)
            {
                Debug.LogError("transition animation not set");
                return;
            };
            _nextSceneName = sceneName;
            transitionAnimation.FadeOutTransitionOver += LoadNextScene;
            transition.ResetTrigger("fadeIn");
            transition.SetTrigger("fadeOut");
        }

        public void LoadNextScene()
        {
            transitionAnimation.FadeOutTransitionOver -= LoadNextScene;
            SceneManager.LoadScene(_nextSceneName);
        }
    }
}