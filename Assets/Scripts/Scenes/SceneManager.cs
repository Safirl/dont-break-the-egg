
using System;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using Levels;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Scenes
{
    public class SceneManager: MonoBehaviour
    {

        private ScenesOrder currentScene;
        private ScenesOrder currentLevel;
        public GameManager gameManager;
        public Animator transition;
        
        public void Start()
        {
            currentLevel = gameManager.currentLevel;
            
        } 


        public void LoadNextLevel(RequestScene requestedScene)
        {

            switch (requestedScene)
            {
                case  RequestScene.LOSE:
                    ChangeScene(ScenesOrder.LOSING);
                    break;
                
                case  RequestScene.WIN:
                    ChangeScene(ScenesOrder.WINNING);
                    break;
                
                case  RequestScene.NEXT_SCENE:
                    int nextLevelInt = (int)currentLevel + 1;

                    if (!System.Enum.IsDefined(typeof(ScenesOrder), nextLevelInt))
                        throw new Exception("Level not defined in ScenesOrder ENUM. value: " +  nextLevelInt);

                    ChangeScene((ScenesOrder)nextLevelInt);
                        
                    break;
                    
                    
                default:
                    throw new Exception("ERROR: Requested scene is not handled" +  requestedScene);
                        
                    
                    
            }

        }

        private void ChangeScene(ScenesOrder scene)
        {

            StartCoroutine(TransitionScene(scene));
        }

        private IEnumerator TransitionScene(ScenesOrder scene)
        {
            transition.SetTrigger("ChangeScene");
            yield return new WaitForSeconds(1);
            UnityEngine.SceneManagement.SceneManager.LoadScene((int)scene);
        }
    }
}