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
    [SerializeField] private Vector3 targetRotation;
    // Vector3(47.4348106,145.658997,1.26217844e-06)
    [SerializeField] private float damping = .8f;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 startRotation;

    public delegate void OnIntroAnimationCompletedDelegate();

    public OnIntroAnimationCompletedDelegate OnIntroAnimationCompleted;

    private void Start()
    {
        if (!Level.Instance)
        {
            Debug.LogWarning("CAMERABEHAVIOR : GameLevel doesn't exist");
            return; 
        }
        Level.Instance.OnLevelStarted += OnLevelStarted;
    }

    public void OnLevelStarted()
    {
        Level.Instance.OnLevelStarted -= OnLevelStarted;
        if (!GameManager.Instance.isDevMode)
        {
            // transform.position = targetPosition.position + offset;
            transform.DOMove(targetRotation, 10f).SetEase(Ease.InOutExpo);
            transform.DOMove(targetPosition.position + offset, 10f).SetEase(Ease.InOutExpo)
                .OnComplete(() => {
                    OnIntroAnimationCompleted.Invoke();
                });
        } else {
            transform.position = targetPosition.position + offset;
            OnIntroAnimationCompleted.Invoke();
        }
    }

    private void LateUpdate()
    {
        if (!Level.Instance || !Level.Instance.IsPlayerRunning) return;
        if (!targetPosition) return;
        
        Vector3 newTargetPosition = targetPosition.position + offset;

        float compareDistSqr = .001f;
        
        if (newTargetPosition == transform.position) return;
        
        if ((newTargetPosition - transform.position).sqrMagnitude <= compareDistSqr)
        {
            transform.position = newTargetPosition;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, newTargetPosition, 1 - Mathf.Exp(-damping * Time.deltaTime));
        }
    }
    
    
    private void OnDisable()
    {
        if (!Level.Instance)
        {
            Level.Instance.OnLevelStarted -= OnLevelStarted;
        }
    }

}
