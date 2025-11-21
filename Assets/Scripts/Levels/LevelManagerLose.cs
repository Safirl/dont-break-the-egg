using System;
using Levels;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LevelManagerLose : LevelManager
{
    public Button nextButton;

    public void Sta()
    {
        nextButton.onClick.RemoveAllListeners(); 
        nextButton.onClick.AddListener(OnButtonClicked);
    }

    // void OnButtonClicked()
    // {
    //     EndLevel(true, EndingLevelStatus.RESUME);
    // }
    
}
