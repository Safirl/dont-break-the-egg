
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.Serialization;

namespace Scenes
{
    public class SceneManager: MonoBehaviour
    {

        public SceneAsset CurrentLevel { get; private set; }
        public List<SceneAsset> Levels {get; private set;}
        public Dictionary<Scenes, SceneAsset> MainScenes {get; private set;}
        public Scenes CurrentScene { get; private set; }
        public GameManager gameManager;
        public Animator transition;

        public static SceneManager Instance { get; private set; }

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

        public void LoadLevel(Scenes requestedScene)
        {
            if (requestedScene == CurrentScene)
            {
                Debug.LogWarning("Scene " + CurrentScene + " is already loaded");
                return;
            }

            if (requestedScene == Scenes.NEXT_LEVEL)
            {
                var currentLevelIndex = Levels.FindIndex(x => x == CurrentLevel);
                //First level
                if (currentLevelIndex == -1)
                {
                    CurrentLevel = Levels[0];
                    StartCoroutine(TransitionScene(CurrentLevel.name));
                }
                //Last level
                else if (currentLevelIndex + 1 == Levels.Count)
                {
                    CurrentLevel = Levels[currentLevelIndex+1];
                    if (!CurrentLevel)
                    {
                        requestedScene = Scenes.WIN;
                    }
                }
                else
                {
                    CurrentLevel = Levels[currentLevelIndex];
                    StartCoroutine(TransitionScene(CurrentLevel.name));
                    return;
                }
            }

            var sceneName = MainScenes[requestedScene].name;;
            if (sceneName == "")
            {
                Debug.LogError("Scene " + requestedScene + " doesn't exist");
            }
            StartCoroutine(TransitionScene(sceneName));
        }

        private IEnumerator TransitionScene(string sceneName)
        {
            transition.SetTrigger("ChangeScene");
            yield return new WaitForSeconds(1);
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }
}