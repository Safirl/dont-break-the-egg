using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Levels;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace Scenes
{
    public class LevelManager: MonoBehaviour
    {

        public SceneAsset CurrentLevel { get; private set; }
        [SerializeField] private List<SceneAsset> Levels;
        [SerializeField] private SceneAsset winScene;
        [SerializeField] private SceneAsset loseScene;

        public Scenes CurrentScene { get; private set; } = Scenes.NONE;
        private string nextSceneName;
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
                transition.SetTrigger("FadeOut");
                BindToLevelDelegates();
            };
            
            //For development perspectives, if we are directly in a level we want to assign it
            if (!Level.Instance || !GameManager.Instance || !GameManager.Instance.isDevMode) return;
            BindToLevelDelegates();
        }
        public void BindToLevelDelegates()
        {
            if (!Level.Instance) return;
            print("binding level delegates");
            var currentScene = SceneManager.GetActiveScene();
            CurrentLevel = Levels.Find(x => x.name == currentScene.name);
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

            switch (requestedScene)
            {
                case Scenes.NEXT_LEVEL:
                {
                    //First level
                    if (!CurrentLevel)
                    {
                        TransitionScene(Levels[0].name);
                    }
                    //Last level
                    var currentLevelIndex = Levels.FindIndex(x => x == CurrentLevel);
                    if (currentLevelIndex == Levels.Count - 1)
                    {
                        //@TODO
                        //Trigger win event
                    }
                    else
                    {
                        // CurrentLevel = Levels[currentLevelIndex + 1];
                        TransitionScene(Levels[currentLevelIndex + 1].name);
                    }

                    break;
                }
                case Scenes.SAME_LEVEL:
                    TransitionScene(CurrentLevel.name);
                    break;
                case Scenes.LOSE:
                    TransitionScene(loseScene.name);
                    break;
                case Scenes.WIN:
                    TransitionScene(winScene.name);
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
            nextSceneName = sceneName;
            transitionAnimation.FadeOutTransitionOver += LoadNextScene;
            transition.SetTrigger("ChangeScene");
        }

        public void LoadNextScene()
        {
            transitionAnimation.FadeOutTransitionOver -= LoadNextScene;
            SceneManager.LoadScene(nextSceneName);
        }
    }
}