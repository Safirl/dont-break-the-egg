using System;
using Levels;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LevelWin : Level
{
    public Button nextButton;

    public new void Start()
    {
        nextButton.onClick.AddListener(OnButtonClicked);
    }

    void OnButtonClicked()
    {
        nextButton.onClick.RemoveListener(OnButtonClicked);
        OnLevelEnded?.Invoke(Scenes.Scenes.NEXT_LEVEL);
    }
    
}
