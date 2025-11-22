using UnityEngine;


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
    }
}
