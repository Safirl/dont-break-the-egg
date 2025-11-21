
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


        public void LoadNextLevel(Scenes requestedScene)
        {
            if (requestedScene == CurrentScene)
            {
                Debug.LogWarning("Scene " + CurrentScene + " is already loaded");
                return;
            }

            if (requestedScene == Scenes.NEXT_LEVEL)
            {
                var currentLevelIndex = Levels.FindIndex(x => x == CurrentLevel);
                
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