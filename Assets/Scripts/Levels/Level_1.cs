using System;
using Levels;
using UnityEngine;

public class Level_1 : GameLevel
{

    
    private Transform panPosition;
    private Transform playerPosition;
    
    
    public override void OnStart()
    {
        print(GameObject.FindWithTag("Player"));
        panPosition = GameObject.FindWithTag("Target").transform;
        playerPosition = GameObject.FindWithTag("Player").transform;
        sceneCamera.SetTargetPosition(playerPosition);
    }

    public override void OnUpdate()
    {
        if(sceneCamera.isIntroAnimationComplete) sceneCamera.SetTargetPosition(playerPosition);
    }

    public override void OnLevelStarting()
    {
        // throw new NotImplementedException();
    }
    
}
