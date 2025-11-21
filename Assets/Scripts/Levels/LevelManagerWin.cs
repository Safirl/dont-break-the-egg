using System;
using Levels;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LevelManagerWin : LevelManager
{
    public Button nextButton;

    public override void OnStart()
    {
        nextButton.onClick.RemoveAllListeners(); 
        nextButton.onClick.AddListener(OnButtonClicked);
    }

    public override void OnUpdate()
    {

    }

    public override void OnLevelStarting()
    {
        
    }

    void OnButtonClicked()
    {
        EndLevel(true, EndingLevelStatus.NEXT);
    }
    
}
