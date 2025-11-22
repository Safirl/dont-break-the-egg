
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

        public SceneAsset CurrentLevel { get; private set; }
        [SerializeField] private List<SceneAsset> Levels;
        public Dictionary<Scenes, SceneAsset> MainScenes;
        public Scenes CurrentScene { get; private set; }
        public GameManager gameManager;
        public Animator transition;

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
            
            //For development perspectives, if we are directly in a level we want to assign it
            if (!Level.Instance || !GameManager.Instance.isDevMode) return;
            var currentScene = SceneManager.GetActiveScene();
            CurrentLevel = Levels.Find(x => x.name == currentScene.name);
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
                        CurrentLevel = Levels[0];
                        StartCoroutine(TransitionScene(CurrentLevel.name));
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
                        CurrentLevel = Levels[currentLevelIndex + 1];
                        StartCoroutine(TransitionScene(CurrentLevel.name));
                    }

                    break;
                }
                case Scenes.SAME_LEVEL:
                    StartCoroutine(TransitionScene(CurrentLevel.name));
                    break;
                default:
                    var sceneName = MainScenes[requestedScene].name;;
                    if (sceneName == "")
                    {
                        Debug.LogError("Scene " + requestedScene + " doesn't exist");
                    }
                    StartCoroutine(TransitionScene(sceneName));
                    break;
            }
        }

        private IEnumerator TransitionScene(string sceneName)
        {
            transition.SetTrigger("ChangeScene");
            yield return new WaitForSeconds(1);
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }
}