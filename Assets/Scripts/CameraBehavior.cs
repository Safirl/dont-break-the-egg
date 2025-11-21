using System;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using Levels;
using UnityEngine.Events;

public class CameraBehavior : MonoBehaviour
{
    // [SerializeField] private Transform playerPosition;
    [SerializeField] private Transform targetPosition;
    [SerializeField] private float damping = .8f;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 startRotation;
    public bool isIntroAnimationComplete = false;

    public delegate void OnIntroAnimationCompletedDelegate();

    public OnIntroAnimationCompletedDelegate OnIntroAnimationCompleted;

    private void Awake()
    {
        if (!LevelManager.Instance) Debug.LogError("CAMERABEHAVIOR : GameLevel doesn't exist"); 
        
        LevelManager.Instance.OnLevelStarted += OnLevelStarted;
        
    }

    public void OnLevelStarted()
    {
        if (!GameManager.Instance.isDevMode)
        {
            transform.position = targetPosition.position + offset;
            
            transform.DOMove(targetPosition.position + offset, 10f).SetEase(Ease.InOutExpo)
                .OnComplete(() => {
                    OnIntroAnimationCompleted.Invoke();
                    isIntroAnimationComplete = true;
                });
        } else {
            transform.position = targetPosition.position + offset;
            OnIntroAnimationCompleted.Invoke();
            isIntroAnimationComplete = true;
        }
    }

    private void LateUpdate()
    {
        if (!LevelManager.Instance.IsLevelRunning) return;
        
        Vector3 newTargetPosition = targetPosition.position + offset;

        float compareDistSqr = .001f;
        
        if (newTargetPosition == transform.position) return;
        
        if ((newTargetPosition - transform.position).sqrMagnitude <= compareDistSqr)
        {
            transform.position = newTargetPosition;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition.position, 1 - Mathf.Exp(-damping * Time.deltaTime));
        }
    }
    
    
    private void OnDisable()
    {
        if (!LevelManager.Instance)
        {
            LevelManager.Instance.OnLevelStarted -= OnLevelStarted;
        }
    }

}
