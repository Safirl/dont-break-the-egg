using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public bool isDevMode;
    
    public static GameManager Instance
    {
        get; private set;
    }

    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        SceneManager.sceneLoaded += (arg0, mode) =>
        {
            var debugObjects = GameObject.FindGameObjectsWithTag("Debug");
            foreach (var debugObject in debugObjects)
            {
                debugObject.GetComponent<MeshRenderer>().enabled = isDevMode;
            }
        };
    }
}
