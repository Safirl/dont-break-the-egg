using System;
using Levels;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LevelLose : Level
{
    public Button nextButton;

    public new void Start()
    {
        // nextButton.onClick.RemoveAllListeners(); 
        nextButton.onClick.AddListener(OnButtonClicked);
    }

    void OnButtonClicked()
    {
        nextButton.onClick.RemoveListener(OnButtonClicked);
        OnLevelEnded?.Invoke(Scenes.Scenes.SAME_LEVEL);
    }
    
}
